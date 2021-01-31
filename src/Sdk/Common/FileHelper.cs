namespace Aylos.Xrm.Sdk.Common
{
    using System;
    using System.Diagnostics;
    using System.IO;

    public static class FileHelper
    {
        public static string GetFileVersion(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName)) throw new ArgumentNullException(nameof(fileName));

            if (File.Exists(fileName))
            {
                FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(fileName);
                return fvi.FileVersion;
            }
            return null;
        }
    }
}
