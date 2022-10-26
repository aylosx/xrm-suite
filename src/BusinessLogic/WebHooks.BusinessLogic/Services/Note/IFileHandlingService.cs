namespace WebHooks.BusinessLogic.Services.Note
{
    using System;
    using System.Net.Http;

    using WebHooks.BusinessLogic.Services.Data;

    public interface IFileHandlingService : IDisposable
    {
        /// <summary>
        /// Gets or sets the CRM service.
        /// </summary>
        /// <value>The CRM service object instance.</value>
        ICrmService CrmService { get; set; }

        /// <summary>
        /// Called when a new annotation is create, and handle 
        /// all the activities related to a new file upload. 
        /// </summary>
        void HandleFileUpload(HttpRequestMessage req);

        /// <summary>
        /// Called when an annotation is deleted, and handles 
        /// all the activities related to a file deletion. 
        /// </summary>
        void HandleFileDeletion(HttpRequestMessage req);

        /// <summary>
        /// Called when an annotation is retrieved, and handles 
        /// all the activities related to a file download. 
        /// </summary>
        void HandleFileDownload(HttpRequestMessage req);
    }
}
