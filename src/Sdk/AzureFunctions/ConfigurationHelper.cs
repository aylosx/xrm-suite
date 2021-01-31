namespace Aylos.Xrm.Sdk.AzureFunctions
{
    using System;
    using System.Configuration;

    public static class ConfigurationHelper
    {
        public static string Get(string name)
        {
            string value = ConfigurationManager.AppSettings.Get(name);
            if (string.IsNullOrWhiteSpace(value)) {
                throw new ConfigurationErrorsException($"Specifies the '{name}' not found in the application settings configuration.");
            }
            return value;
        }
    }
}
