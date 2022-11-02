namespace Webhook.Plugins.BusinessLogic.Services.Note
{
    using Shared.Models.Domain;

    using System;
    using System.Net.Http;

    using Webhook.Plugins.BusinessLogic.Services.Data;

    public interface IFileHandlingService : IDisposable
    {
        ICrmService CrmService { get; set; }

        void HandleFileUpload(HttpRequestMessage req);

        void HandleFileDeletion(HttpRequestMessage req);

        void HandleFileDownload(HttpRequestMessage req);

        void SubmitFileToAV(Note annotation);

        void UploadFileToStorage(Note annotation);
    }
}
