namespace Shared.BusinessLogic.Services.ExecutionContext
{
    using System;
    using System.Globalization;
    using System.Reflection;

    using Microsoft.Xrm.Sdk;
    using Microsoft.Xrm.Sdk.Client;

    using Aylos.Xrm.Sdk.Common;

    using Shared.BusinessLogic.Services.Data;
    using Shared.Models.Domain;

    /// <summary>
    /// The main purpose of this module is to manage the entity 
    /// and return the new data to the caller.
    /// </summary>
    public class ManageExecutionContext : GenericService<CrmServiceContext>, IManageExecutionContext
    {
        #region Static Members 

        public static DateTime DateTimeNow = DateTime.UtcNow;
        public const string SubjectFormat = "{0} | {1} | {2} | {3}";
        public const string SystemUserEntityLogicalName = "systemuser";
        public const string DateTimeFormat = "yyyyMMddTHHmmss.fffffffK";

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
        /// ManageExecutionContext Constructor
        /// </summary>
        /// <param name="tracingService">ITracingService</param>
        public ManageExecutionContext(ITracingService tracingService)
        {
            TracingService = tracingService;
        }

        /// <summary>
        /// ManageExecutionContext Constructor
        /// </summary>
        /// <param name="pluginExecutionContext">IPluginExecutionContext</param>
        /// <param name="tracingService">ITracingService</param>
        public ManageExecutionContext(IPluginExecutionContext pluginExecutionContext, ITracingService tracingService)
            : this(tracingService)
        {
            PluginExecutionContext = pluginExecutionContext;
        }

        /// <summary>
        /// ManageExecutionContext Constructor
        /// </summary>
        /// <param name="crmService">ICrmService</param>
        /// <param name="pluginExecutionContext">IPluginExecutionContext</param>
        /// <param name="tracingService">ITracingService</param>
        public ManageExecutionContext(ICrmService crmService, IPluginExecutionContext pluginExecutionContext, ITracingService tracingService)
            : this(pluginExecutionContext, tracingService)
        {
            CrmService = crmService;
        }

        /// <summary>
        /// ManageExecutionContext Constructor
        /// </summary>
        /// <param name="crmService">ICrmService</param>
        /// <param name="organizationServiceContext">OrganizationServiceContext</param>
        /// <param name="pluginExecutionContext">IPluginExecutionContext</param>
        /// <param name="tracingService">ITracingService</param>
        public ManageExecutionContext(ICrmService crmService, CrmServiceContext organizationServiceContext, IPluginExecutionContext pluginExecutionContext, ITracingService tracingService)
            : this(crmService, pluginExecutionContext, tracingService)
        {
            OrganizationServiceContext = organizationServiceContext;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Creates a new record in the ExecutionContext entity and populates all the needed fields 
        /// including a serialized version of the PluginExecutioncontext.
        /// </summary>
        /// <returns>void()</returns>
        public void StoreExecutionContext()
        {
            Trace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.EnteredMethod, UnderlyingSystemTypeName, MethodBase.GetCurrentMethod().Name));

            // System.Threading.Thread.Sleep(5000); // intentional delay for testing purposes - to be removed

            if (PluginExecutionContext.MessageName.Equals(PlatformMessageHelper.Update)) System.Threading.Thread.Sleep(100 * PluginExecutionContext.Depth);

            DateTimeNow = DateTime.UtcNow;

            string content = SerializationHelper.SerializeJson(ExecutionContext); // does not serialize nested object ParentContext and therefore it will be NULL

            ExecutionContext ec = new ExecutionContext
            {
                ContainsParentContext = PluginExecutionContext.ParentContext != null,
                Content = content,
                ContentLength = content.Length,
                CorrelationId = PluginExecutionContext.CorrelationId.ToString(),
                Depth = PluginExecutionContext.Depth,
                EntityId = PluginExecutionContext.PrimaryEntityId.ToString(),
                EntityName = PluginExecutionContext.PrimaryEntityName,
                InitiatingUser = new EntityReference(SystemUserEntityLogicalName, PluginExecutionContext.InitiatingUserId),
                MessageName = PluginExecutionContext.MessageName,
                OperationId = PluginExecutionContext.OperationId.ToString(),
                ParentIsMainOperation = PluginExecutionContext.ParentContext != null && PluginExecutionContext.ParentContext.Stage == (int)SdkMessageProcessingStepStage.MainOperation,
                Stage = PluginExecutionContext.Stage,
                Subject = DateTimeNow.ToString(DateTimeFormat, CultureInfo.InvariantCulture),
                User = new EntityReference(SystemUserEntityLogicalName, PluginExecutionContext.UserId),
            };

            OrganizationServiceContext.AddObject(ec);

            CrmService.SaveChanges(SaveChangesOptions.None);

            Trace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.ExitingMethod, UnderlyingSystemTypeName, MethodBase.GetCurrentMethod().Name));
        }

        #endregion

        #region Private Methods

        #endregion

        #region IDisposable Support

        private bool _disposed = false; 

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

        ~ManageExecutionContext()
        {
            Dispose(false);
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
