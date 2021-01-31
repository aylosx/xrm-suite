namespace Aylos.Xrm.Sdk.BuildTools.UpdateWebFiles
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

        [Option("inputPath", Required = true, HelpText = "Specifies the input full file path.")]
        public string InputPath { get; set; }

        [Option("websitePrimaryKey", Required = true, HelpText = "Specifies the primary key of the target web site.")]
        public string WebsitePrimaryKey { get; set; }

        [Option("blockAttachments",
            Required = false,
            DefaultValue = "ade;adp;app;asa;ashx;asmx;asp;bas;bat;cdx;cer;chm;class;cmd;com;config;cpl;crt;csh;dll;exe;fxp;hlp;hta;htr;htw;ida;idc;idq;inf;ins;isp;its;jar;js;jse;ksh;lnk;mad;maf;mag;mam;maq;mar;mas;mat;mau;mav;maw;mda;mdb;mde;mdt;mdw;mdz;msc;msh;msh1;msh1xml;msh2;msh2xml;mshxml;msi;msp;mst;ops;pcd;pif;prf;prg;printer;pst;reg;rem;scf;scr;sct;shb;shs;shtm;shtml;soap;stm;tmp;url;vb;vbe;vbs;vsmacros;vss;vst;vsw;ws;wsc;wsf;wsh",
            HelpText = "A semicolon separated list of the blocked files.")]
        public string BlockAttachments { get; set; }
    }
}
