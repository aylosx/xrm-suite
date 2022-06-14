namespace Aylos.Xrm.Sdk.Core.ConsoleApps.TestConnectionCore
{
    using Connection = Common.Connection;
    using NLog;
    using System;
    using System.Globalization;

    public sealed class TestConnectionCoreApp : ConsoleApp
    {
        static void Main(string[] args)
        {
            TestConnectionCoreApp app = new();
            Logger logger = app.Logger;
            Sdk.ConsoleApps.ExitCode exitCode = Sdk.ConsoleApps.ExitCode.None;
            try
            {
                logger.Info("Application started.");

                CultureInfo.CurrentCulture = CultureInfo.CreateSpecificCulture("en-GB");

                Options options = new();

                Console.ReadLine();

                CommandLineOptions.ParseArguments<Options>(args);

                Connection connection = new(options.ConnectionString, options.ConnectionRetries, options.ConnectionTimeout);

                exitCode = Sdk.ConsoleApps.ExitCode.Success;
            }
            catch (Exception ex)
            {
                exitCode = new Sdk.ConsoleApps.ExceptionHandlingService(ex).GetExitCode();
            }
            finally
            {
                logger.Info(CultureInfo.InvariantCulture, "Application exited with code: {0}", (int)exitCode);
                Environment.Exit((int)exitCode);
            }
        }
    }
}
