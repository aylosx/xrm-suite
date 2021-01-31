namespace Aylos.Xrm.Sdk.BuildTools.DecompressFile
{
    using Aylos.Xrm.Sdk.ConsoleApps;
    using CommandLine;

    public class Options : CommandLineOptions
    {
        [Option("inputFile", Required = true, HelpText = "Specifies the file path of the input file.")]
        public string InputFileName { get; set; }

        [Option("outputPath", Required = true, HelpText = "Specifies the path of the output directory.")]
        public string OutputPath { get; set; }
    }
}
