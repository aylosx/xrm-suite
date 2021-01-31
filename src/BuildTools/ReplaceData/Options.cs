namespace Aylos.Xrm.Sdk.BuildTools.ReplaceData
{
    using Aylos.Xrm.Sdk.ConsoleApps;
    using CommandLine;

    public class Options : CommandLineOptions
    {
        [Option("inputFile", Required = true, HelpText = "Specifies the file path of the input data file.")]
        public string InputFileName { get; set; }

        [Option("outputFile", Required = true, HelpText = "Specifies the file path of the output data file.")]
        public string OutputFileName { get; set; }

        [Option("findText", Required = true, HelpText = "Specifies the text you want to replace.")]
        public string FindText { get; set; }

        [Option("replaceWithText", Required = true, HelpText = "Specifies the text you want to be used by the replacement.")]
        public string ReplaceWithText { get; set; }
    }
}
