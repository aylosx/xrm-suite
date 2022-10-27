namespace Webhook.Plugins.BusinessLogic.Services.Note
{
    using Aylos.Xrm.Sdk.Common;
    using Aylos.Xrm.Sdk.Core.WebhookPlugins;

    using Microsoft.Extensions.Logging;
    using Microsoft.Xrm.Sdk;
    using Microsoft.Xrm.Sdk.Client;

    using Shared.Models.Domain;

    using System;
    using System.Globalization;
    using System.Net.Http;
    using System.Reflection;

    using Webhook.Plugins.BusinessLogic.Services.Data;

    public class FileHandlingService : DataverseService<Note>, IFileHandlingService
    {
        #region Static Members 

        public const string TextFormat = "Code-{0}";
        public const string DateTimeFormat = "yyyyMMddHHmmss";
        public static readonly DateTime DateTimeNow = DateTime.UtcNow;
        
        public const string NoteNotExistMessage = "Oups, we were not able to find a note for the given key.";

        #endregion

        #region Properties

        public ICrmService CrmService { get; set; }

        public static HttpClient HttpClient { get; set; }

        #endregion

        #region Constructor

        public FileHandlingService(HttpClient httpClient, ICrmService crmService, IOrganizationService organizationService, RemoteExecutionContext remoteExecutionContext, ILoggerFactory loggerFactory) 
            : base(organizationService, remoteExecutionContext, loggerFactory)
        {
            HttpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));

            CrmService = crmService ?? throw new ArgumentNullException(nameof(crmService));
        }

        #endregion

        #region Methods

        /// <summary>
        /// Called when a new annotation is create, and handle 
        /// all the activities related to a new file upload. 
        /// </summary>
        public void HandleFileUpload(HttpRequestMessage req)
        {
            Logger.LogTrace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.EnteredMethod, UnderlyingSystemTypeName, MethodBase.GetCurrentMethod().Name));

            HttpRequestMessage = req ?? throw new ArgumentNullException(nameof(req));

            switch (RemoteExecutionContext.MessageName)
            {
                case PlatformMessageHelper.Create:

                    break;

                default: throw new InvalidOperationException();
            }

            Note updatedNote = null;
            if (HttpMessageSizeExceeded)
            {
                Note annotation = CrmService.GetNoteByKey(RemoteExecutionContext.PrimaryEntityId);
                if (annotation == null) throw new InvalidPluginExecutionException(NoteNotExistMessage);

                if (!string.IsNullOrWhiteSpace(annotation.Document))
                {
                    // TO-DO: Upload the content to the AV scanning microservice
                    // virtual
                    // TO-DO: Upload the content to the Azure BLOB storage
                    // virtual
                    // TO-DO: Remove the content from the Dataverse storage
                    updatedNote = new Note
                    {
                        AnnotationId = annotation.AnnotationId,
                        Document = annotation.Document,
                    };
                }
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(TargetBusinessEntity.Document))
                {
                    // TO-DO: Upload the content to the AV scanning microservice
                    // virtual
                    // TO-DO: Upload the content to the Azure BLOB storage
                    // virtual
                    // TO-DO: Remove the content from the Dataverse storage
                    updatedNote = new Note
                    {
                        AnnotationId = TargetBusinessEntity.Id,
                        Document = TargetBusinessEntity.Document,
                    };
                }
            }

            if (updatedNote != null)
            {
                CrmService.OrganizationServiceContext.ClearChanges();
                CrmService.OrganizationServiceContext.Attach(updatedNote);
                CrmService.OrganizationServiceContext.UpdateObject(updatedNote);
                CrmService.SaveChanges(SaveChangesOptions.None);
            }

            Logger.LogTrace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.ExitingMethod, UnderlyingSystemTypeName, MethodBase.GetCurrentMethod().Name));
        }

        /// <summary>
        /// Called when an annotation is deleted, and handles 
        /// all the activities related to a file deletion. 
        /// </summary>
        public void HandleFileDeletion(HttpRequestMessage req)
        {
            Logger.LogTrace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.EnteredMethod, UnderlyingSystemTypeName, MethodBase.GetCurrentMethod().Name));

            HttpRequestMessage = req ?? throw new ArgumentNullException(nameof(req));

            switch (RemoteExecutionContext.MessageName)
            {
                case PlatformMessageHelper.Delete:
                    break;

                default: throw new InvalidOperationException();
            }

            Logger.LogTrace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.ExitingMethod, UnderlyingSystemTypeName, MethodBase.GetCurrentMethod().Name));
        }

        /// <summary>
        /// Called when an annotation is retrieved, and handles 
        /// all the activities related to a file download. 
        /// </summary>
        public void HandleFileDownload(HttpRequestMessage req)
        {
            Logger.LogTrace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.EnteredMethod, UnderlyingSystemTypeName, MethodBase.GetCurrentMethod().Name));

            HttpRequestMessage = req ?? throw new ArgumentNullException(nameof(req));

            switch (RemoteExecutionContext.MessageName)
            {
                case PlatformMessageHelper.Retrieve:
                    break;

                default: throw new InvalidOperationException();
            }

            Logger.LogTrace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.ExitingMethod, UnderlyingSystemTypeName, MethodBase.GetCurrentMethod().Name));
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

        ~FileHandlingService()
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
