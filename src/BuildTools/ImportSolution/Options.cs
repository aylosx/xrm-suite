namespace Aylos.Xrm.Sdk.BuildTools.ImportSolution
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

        [Option("inputFile", Required = true, HelpText = "Specifies the input file path.")]
        public string InputFile { get; set; }

        [Option("holdingSolution", DefaultValue = true, Required = false, HelpText = "Denotes whether the solution is of holding type.")]
        public bool? HoldingSolution { get; set; }

        [Option("overwriteUnmanagedCustomizations", DefaultValue = true, Required = false, HelpText = "Denotes whether the solution will ovewrite unmanaged customizations.")]
        public bool? OverwriteUnmanagedCustomizations { get; set; }

        [Option("publishWorkflows", DefaultValue = true, Required = false, HelpText = "Denotes whether the solution will publish workflows.")]
        public bool? PublishWorkflows { get; set; }

        [Option("skipProductUpdateDependencies", DefaultValue = false, Required = false, HelpText = "Denotes whether the solution will skip enforcement of product update dependencies.")]
        public bool? SkipProductUpdateDependencies { get; set; }

        [Option("pollingInterval", DefaultValue = 3, Required = false, HelpText = "Specifies the number of seconds between each polling to the asynchronous system job.")]
        public int PollingInterval { get; set; }

        [Option("pollingTimeout", DefaultValue = 600, Required = false, HelpText = "Specifies the number of seconds before the asynchronous system job times out.")]
        public int PollingTimeout { get; set; }
    }
}
