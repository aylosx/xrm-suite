namespace Aylos.Xrm.Sdk.BuildTools.HideData
{
    using Aylos.Xrm.Sdk.BuildTools.Services;
    using Aylos.Xrm.Sdk.ConsoleApps;
    using CommandLine;
    using NLog;
    using System;
    using System.Globalization;

    public sealed class HideDataApp : ConsoleApp
    {
        static void Main(string[] args)
        {
            HideDataApp app = new HideDataApp();
            Logger logger = app.Logger;
            ExitCode exitCode = ExitCode.None;
            try
            {
                logger.Info("Application started.");

                CultureInfo.CurrentCulture = CultureInfo.CreateSpecificCulture("en-GB");

                Options options = new Options();

                Parser.Default.ParseArgumentsStrict(args, options, CommandLineOptions.ArgumentParsingFailed);

                using (IDataMigrationService dataMigrationService = new DataMigrationService())
                {
                    dataMigrationService.HideData(options.InputFileName, options.OutputFileName, options.PrimaryKey, options.AttributeName, options.AttributeValue);
                    exitCode = ExitCode.Success;
                }
            }
            catch (Exception ex)
            {
                exitCode = new ExceptionHandlingService(ex).GetExitCode();
            }
            finally
            {
                logger.Info(CultureInfo.InvariantCulture, "Application exited with code: {0}", (int)exitCode);
                Environment.Exit((int)exitCode);
            }
        }
    }
}
