namespace Aylos.Xrm.Sdk.Plugins
{
    using Aylos.Xrm.Sdk.Common;

    using Microsoft.Xrm.Sdk;

    using System;
    using System.Diagnostics;
    using System.Globalization;
    using System.ServiceModel;
    using System.Text;

    public abstract class CustomPlugin : IPlugin
    {
        #region Constants 

        public const string TracingServiceMessage = "Unable to get tracing service from the service manager.";
        public const string PluginExecutionContextMessage = "Unable to get plugin execution context from the service manager.";
        public const string OrganizationServiceFactoryMessage = "Unable to get organization service factory from the service manager.";
        public const string NotificationServiceMessage = "Unable to get service endpoint notification service from the service manager.";
        public const string CurrentUserServiceMessage = "Unable to create the current user service.";
        public const string SystemUserServiceMessage = "Unable to create the system user service.";

        #endregion

        #region Properties

        public static string PrimaryEntityLogicalName { get; set; }

        public static string PluginMessageName { get; set; }

        public static int PluginPipelineStage { get; set; }

        public static int MaximumAllowedExecutionDepth { get; set; }
        
        public IOrganizationServiceFactory OrganizationServiceFactory { get; set; }
        
        public IOrganizationService CurrentUserService { get; set; }
        
        public IOrganizationService SystemUserService { get; set; }
        
        public IPluginExecutionContext PluginExecutionContext { get; set; }

        public IServiceEndpointNotificationService NotificationService { get; set; }

        public ITracingService TracingService { get; set; }
        
        public IServiceProvider ServiceProvider { get; set; }

        public string UnderlyingSystemTypeName { get { return GetType().UnderlyingSystemType.Name; } }

        public Stopwatch Stopwatch { get; private set; }

        public bool? ThrowsException { get; set; }

        public string ExceptionMessage { get; set; }

        #endregion

        #region Methods

        public void Execute(IServiceProvider serviceProvider)
        {
            if (serviceProvider == null) throw new ArgumentNullException(nameof(serviceProvider));

            try
            {
                ServiceProvider = serviceProvider;

                ThrowsException = ThrowsException.HasValue ? ThrowsException.Value : true;
                Stopwatch = Stopwatch.StartNew();

                TracingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));
                if (TracingService == null) throw new InvalidPluginExecutionException(TracingServiceMessage);

                PluginExecutionContext = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
                if (PluginExecutionContext == null) throw new InvalidPluginExecutionException(PluginExecutionContextMessage);

                OrganizationServiceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
                if (OrganizationServiceFactory == null) throw new InvalidPluginExecutionException(OrganizationServiceFactoryMessage);

                NotificationService = (IServiceEndpointNotificationService)serviceProvider.GetService(typeof(IServiceEndpointNotificationService));
                if (NotificationService == null) throw new InvalidPluginExecutionException(NotificationServiceMessage);

                CurrentUserService = OrganizationServiceFactory.CreateOrganizationService(PluginExecutionContext.UserId);
                if (CurrentUserService == null) throw new InvalidPluginExecutionException(CurrentUserServiceMessage);

                SystemUserService = OrganizationServiceFactory.CreateOrganizationService(null);
                if (SystemUserService == null) throw new InvalidPluginExecutionException(SystemUserServiceMessage);

                Trace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.StartTracing, UnderlyingSystemTypeName, Stopwatch.ElapsedMilliseconds));
                Trace(TraceHelper.Trace(PluginExecutionContext)); // Trace the plugin execution context
                Trace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.ExecutionContextTracingCompleted, UnderlyingSystemTypeName, Stopwatch.ElapsedMilliseconds));

                Validate(); // Call the overriden validate method

                Execute(); // Call the overriden execute method
            }
            catch (InvalidPluginExecutionException ex)
            {
                var sb = new StringBuilder();
                sb.AppendLine(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.InvalidPluginExecution, UnderlyingSystemTypeName, ex.Message, ex.StackTrace));

                Action<Exception> le = null;
                le = (n) => { sb.AppendLine(n.Message); sb.AppendLine(n.StackTrace); if (n.InnerException != null) le(n.InnerException); };
                le(ex);

                Trace(sb.ToString());
                ExceptionMessage = ex.Message;

                if (ThrowsException.Value) throw;
            }
            catch (FaultException<OrganizationServiceFault> ex)
            {
                var sb = new StringBuilder();
                sb.AppendLine(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.OrganizationServiceFault, UnderlyingSystemTypeName, ex.Message, ex.StackTrace));

                if (ex.Detail == null)
                {
                    ExceptionMessage = ex.Message;
                }
                else
                {
                    sb.AppendLine(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.ExceptionErrorCode, ex.Detail.ErrorCode));
                    sb.AppendLine(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.ExceptionMessage, ex.Detail.Message));
                    sb.AppendLine(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.ExceptionTrace, ex.Detail.TraceText));
                    foreach (var errorDetail in ex.Detail.ErrorDetails)
                    {
                        sb.AppendLine(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.ExceptionDetailKey, errorDetail.Key, errorDetail.Value));
                    }

                    ExceptionMessage = ex.Detail.Message;
                }

                Trace(sb.ToString());

                if (ThrowsException.Value) throw new InvalidPluginExecutionException(ExceptionMessage, ex);
            }
            catch (Exception ex)
            {
                var sb = new StringBuilder();
                sb.AppendLine(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.UndefinedFault, UnderlyingSystemTypeName, ex.Message, ex.StackTrace));

                Action<Exception> le = null;
                le = (n) => { sb.AppendLine(n.Message); sb.AppendLine(n.StackTrace); if (n.InnerException != null) le(n.InnerException); };
                le(ex);

                Trace(sb.ToString());

                ExceptionMessage = ex.Message;

                if (ThrowsException.Value) throw new InvalidPluginExecutionException(ExceptionMessage, ex);
            }
            finally
            {
                if (Stopwatch != null)
                {
                    Trace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.ExecutionContextTracingCompleted, UnderlyingSystemTypeName, Stopwatch.Elapsed.TotalMilliseconds));
                    Stopwatch.Stop();
                    Stopwatch = null;
                }

                Trace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.StopTracing, UnderlyingSystemTypeName));
            }
        }

        public void Trace(string message)
        {
            if (TracingService == null || string.IsNullOrWhiteSpace(message)) return;
            TracingService.Trace(message);
        }

        #endregion

        #region Overriden Methods

        public abstract void Execute();

        public abstract void Validate();

        #endregion
    }
}