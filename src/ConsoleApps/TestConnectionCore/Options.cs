namespace Aylos.Xrm.Sdk.Core.ConsoleApps.TestConnectionCore
{
    using Aylos.Xrm.Sdk.Core.ConsoleApps;
    using CommandLine;

    public class Options : CommandLineOptions
    {
        [Option("connectionString", Required = true, HelpText = "Specifies the Dataverse connection string.")]
        public string ConnectionString { get; set; }

        [Option("connectionRetries", Default = 3, Required = false, HelpText = "Specifies the number of attempts to connect before the connection fails.")]
        public int ConnectionRetries { get; set; }

        [Option("connectionTimeout", Default = 120, Required = false, HelpText = "Specifies the number of seconds before the individual action times out.")]
        public int ConnectionTimeout { get; set; }
    }
}
