namespace Aylos.Xrm.Sdk.BuildTools.UpdateOrganizationSetting
{
    using Aylos.Xrm.Sdk.BuildTools.Models.Domain;
    using Aylos.Xrm.Sdk.BuildTools.Services;
    using Aylos.Xrm.Sdk.Common;
    using Aylos.Xrm.Sdk.ConsoleApps;
    using CommandLine;
    using NLog;
    using System;
    using System.Globalization;

    public sealed class UpdateOrganizationSettingApp : ConsoleApp
    {
        static void Main(string[] args)
        {
            UpdateOrganizationSettingApp app = new UpdateOrganizationSettingApp();
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

                //TO-DO: this is in experimental mode 
                //Console.ReadLine();

                using (CrmServiceContext organizationServiceContext = new CrmServiceContext(connection.OrganizationService))
                using (CrmService crmService = new CrmService(organizationServiceContext, connection))
                using (DeploymentService deploymentService = new DeploymentService(organizationServiceContext, crmService))
                {
                    var organizationSettings = new Organization
                    {
                        BlockAttachments = options.BlockAttachments,
                        EnableAccessToLegacyWebClientUI = options.EnableAccessToLegacyWebClientUI,
                        LegacyFormRendering = options.LegacyFormRendering, 
                        OrganizationName = options.OrganizationName,
                        SessionTimeoutEnabled = options.SessionTimeoutEnabled,
                        SessionTimeoutInMinutes = options.SessionTimeoutInMinutes,
                        SessionTimeoutReminderInMinutes = options.SessionTimeoutReminderInMinutes,
                        SLAPauseStates = options.SLAPauseStates
                    };
                    deploymentService.UpdateOrganizationSettings(organizationSettings);
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
