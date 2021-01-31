namespace Aylos.Xrm.Sdk.ConsoleApps
{
    using Aylos.Xrm.Sdk.Common;
    using CommandLine;
    using CommandLine.Text;
    using System;
    using System.Globalization;
    using System.Reflection;

    public class CommandLineOptions : ConsoleApp
    {
        string SystemTypeName { get { return GetType().UnderlyingSystemType.Name; } }

        public CommandLineOptions() : base()
        {
            Logger.Trace(CultureInfo.InvariantCulture, TraceMessageHelper.EnteredMethod, SystemTypeName, MethodBase.GetCurrentMethod().Name);

            Logger.Trace(CultureInfo.InvariantCulture, TraceMessageHelper.ExitingMethod, SystemTypeName, MethodBase.GetCurrentMethod().Name);
        }

        [HelpOption]
        public string GetUsage()
        {
            Logger.Trace(CultureInfo.InvariantCulture, TraceMessageHelper.EnteredMethod, SystemTypeName, MethodBase.GetCurrentMethod().Name);

            string usageText = HelpText.AutoBuild(this, (HelpText current) => HelpText.DefaultParsingErrorsHandler(this, current));

            Logger.Trace(CultureInfo.InvariantCulture, TraceMessageHelper.ExitingMethod, SystemTypeName, MethodBase.GetCurrentMethod().Name);

            return usageText;
        }

        public static void ArgumentParsingFailed()
        {
            throw new ArgumentException("Please check the input arguments syntax and retry.");
        }
    }
}
