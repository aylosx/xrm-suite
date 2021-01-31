namespace Aylos.Xrm.Sdk.BuildTools.HideData
{
    using Aylos.Xrm.Sdk.ConsoleApps;
    using CommandLine;

    public class Options : CommandLineOptions
    {
        [Option("inputFile", Required = true, HelpText = "Specifies the file path of the input data file.")]
        public string InputFileName { get; set; }

        [Option("outputFile", Required = true, HelpText = "Specifies the file path of the output data file.")]
        public string OutputFileName { get; set; }

        [Option("primaryKey", Required = true, HelpText = "Specifies the primary key of the record you wish to hide a value.")]
        public string PrimaryKey { get; set; }

        [Option("attributeName", Required = true, HelpText = "Specifies the field attribute name you wish to hide.")]
        public string AttributeName { get; set; }

        [Option("attributeValue", Required = true, HelpText = "Specifies the field attribute value you wish to set.")]
        public string AttributeValue { get; set; }
    }
}
