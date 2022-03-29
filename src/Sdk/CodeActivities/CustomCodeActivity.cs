namespace Aylos.Xrm.Sdk.CodeActivities
{
    using Aylos.Xrm.Sdk.Common;
    using System;
    using System.Activities;
    using System.Diagnostics;
    using System.Globalization;
    using System.ServiceModel;
    using System.Text;
    using Microsoft.Xrm.Sdk;
    using Microsoft.Xrm.Sdk.Workflow;
    using Exceptions;

    public abstract class CustomCodeActivity : CodeActivity
    {
        public const string TracingExtensionMessage = "Unable to get tracing service extension from the code activity context.";
        public const string WorkflowExtensionMessage = "Unable to get workflow context extension from the code activity context.";
        public const string OrganizationServiceFactoryExtensionMessage = "Unable to get organization service factory extension from the code activity context.";
        public const string CurrentUserServiceMessage = "Unable to create the organization service.";
        public const string SystemUserServiceMessage = "Unable to create the impersonated organization service.";

        public CodeActivityContext CodeActivityContext { get; private set; }

        public IOrganizationServiceFactory OrganizationServiceFactory { get; private set; }

        public IOrganizationService CurrentUserService { get; set; }

        public IOrganizationService SystemUserService { get; set; }

        public IWorkflowContext WorkflowContext { get; private set; }

        public ITracingService TracingService { get; private set; }

        public string SystemTypeName { get { return GetType().UnderlyingSystemType.Name; } }

        public Stopwatch Stopwatch { get; private set; }

        public bool? ThrowsException { get; set; }

        private void Initialize()
        {
            ThrowsException = ThrowsException.HasValue ? ThrowsException.Value : true;

            Stopwatch = Stopwatch.StartNew();

            TracingService = CodeActivityContext.GetExtension<ITracingService>();
            if (TracingService == null) throw new InvalidPluginExecutionException(TracingExtensionMessage);

            WorkflowContext = CodeActivityContext.GetExtension<IWorkflowContext>();
            if (WorkflowContext == null) throw new InvalidPluginExecutionException(WorkflowExtensionMessage);

            OrganizationServiceFactory = CodeActivityContext.GetExtension<IOrganizationServiceFactory>();
            if (OrganizationServiceFactory == null) throw new InvalidPluginExecutionException(OrganizationServiceFactoryExtensionMessage);

            CurrentUserService = OrganizationServiceFactory.CreateOrganizationService(WorkflowContext.UserId);
            if (CurrentUserService == null) throw new InvalidPluginExecutionException(CurrentUserServiceMessage);

            SystemUserService = OrganizationServiceFactory.CreateOrganizationService(null);
            if (SystemUserService == null) throw new InvalidPluginExecutionException(SystemUserServiceMessage);
        }

        protected override void Execute(CodeActivityContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            try
            {
                CodeActivityContext = context;
                Initialize();
                Trace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.StartTracing, SystemTypeName, Stopwatch.ElapsedMilliseconds));
                Trace(TraceHelper.Trace(WorkflowContext));
                Trace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.ExecutionContextTracingCompleted, SystemTypeName, Stopwatch.ElapsedMilliseconds));
                Validate();
                Execute();
            }
            catch (InvalidPluginExecutionException ex)
            {
                var sb = new StringBuilder();
                sb.AppendLine(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.InvalidPluginExecution, SystemTypeName, ex.Message, ex.StackTrace));

                Action<Exception> le = null;
                le = (n) => { sb.AppendLine(n.Message); sb.AppendLine(n.StackTrace); if (n.InnerException != null) le(n.InnerException); };
                le(ex);

                Trace(sb.ToString());

                ExceptionJson ej = new ExceptionJson
                {
                    Details = sb.ToString(),
                    Message = ex.Message,
                    Source = ex.Source,
                };
                Exception.Set(CodeActivityContext, ej.Message);
                ExceptionJson.Set(CodeActivityContext, SerializationHelper.SerializeJson(ej));

                if (ThrowsException.Value) throw;
            }
            catch (FaultException<OrganizationServiceFault> ex)
            {
                var sb = new StringBuilder();
                sb.AppendLine(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.OrganizationServiceFault, SystemTypeName, ex.Message, ex.StackTrace));

                if (ex.Detail == null)
                {
                    Trace(sb.ToString());

                    ExceptionJson ej = new ExceptionJson
                    {
                        Details = sb.ToString(),
                        Message = ex.Message,
                        Source = ex.Source,
                    };
                    Exception.Set(CodeActivityContext, ej.Message);
                    ExceptionJson.Set(CodeActivityContext, SerializationHelper.SerializeJson(ej));

                    if (ThrowsException.Value) throw new InvalidPluginExecutionException(ex.Message, ex);
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

                    Trace(sb.ToString());

                    ExceptionJson ej = new ExceptionJson
                    {
                        Details = sb.ToString(),
                        Message = ex.Detail.Message,
                        Source = ex.Source,
                    };
                    Exception.Set(CodeActivityContext, ej.Message);
                    ExceptionJson.Set(CodeActivityContext, SerializationHelper.SerializeJson(ej));

                    if (ThrowsException.Value) throw new InvalidPluginExecutionException(ex.Detail.Message, ex);
                }
            }
            catch (Exception ex)
            {
                var sb = new StringBuilder();
                sb.AppendLine(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.UndefinedFault, SystemTypeName, ex.Message, ex.StackTrace));

                Action<Exception> le = null;
                le = (n) => { sb.AppendLine(n.Message); sb.AppendLine(n.StackTrace); if (n.InnerException != null) le(n.InnerException); };
                le(ex);

                Trace(sb.ToString());

                ExceptionJson ej = new ExceptionJson
                {
                    Details = sb.ToString(),
                    Message = ex.Message,
                    Source = ex.Source,
                };
                Exception.Set(CodeActivityContext, ej.Message);
                ExceptionJson.Set(CodeActivityContext, SerializationHelper.SerializeJson(ej));

                if (ThrowsException.Value) throw new InvalidPluginExecutionException(ex.Message, ex);
            }
            finally
            {
                if (Stopwatch != null)
                {
                    Trace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.ExecutionContextTracingCompleted, SystemTypeName, Stopwatch.Elapsed.TotalMilliseconds));
                    Stopwatch.Stop(); 
                    Stopwatch = null;
                }
                Trace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.StopTracing, SystemTypeName));
            }
        }

        public void Trace(string message)
        {
            if (TracingService == null || string.IsNullOrWhiteSpace(message)) return;
            TracingService.Trace(message);
        }

        public abstract void Execute();

        public abstract void Validate();

        [Output("ExceptionJson")]
        public OutArgument<string> ExceptionJson { get; set; }

        [Output("Exception")]
        public OutArgument<string> Exception { get; set; }
    }
}
