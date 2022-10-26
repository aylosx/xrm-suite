namespace Aylos.Xrm.Sdk.Core.WebHooks
{
    public class DataverseServiceConfig
    {
        public const string DataverseApiConfig = "DataverseApi";
        public string OrganizationUrl { get; set; }
        public int TokenExpiryTime { get; set; }
    }
}
