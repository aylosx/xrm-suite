namespace Shared.BusinessLogic.Services.ExecutionContext
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;

    using Microsoft.Xrm.Sdk;

    using Aylos.Xrm.Sdk.Common;

    using Shared.BusinessLogic.Services.Data;
    using Shared.Models.Domain;

    public class PostMessageService : GenericService<CrmServiceContext, ExecutionContext>, IPostMessageService
    {
        #region Static Members 

        public const string TextFormat = "Code-{0}";
        public const string DateTimeFormat = "yyyyMMddHHmmss";
        public static readonly DateTime DateTimeNow = DateTime.UtcNow;

        public const string ExecutionContextNotExistMessage = "Something went wrong, I could not find an account with the given key.";
        public const string ExecutionContextNumberAlreadySetMessage = "The account number already set, you can only set the account number once.";

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the CRM service.
        /// </summary>
        /// <value>The CRM service object instance.</value>
        public ICrmService CrmService { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// PostMessageService Constructor
        /// </summary>
        /// <param name="tracingService">ITracingService</param>
        public PostMessageService(ITracingService tracingService)
        {
            TracingService = tracingService;
        }

        /// <summary>
        /// PostMessageService Constructor
        /// </summary>
        /// <param name="pluginExecutionContext">IPluginExecutionContext</param>
        /// <param name="tracingService">ITracingService</param>
        public PostMessageService(IPluginExecutionContext pluginExecutionContext, ITracingService tracingService)
            : this(tracingService)
        {
            PluginExecutionContext = pluginExecutionContext;
        }

        /// <summary>
        /// PostMessageService Constructor
        /// </summary>
        /// <param name="crmService">ICrmService</param>
        /// <param name="pluginExecutionContext">IPluginExecutionContext</param>
        /// <param name="tracingService">ITracingService</param>
        public PostMessageService(ICrmService crmService, IPluginExecutionContext pluginExecutionContext, ITracingService tracingService)
            : this(pluginExecutionContext, tracingService)
        {
            CrmService = crmService;
        }

        /// <summary>
        /// PostMessageService Constructor
        /// </summary>
        /// <param name="crmService">ICrmService</param>
        /// <param name="organizationServiceContext">OrganizationServiceContext</param>
        /// <param name="pluginExecutionContext">IPluginExecutionContext</param>
        /// <param name="tracingService">ITracingService</param>
        public PostMessageService(ICrmService crmService, CrmServiceContext organizationServiceContext, IPluginExecutionContext pluginExecutionContext, ITracingService tracingService)
            : this(crmService, pluginExecutionContext, tracingService)
        {
            OrganizationServiceContext = organizationServiceContext;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Post the messages to the service bus queue and deletes the records from the platform entity. 
        /// Please note that the method has designed to run on the post create message event.
        /// </summary>
        public void PostMessage()
        {
            Trace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.EnteredMethod, UnderlyingSystemTypeName, MethodBase.GetCurrentMethod().Name));

            var rec = TargetBusinessEntity.ToEntity<ExecutionContext>();

            if (rec.Depth > 1)
            {
                Trace(string.Format(CultureInfo.InvariantCulture, "{0} | Depth: {2}  | {1}.", UnderlyingSystemTypeName, MethodBase.GetCurrentMethod().Name, rec.Depth));
                Trace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.AbortingMethod, UnderlyingSystemTypeName, MethodBase.GetCurrentMethod().Name));
                return;
            }

            if (!rec.Contains(Models.Domain.ExecutionContext.Fields.ContainsParentContext))
            {
                Trace(string.Format(CultureInfo.InvariantCulture, "{0} | Missing argument: {2}  | {1}.", UnderlyingSystemTypeName, MethodBase.GetCurrentMethod().Name, Models.Domain.ExecutionContext.Fields.ContainsParentContext));
                Trace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.AbortingMethod, UnderlyingSystemTypeName, MethodBase.GetCurrentMethod().Name));
                return;
            }

            if (rec.ContainsParentContext.GetValueOrDefault(false)) 
            {
                if (rec.ParentIsMainOperation.GetValueOrDefault(false)) 
                {
                    IList<ExecutionContext> recs = CrmService.GetExecutionContexts(PluginExecutionContext.CorrelationId.ToString(), PluginExecutionContext.OperationId.ToString()).ToList();
                    Trace(string.Format(CultureInfo.InvariantCulture, "{0} | Records: {2}  | {1}.", UnderlyingSystemTypeName, MethodBase.GetCurrentMethod().Name, recs.Count));
                    ExecutionContext lastRecord = recs.OrderByDescending(x => x.Subject).First();

                    if (!rec.Subject.Equals(lastRecord.Subject))
                    {
                        Trace(string.Format(CultureInfo.InvariantCulture, "{0} | Not the last record: {2}  | {1}.", UnderlyingSystemTypeName, MethodBase.GetCurrentMethod().Name, rec.Subject));
                        Trace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.AbortingMethod, UnderlyingSystemTypeName, MethodBase.GetCurrentMethod().Name));
                        return;
                    }

                    PostMessages(recs);
                }
            }
            else //ParentContext: NULL
            {
                IList<ExecutionContext> recs = CrmService.GetExecutionContexts(PluginExecutionContext.CorrelationId.ToString(), PluginExecutionContext.OperationId.ToString()).ToList();
                PostMessages(recs);
            }

            Trace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.ExitingMethod, UnderlyingSystemTypeName, MethodBase.GetCurrentMethod().Name));
        }

        #endregion

        #region Private Members

        private void PostMessages(IList<ExecutionContext> recs)
        {
            Trace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.EnteredMethod, UnderlyingSystemTypeName, MethodBase.GetCurrentMethod().Name));

            foreach (ExecutionContext ec in recs.OrderBy(x => x.Subject))
            {
                Trace(string.Format(CultureInfo.InvariantCulture, "{0} | Action: {2} {3} {4} | {1}.", UnderlyingSystemTypeName, MethodBase.GetCurrentMethod().Name, ec.MessageName, ec.EntityName, ec.EntityId));
                switch (ec.MessageName)
                {
                    case PlatformMessageHelper.Create:
                    case PlatformMessageHelper.Update:
                    case PlatformMessageHelper.Delete:
                        // TO-DO: Submit to the service bus queue
                        Trace(string.Format(CultureInfo.InvariantCulture, "{0} | Post: {2} | {1}.", UnderlyingSystemTypeName, MethodBase.GetCurrentMethod().Name, ec.Subject));
                        break;
                }

                Trace(string.Format(CultureInfo.InvariantCulture, "{0} | Delete: {2} | {1}.", UnderlyingSystemTypeName, MethodBase.GetCurrentMethod().Name, ec.Subject));
                //CrmService.DeleteEntity(ExecutionContext.EntityLogicalName, ec.Id);
            }

            Trace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.ExitingMethod, UnderlyingSystemTypeName, MethodBase.GetCurrentMethod().Name));
        }

        #endregion

        #region IDisposable Support

        private bool _disposed;

        /// <summary>
        /// Consider disposing any unmanaged resources within the dispose method
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;

            if (disposing)
            {
                
            }

            // TODO: free unmanaged resources (unmanaged objects).

            _disposed = true;
        }

        ~PostMessageService()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
