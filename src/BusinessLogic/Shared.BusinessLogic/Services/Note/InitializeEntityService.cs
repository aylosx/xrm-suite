namespace Shared.BusinessLogic.Services.Note
{
    using Aylos.Xrm.Sdk.Common;
    using Aylos.Xrm.Sdk.Plugins;

    using Microsoft.Xrm.Sdk;

    using Shared.BusinessLogic.Services.Data;
    using Shared.Models.Domain;

    using System;
    using System.Globalization;
    using System.Net.Http;
    using System.Reflection;
    using System.Text;
    using System.Web;

    public class InitializeEntityService : GenericService<CrmServiceContext, Note>, IInitializeEntityService
    {
        #region Static Members 

        public const string TextFormat = "Code-{0}";
        public const string DateTimeFormat = "yyyyMMddHHmmss";
        public static readonly DateTime DateTimeNow = DateTime.UtcNow;

        public const string PortalEntitiesPrefix = "adx_";

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the CRM service.
        /// </summary>
        /// <value>The CRM service object instance.</value>
        public ICrmService CrmService { get; set; }

        /// <summary>
        /// Gets or sets the HTTP client.
        /// </summary>
        /// <value>The HTTP client object instance.</value>
        public HttpClient HttpClient { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// InitializeEntityService Constructor
        /// </summary>
        /// <param name="tracingService">ITracingService</param>
        public InitializeEntityService(ITracingService tracingService, HttpClient httpClient)
        {
            TracingService = tracingService;
            HttpClient = httpClient;
        }

        /// <summary>
        /// InitializeEntityService Constructor
        /// </summary>
        /// <param name="pluginExecutionContext">IPluginExecutionContext</param>
        /// <param name="tracingService">ITracingService</param>
        public InitializeEntityService(IPluginExecutionContext pluginExecutionContext, ITracingService tracingService, HttpClient httpClient)
            : this(tracingService, httpClient)
        {
            PluginExecutionContext = pluginExecutionContext;
        }

        /// <summary>
        /// InitializeEntityService Constructor
        /// </summary>
        /// <param name="crmService">ICrmService</param>
        /// <param name="pluginExecutionContext">IPluginExecutionContext</param>
        /// <param name="tracingService">ITracingService</param>
        public InitializeEntityService(ICrmService crmService, IPluginExecutionContext pluginExecutionContext, ITracingService tracingService, HttpClient httpClient)
            : this(pluginExecutionContext, tracingService, httpClient)
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
        public InitializeEntityService(ICrmService crmService, CrmServiceContext organizationServiceContext, IPluginExecutionContext pluginExecutionContext, ITracingService tracingService, HttpClient httpClient)
            : this(crmService, pluginExecutionContext, tracingService, httpClient)
        {
            OrganizationServiceContext = organizationServiceContext;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Initializes the entity, by updating the target entity as defined in the business logic. Submits 
        /// the annotation body for scanning and uploads it to the storage, then clears the body and
        /// updates the execution context with the new version of the annotation.
        /// </summary>
        public void InitializeEntity()
        {
            Trace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.EnteredMethod, UnderlyingSystemTypeName, MethodBase.GetCurrentMethod().Name));

            // Exit if the annotation is not an attachment
            if (TargetBusinessEntity.IsDocument.GetValueOrDefault(false))
            {
                Trace(string.Format(CultureInfo.InvariantCulture, "{0} | Annotation is not an attachment rule applied | {1}.", UnderlyingSystemTypeName, MethodBase.GetCurrentMethod().Name));
                Trace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.AbortingMethod, UnderlyingSystemTypeName, MethodBase.GetCurrentMethod().Name));
                return;
            }

            // Exit if the parent entity is excluded
            if (TargetBusinessEntity.Regarding.LogicalName.StartsWith(PortalEntitiesPrefix)) 
            {
                Trace(string.Format(CultureInfo.InvariantCulture, "{0} | Excluded entity rule applied | {1}.", UnderlyingSystemTypeName, MethodBase.GetCurrentMethod().Name));
                Trace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.AbortingMethod, UnderlyingSystemTypeName, MethodBase.GetCurrentMethod().Name));
                return;
            }

            // Exit if the parent entity cannot be accessed
            Entity parentEntity = CrmService.RetrieveEntity(TargetBusinessEntity.Regarding.LogicalName, TargetBusinessEntity.Regarding.Id);
            if (parentEntity == null) 
            {
                Trace(string.Format(CultureInfo.InvariantCulture, "{0} | Parent entity access rule applied | {1}.", UnderlyingSystemTypeName, MethodBase.GetCurrentMethod().Name));
                Trace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.AbortingMethod, UnderlyingSystemTypeName, MethodBase.GetCurrentMethod().Name));
                return;
            }

            // Call the Webhook Plugin (upload the file)
            UploadExecutionContext(ExecutionContext);

            // Clear the body of the document
            TargetBusinessEntity.Document = null; 

            // Update the Plugin execution context
            PluginExecutionContext.InputParameters[PlatformConstants.TargetText] = TargetBusinessEntity.ToEntity<Entity>();

            Trace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.ExitingMethod, UnderlyingSystemTypeName, MethodBase.GetCurrentMethod().Name));
        }

        public virtual async void UploadExecutionContext(PluginExecutionContext executionContext)
        {
            Trace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.EnteredMethod, UnderlyingSystemTypeName, MethodBase.GetCurrentMethod().Name));

            var payload = new StringContent(SerializationHelper.SerializeJson(executionContext), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await HttpClient.PostAsync("api/webhook/plugins/annotation/handle-file-upload", payload);
            var statusCode = response.StatusCode;
            string responseJson = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                throw
                    new InvalidPluginExecutionException("An error occured whilst trying to upload the file.",
                    new HttpException(string.Format(CultureInfo.InvariantCulture, "Status: {0} | {1}", statusCode, responseJson)));
            }
            else 
            {
                Trace(string.Format(CultureInfo.InvariantCulture, "HTTP Status: {0} | {1} | {2}", UnderlyingSystemTypeName, MethodBase.GetCurrentMethod().Name, statusCode));
                Trace(string.Format(CultureInfo.InvariantCulture, "HTTP Response: {0} | {1} | {2}", UnderlyingSystemTypeName, MethodBase.GetCurrentMethod().Name, responseJson));
            }

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
