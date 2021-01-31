namespace Aylos.Xrm.Sdk.BuildTools.ExportSolution
{
    using Aylos.Xrm.Sdk.ConsoleApps;
    using CommandLine;

    public class Options : CommandLineOptions
    {
        [Option("connectionString", Required = false, HelpText = "Specifies the Common Data Service instance connection string.")]
        public string ConnectionString { get; set; }

        [Option("connectionRetries", DefaultValue = 3, Required = false, HelpText = "Specifies the number of attempts to connect before the connection fails.")]
        public int ConnectionRetries { get; set; }

        [Option("connectionTimeout", DefaultValue = 120, Required = false, HelpText = "Specifies the number of seconds before the individual action times out.")]
        public int ConnectionTimeout { get; set; }

        [Option("authorityUrl", Required = false, DefaultValue = "https://login.microsoftonline.com/common", HelpText = "Specifies the Azure Active Directory Authority URL.")]
        public string AuthorityUrl { get; set; }

        [Option("tenantId", Required = false, HelpText = "Specifies the Azure Active Directory Tenant ID. e.g. 00000000-0000-0000-0000-000000000000")]
        public string TenantId { get; set; }

        [Option("organizationUrl", Required = false, HelpText = "Specifies the Common Data Service URL. e.g. https://organizationname.crm.dynamics.com")]
        public string OrganizationUrl { get; set; }

        [Option("organizationUrlSuffix", Required = false, DefaultValue = "/xrmservices/2011/organization.svc/web?SdkClientVersion=9.0", HelpText = "Specifies the Common Data Service URL suffix.")]
        public string OrganizationUrlSuffix { get; set; }

        [Option("servicePrincipalId", Required = false, HelpText = "Specifies the Service Principal ID a.k.a. Application (client) ID. e.g. 00000000-0000-0000-0000-000000000000")]
        public string ServicePrincipalId { get; set; }

        [Option("servicePrincipalSecret", Required = false, HelpText = "Specifies the Service Principal Secret.")]
        public string ServicePrincipalSecret { get; set; }

        [Option("solutionName", Required = true, HelpText = "Specifies the solution unique name. You can use wildcard to export multiple solutions.")]
        public string SolutionName { get; set; }

        [Option("solutionType", DefaultValue = 0, Required = false, HelpText = "Specifies the solution type. You can use '0' for unmanaged, '1' for managed or '2' for both. Default value is 0.")]
        public int SolutionType { get; set; }

        [Option("outputPath", Required = true, HelpText = "Specifies the output file path.")]
        public string OutputPath { get; set; }

        [Option("includeVersion", DefaultValue = false, HelpText = "Determines if the version will be concatenated within the name. Default value is false.")]
        public bool? IncludeVersion { get; set; }
    }
}
