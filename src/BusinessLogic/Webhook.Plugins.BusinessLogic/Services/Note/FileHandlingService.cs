namespace Webhook.Plugins.BusinessLogic.Services.Note
{
    using Aylos.Xrm.Sdk.Common;
    using Aylos.Xrm.Sdk.Core.WebhookPlugins;

    using Microsoft.Extensions.Logging;
    using Microsoft.PowerPlatform.Dataverse.Client;
    using Microsoft.Xrm.Sdk;
    using Microsoft.Xrm.Sdk.Query;

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

        public const string RegardingEntityNotExist = "Oups, we were not able to find a record for the given key.";
        public const string RetrieveEntityFailed = "Oups, an error has occured whilst trying to retrieve the parent record.";

        #endregion

        #region Properties

        public ICrmService CrmService { get; set; }

        public static HttpClient HttpClient { get; set; }

        #endregion

        #region Constructor

        public FileHandlingService(HttpClient httpClient, ICrmService crmService, ServiceClient serviceClient, RemoteExecutionContext remoteExecutionContext, HttpRequestMessage requestMessage, ILoggerFactory loggerFactory) 
            : base(serviceClient, remoteExecutionContext, requestMessage, loggerFactory)
        {
            HttpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));

            CrmService = crmService ?? throw new ArgumentNullException(nameof(crmService));
        }

        #endregion

        #region Methods

        /// <summary>
        /// Called when a new annotation is created, and handles all the activities related to a new file upload. 
        /// </summary>
        /// <remarks>Throws exception if an error occurs.</remarks>
        public void HandleFileUpload()
        {
            Logger.LogTrace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.EnteredMethod, UnderlyingSystemTypeName, MethodBase.GetCurrentMethod().Name));

            // Upload the content to the AV scanning microservice
            SubmitFileToAV(TargetBusinessEntity);

            // Upload the content to the Azure BLOB storage
            UploadFileToStorage(TargetBusinessEntity);

            Logger.LogTrace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.ExitingMethod, UnderlyingSystemTypeName, MethodBase.GetCurrentMethod().Name));
        }

        /// <summary>
        /// Called when an annotation is deleted, and handles all the activities related to a file deletion. 
        /// </summary>
        /// <remarks>Throws exception if an error occurs.</remarks>
        public void HandleFileDeletion()
        {
            Logger.LogTrace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.EnteredMethod, UnderlyingSystemTypeName, MethodBase.GetCurrentMethod().Name));

            Logger.LogTrace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.ExitingMethod, UnderlyingSystemTypeName, MethodBase.GetCurrentMethod().Name));
        }

        /// <summary>
        /// Called when an annotation is retrieved, and handles all the activities related to a file download. 
        /// </summary>
        /// <remarks>Throws exception if an error occurs.</remarks>
        public void HandleFileDownload()
        {
            Logger.LogTrace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.EnteredMethod, UnderlyingSystemTypeName, MethodBase.GetCurrentMethod().Name));

            Logger.LogTrace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.ExitingMethod, UnderlyingSystemTypeName, MethodBase.GetCurrentMethod().Name));
        }

        #endregion

        #region Virtual Members

        /// <summary>
        /// Submits a file to the AV scanning microservice.
        /// </summary>
        /// <param name="annotation">The annotation entity.</param>
        /// <remarks>Throws exception if an error occurs.</remarks>
        public virtual void SubmitFileToAV(Note annotation)
        {
            Logger.LogTrace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.EnteredMethod, UnderlyingSystemTypeName, MethodBase.GetCurrentMethod().Name));

            /**
             * TO-DO: Upload the content to the AV scanning microservice
             * Throw error if the AV microservice reports an issue with the file
             * Consider implementing auto-repeat if a connection issue occurs
             **/

            throw new NotImplementedException("SubmitFileToAV");

#pragma warning disable CS0162 // Unreachable code detected
            Logger.LogTrace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.ExitingMethod, UnderlyingSystemTypeName, MethodBase.GetCurrentMethod().Name));
#pragma warning restore CS0162 // Unreachable code detected
        }

        /// <summary>
        /// Submits a file to the upload storage microservice.
        /// </summary>
        /// <param name="annotation">The annotation entity.</param>
        /// <remarks>Throws exception if an error occurs.</remarks>
        public virtual void UploadFileToStorage(Note annotation)
        {
            Logger.LogTrace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.EnteredMethod, UnderlyingSystemTypeName, MethodBase.GetCurrentMethod().Name));

            /**
             * TO-DO: Upload the file to the storage microservice
             * Throw error if the storage microservice reports an issue
             * Consider implementing auto-repeat if a connection issue occurs
             **/

            throw new NotImplementedException("UploadFileToStorage");

#pragma warning disable CS0162 // Unreachable code detected
            Logger.LogTrace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.ExitingMethod, UnderlyingSystemTypeName, MethodBase.GetCurrentMethod().Name));
#pragma warning restore CS0162 // Unreachable code detected
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
