namespace Aylos.Xrm.Sdk.Common
{
    using System;
    using System.IO;

    public static class AzureHelper
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public static AzureFile CreateAzureFile(string document, int maximumLength)
        {
            AzureFile azureFile = null;
            byte[] bytes = Convert.FromBase64String(document);
            if (bytes.Length > 0 && bytes.Length < maximumLength)
            {
                using (MemoryStream ms = new MemoryStream(bytes))
                using (StreamReader sr = new StreamReader(ms))
                {
                    string fileContent = sr.ReadToEnd();
                    try
                    {
                        azureFile = SerializationHelper.DeserializeJson<AzureFile>(fileContent);
                    }
                    catch { }
                }
            }

            return azureFile;
        }
    }
}