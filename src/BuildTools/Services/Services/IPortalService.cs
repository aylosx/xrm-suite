namespace Aylos.Xrm.Sdk.BuildTools.Services
{
    using System;

    public interface IPortalService : IDisposable
    {
        void ExportWebFiles(string outputPath, string websitePrimaryKey);

        void UpdateWebFiles(string inputPath, string websitePrimaryKey, string blockedAttachments);

        void UpdateWebTemplates(string inputPath, string websitePrimaryKey);
    }
}
