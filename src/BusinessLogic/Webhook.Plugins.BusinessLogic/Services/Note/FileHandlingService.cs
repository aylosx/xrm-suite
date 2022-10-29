namespace Webhook.Plugins.BusinessLogic.Services.Note
{
    using Aylos.Xrm.Sdk.Common;
    using Aylos.Xrm.Sdk.Core.WebhookPlugins;

    using Microsoft.Extensions.Logging;
    using Microsoft.PowerPlatform.Dataverse.Client;
    using Microsoft.Xrm.Sdk;

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

        public FileHandlingService(HttpClient httpClient, ICrmService crmService, ServiceClient serviceClient, RemoteExecutionContext remoteExecutionContext, ILoggerFactory loggerFactory) 
            : base(serviceClient, remoteExecutionContext, loggerFactory)
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

            /*
            var serviceClient = CrmService.ServiceClient;
            serviceClient.CallerId = new Guid("65484825-2be0-ec11-bb3d-0022481a94f4"); // RemoteExecutionContext.InitiatingUserId;
            */

            Note annotation = CrmService.GetNoteByKey(RemoteExecutionContext.PrimaryEntityId, RemoteExecutionContext.InitiatingUserId);
            if (annotation == null) throw new InvalidPluginExecutionException(NoteNotExistMessage);

            if (HttpMessageSizeExceeded)
            {
                /*
                Note annotation = CrmService.GetNoteByKey(RemoteExecutionContext.PrimaryEntityId, RemoteExecutionContext.InitiatingUserId);
                if (annotation == null) throw new InvalidPluginExecutionException(NoteNotExistMessage);
                */

                if (annotation.IsDocument.GetValueOrDefault(true) && !string.IsNullOrWhiteSpace(annotation.Document))
                {
                    string mimeType = annotation.MimeType;
                    string base64Body = annotation.Document;

                    // TO-DO: Upload the content to the AV scanning microservice
                    // virtual
                    // TO-DO: Upload the content to the Azure BLOB storage
                    // virtual
                    // TO-DO: Remove the content from the Dataverse storage in the pre-create plugin
                }
            }
            else
            {
                if (TargetBusinessEntity.IsDocument.GetValueOrDefault(true) && !string.IsNullOrWhiteSpace(TargetBusinessEntity.Document))
                {
                    string mimeType = TargetBusinessEntity.MimeType;
                    string base64Body = TargetBusinessEntity.Document;

                    // TO-DO: Upload the content to the AV scanning microservice
                    // virtual
                    // TO-DO: Upload the content to the Azure BLOB storage
                    // virtual
                    // TO-DO: Remove the content from the Dataverse storage in the pre-create plugin
                }
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
