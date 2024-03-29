﻿namespace Aylos.Xrm.Sdk.BuildTools.UpdateSolutionVersion
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

        [Option("solutionName", Required = true, HelpText = "Specifies the Dynamics 365 solution unique name. You can use wildcard to export multiple solutions.")]
        public string SolutionName { get; set; }

        [Option("timeZone", DefaultValue = "GMT Standard Time", Required = false, HelpText = "Specifies the timezone identifier (e.g. GMT Standard Time).")]
        public string TimeZone { get; set; }

        [Option("version", Required = false, HelpText = "Specifies the solution version (e.g. 19.05.17.1441). If not provided will be calculated using the pattern yy.MM.dd.HHmm.")]
        public string Version { get; set; }
    }
}
