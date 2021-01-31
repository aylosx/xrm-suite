namespace Aylos.Xrm.Sdk.BuildTools.PublishCustomizations
{
    using Aylos.Xrm.Sdk.BuildTools.Models.Domain;
    using Aylos.Xrm.Sdk.BuildTools.Services;
    using Aylos.Xrm.Sdk.Common;
    using Aylos.Xrm.Sdk.ConsoleApps;
    using CommandLine;
    using NLog;
    using System;
    using System.Globalization;

    public sealed class PublishCustomizationsApp : ConsoleApp
    {
        static void Main(string[] args)
        {
            PublishCustomizationsApp app = new PublishCustomizationsApp();
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
                using (SolutionService solutionService = new SolutionService(organizationServiceContext, connection, crmService))
                {
                    solutionService.PublishCustomizations();
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
