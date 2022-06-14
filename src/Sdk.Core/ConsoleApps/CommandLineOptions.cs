namespace Aylos.Xrm.Sdk.Core.ConsoleApps
{
    using CommandLine;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class CommandLineOptions : ConsoleApp
    {
        public static ParserResult<T> ParseArguments<T>(string[] args) where T : class
        {
            if (args is null) throw new ArgumentNullException(nameof(args));

            var errors = new List<Error>();

            ParserResult<T> parserResult = Parser.Default.ParseArguments<T>(args)
                .WithNotParsed(x => errors = x.ToList());

            if (errors.Any())
            {
                throw new ArgumentException("One or more issues occurred whilst parsing the arguments, ensure the arguments have been supplied properly and retry.");
            }

            return parserResult;
        }
    }
}
