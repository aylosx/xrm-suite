namespace Shared.BusinessLogic.Services.Note
{
    using Aylos.Xrm.Sdk.Common;

    using Microsoft.Xrm.Sdk;
    using Microsoft.Xrm.Sdk.Client;

    using Shared.BusinessLogic.Services.Data;
    using Shared.Models.Domain;

    using System;
    using System.Globalization;
    using System.Reflection;

    public class InitializeEntityService : GenericService<CrmServiceContext, Note>, IInitializeEntityService
    {
        #region Static Members 

        public const string TextFormat = "Code-{0}";
        public const string DateTimeFormat = "yyyyMMddHHmmss";
        public static readonly DateTime DateTimeNow = DateTime.UtcNow;

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
        /// InitializeEntityService Constructor
        /// </summary>
        /// <param name="tracingService">ITracingService</param>
        public InitializeEntityService(ITracingService tracingService)
        {
            TracingService = tracingService;
        }

        /// <summary>
        /// InitializeEntityService Constructor
        /// </summary>
        /// <param name="pluginExecutionContext">IPluginExecutionContext</param>
        /// <param name="tracingService">ITracingService</param>
        public InitializeEntityService(IPluginExecutionContext pluginExecutionContext, ITracingService tracingService)
            : this(tracingService)
        {
            PluginExecutionContext = pluginExecutionContext;
        }

        /// <summary>
        /// InitializeEntityService Constructor
        /// </summary>
        /// <param name="crmService">ICrmService</param>
        /// <param name="pluginExecutionContext">IPluginExecutionContext</param>
        /// <param name="tracingService">ITracingService</param>
        public InitializeEntityService(ICrmService crmService, IPluginExecutionContext pluginExecutionContext, ITracingService tracingService)
            : this(pluginExecutionContext, tracingService)
        {
            CrmService = crmService;
        }

        /// <summary>
        /// InitializeEntityService Constructor
        /// </summary>
        /// <param name="crmService">ICrmService</param>
        /// <param name="organizationServiceContext">OrganizationServiceContext</param>
        /// <param name="pluginExecutionContext">IPluginExecutionContext</param>
        /// <param name="tracingService">ITracingService</param>
        public InitializeEntityService(ICrmService crmService, CrmServiceContext organizationServiceContext, IPluginExecutionContext pluginExecutionContext, ITracingService tracingService)
            : this(crmService, pluginExecutionContext, tracingService)
        {
            OrganizationServiceContext = organizationServiceContext;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Initializes the entity, by updating the target entity in the execution context.
        /// Initializes the annotation number
        /// </summary>
        public void InitializeEntity()
        {
            Trace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.EnteredMethod, UnderlyingSystemTypeName, MethodBase.GetCurrentMethod().Name));

            // TO-DO: Call the Azure Function and upload the file contents
            // TO-DO: Clear the body of the documentb nj
            PluginExecutionContext.InputParameters[PlatformConstants.TargetText] = TargetBusinessEntity.ToEntity<Entity>();

            Trace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.ExitingMethod, UnderlyingSystemTypeName, MethodBase.GetCurrentMethod().Name));
        }

        #endregion

        #region Private Members

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

        ~InitializeEntityService()
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
