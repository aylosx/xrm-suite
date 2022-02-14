namespace Aylos.Xrm.Sdk.ConsoleApps.ProcessExecutionContext
{
    using Aylos.Xrm.Sdk.Common;
    using Aylos.Xrm.Sdk.ConsoleApps;
    using Aylos.Xrm.Sdk.ConsoleApps.Services;

    using Shared.Models.Domain;

    using CommandLine;

    using NLog;

    using System;
    using System.Globalization;
    using System.Threading;

    public sealed class ProcessExecutionContextApp : ConsoleApp
    {
        static void Main(string[] args)
        {
            ProcessExecutionContextApp app = new ProcessExecutionContextApp();
            Logger logger = app.Logger;
            ExitCode exitCode = ExitCode.None;
            try
            {
                logger.Info("Application started.");

                CultureInfo.CurrentCulture = CultureInfo.CreateSpecificCulture("en-GB");

                Options options = new Options();

                Parser.Default.ParseArgumentsStrict(args, options, CommandLineOptions.ArgumentParsingFailed);

                Connection connection = null;

                if (string.IsNullOrWhiteSpace(options.ConnectionString))
                {
                    connection = new Connection(options.AuthorityUrl, options.OrganizationUrl, options.OrganizationUrlSuffix, 
                        options.TenantId, options.ServicePrincipalId, options.ServicePrincipalSecret, options.ConnectionRetries, options.ConnectionTimeout);
                }
                else
                {
                    connection = new Connection(options.ConnectionString, options.ConnectionRetries, options.ConnectionTimeout);
                }

                using (CrmServiceContext organizationServiceContext = new CrmServiceContext(connection.OrganizationService))
                using (CrmService crmService = new CrmService(organizationServiceContext, connection))
                using (IExecutionContextService executionContextService = new ExecutionContextService(organizationServiceContext, connection, crmService))
                {
                    Console.WriteLine("Press any key to start ...");
                    Console.ReadKey();
                    do
                    {
                        while (!Console.KeyAvailable)
                        {
                            Console.WriteLine("Press ESC to stop");
                            executionContextService.ProcessExecutionContexts();
                            // Wait 5 secs until the next call
                            // TO-DO: the solution must be more robust to ensure that we are not going to face any pitfalls
                            Thread.Sleep(5000); 
                        }
                    } while (Console.ReadKey(true).Key != ConsoleKey.Escape);

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
