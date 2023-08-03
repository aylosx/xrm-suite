namespace Webhook.Plugins.BusinessLogic.Services.Note
{
    using Shared.Models.Domain;

    using System;
    using System.Net.Http;

    using Webhook.Plugins.BusinessLogic.Services.Data;

    public interface IFileHandlingService : IDisposable
    {
        ICrmService CrmService { get; set; }

        void HandleFileUpload();

        void HandleFileDeletion();

        void HandleFileDownload();

        void SubmitFileToAV(Note annotation);

        void UploadFileToStorage(Note annotation);
    }
}
