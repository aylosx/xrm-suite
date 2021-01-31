namespace Aylos.Xrm.Sdk.BuildTools.CompressFile
{
    using Aylos.Xrm.Sdk.ConsoleApps;
    using CommandLine;

    public class Options : CommandLineOptions
    {
        [Option("inputPath", Required = true, HelpText = "Specifies the path of the input directory.")]
        public string InputPath { get; set; }

        [Option("outputFile", Required = true, HelpText = "Specifies the file path of the output file.")]
        public string OutputFile { get; set; }
	}
}
