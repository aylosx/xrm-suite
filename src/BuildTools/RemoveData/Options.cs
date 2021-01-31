namespace Aylos.Xrm.Sdk.BuildTools.RemoveData
{
    using Aylos.Xrm.Sdk.ConsoleApps;
    using CommandLine;

    public class Options : CommandLineOptions
    {
        [Option("inputFile", Required = true, HelpText = "Specifies the file path of the input data file.")]
        public string InputFileName { get; set; }

        [Option("outputFile", Required = true, HelpText = "Specifies the file path of the output data file.")]
        public string OutputFileName { get; set; }

        [Option("attributeName", Required = true, HelpText = "Specifies the field attribute name of the record you wish to remove.")]
        public string AttributeName { get; set; }

        [Option("attributeValue", Required = true, HelpText = "Specifies the field attribute value of the record you wish to remove.")]
        public string AttributeValue { get; set; }
    }
}
