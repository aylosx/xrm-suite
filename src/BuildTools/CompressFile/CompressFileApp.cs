namespace Aylos.Xrm.Sdk.BuildTools.CompressFile
{
    using Aylos.Xrm.Sdk.BuildTools.Services;
    using Aylos.Xrm.Sdk.ConsoleApps;
    using CommandLine;
    using NLog;
    using System;
    using System.Globalization;

    public sealed class CompressFileApp : ConsoleApp
    {
        static void Main(string[] args)
        {
            CompressFileApp app = new CompressFileApp();
            Logger logger = app.Logger;
            ExitCode exitCode = ExitCode.None;
            try
            {
                logger.Info("Application started.");

                CultureInfo.CurrentCulture = CultureInfo.CreateSpecificCulture("en-GB");

                Options options = new Options();

                Parser.Default.ParseArgumentsStrict(args, options, CommandLineOptions.ArgumentParsingFailed);

                using (ICompressionService compressionService = new CompressionService())
                {
                    compressionService.Compress(options.InputPath, options.OutputFile);
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
