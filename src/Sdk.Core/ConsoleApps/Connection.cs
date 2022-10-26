namespace Aylos.Xrm.Sdk.Core.ConsoleApps
{
    using Aylos.Xrm.Sdk.Common;
    using Aylos.Xrm.Sdk.Exceptions;
    using Microsoft.Crm.Sdk.Messages;
    using Microsoft.Xrm.Sdk.Client;
    using Microsoft.PowerPlatform.Dataverse.Client;
    using NLog;
    using System;
    using System.Globalization;
    using System.IO;
    using System.Net;
    using System.Reflection;
    using System.Threading;

    public sealed class Connection : ConsoleService<OrganizationServiceContext>
    {
        const string XrmSdkAssemblyName = "Microsoft.Xrm.Sdk.dll";

        const string XrmToolingAssemblyName = "Microsoft.Xrm.Tooling.Connector.dll";

        const string UnknownConnectionErrorMessage = "Unknown connection error.";

        readonly int ConnectionPollingInterval = 15000;

        private string SystemTypeName { get { return GetType().UnderlyingSystemType.Name; } }

        private int ConnectionRetries { get; set; }

        private int ConnectionTimeout { get; set; }

        private Connection(int connectionRetries = 3, int connectionTimeout = 120)
        {
            Logger.Trace(CultureInfo.InvariantCulture, TraceMessageHelper.EnteredMethod, SystemTypeName, MethodBase.GetCurrentMethod().Name);

            ConnectionRetries = connectionRetries;

            ConnectionTimeout = connectionTimeout;

            var fi = new FileInfo(Assembly.GetExecutingAssembly().Location);
            string path = fi.Directory.FullName;

            string xrmSdkVersion = FileHelper.GetFileVersion($"{path}\\{XrmSdkAssemblyName}");
            Logger.Info(CultureInfo.InvariantCulture, "The XRM SDK version is {0}", xrmSdkVersion);

            string xrmToolingVersion = FileHelper.GetFileVersion($"{path}\\{XrmToolingAssemblyName}");
            Logger.Info(CultureInfo.InvariantCulture, "The XRM Tooling version : {0}", xrmToolingVersion);

            ServiceClient.MaxConnectionTimeout = TimeSpan.FromSeconds(ConnectionTimeout);

            Logger.Info(CultureInfo.InvariantCulture, "The maximum connection timeout is set to {0} seconds", ServiceClient.MaxConnectionTimeout.Seconds);

            Logger.Trace(CultureInfo.InvariantCulture, TraceMessageHelper.ExitingMethod, SystemTypeName, MethodBase.GetCurrentMethod().Name);
        }

        public Connection(string connectionString, int connectionRetries, int connectionTimeout) : this(connectionRetries, connectionTimeout)
        {
            Logger.Trace(CultureInfo.InvariantCulture, TraceMessageHelper.EnteredMethod, SystemTypeName, MethodBase.GetCurrentMethod().Name);

            SetSecurityProtocol();

            SetOrganizationService(connectionString);

            WhoAmI();

            Logger.Trace(CultureInfo.InvariantCulture, TraceMessageHelper.ExitingMethod, SystemTypeName, MethodBase.GetCurrentMethod().Name);
        }

        private void SetOrganizationService(string connectionString)
        {
            Logger.Trace(CultureInfo.InvariantCulture, TraceMessageHelper.EnteredMethod, SystemTypeName, MethodBase.GetCurrentMethod().Name);

            if (string.IsNullOrWhiteSpace(connectionString)) throw new ArgumentNullException(nameof(connectionString));

            for (int i = 0; i < ConnectionRetries; i++)
            {
                Logger.Info(CultureInfo.InvariantCulture, "Connection attempt: {0}", i + 1);

                ServiceClient = new ServiceClient(connectionString);

                if (ServiceClient == null || !ServiceClient.IsReady)
                {
                    if (ServiceClient != null)
                    {
                        Logger.Warn(ServiceClient.LastError);
                        if (ServiceClient.LastException != null) Logger.Error(ServiceClient.LastException);
                    }

                    if (i < ConnectionRetries - 1) Thread.Sleep(ConnectionPollingInterval);
                }
                else
                {
                    OrganizationService = ServiceClient;

                    Logger.Trace(CultureInfo.InvariantCulture, TraceMessageHelper.ExitingMethod, SystemTypeName, MethodBase.GetCurrentMethod().Name);
                    return;
                }
            }

            Logger.Trace(CultureInfo.InvariantCulture, TraceMessageHelper.AbortingMethod, SystemTypeName, MethodBase.GetCurrentMethod().Name);

            if (ServiceClient == null)
            {
                throw new PlatformException(message: UnknownConnectionErrorMessage);
            }
            else if (ServiceClient.LastException == null)
            {
                throw new PlatformException(message: UnknownConnectionErrorMessage);
            }
            else
            {
                throw new PlatformException("Connection error.", ServiceClient.LastException);
            }
        }

        private void SetSecurityProtocol()
        {
            Logger.Trace(CultureInfo.InvariantCulture, TraceMessageHelper.EnteredMethod, SystemTypeName, MethodBase.GetCurrentMethod().Name);

            if (!ServicePointManager.SecurityProtocol.HasFlag(SecurityProtocolType.Tls11))
            {
                ServicePointManager.SecurityProtocol ^= SecurityProtocolType.Tls11;
            }

            if (!ServicePointManager.SecurityProtocol.HasFlag(SecurityProtocolType.Tls12))
            {
                ServicePointManager.SecurityProtocol ^= SecurityProtocolType.Tls12;
            }

            /*
             * TO-DO: Research and enable TLS 1.3 

            if (!ServicePointManager.SecurityProtocol.HasFlag(SecurityProtocolType.Tls13))
            {
                ServicePointManager.SecurityProtocol ^= SecurityProtocolType.Tls13;
            }

            */

            Logger.Trace(CultureInfo.InvariantCulture, TraceMessageHelper.ExitingMethod, SystemTypeName, MethodBase.GetCurrentMethod().Name);
        }

        private void WhoAmI()
        {
            Logger.Trace(CultureInfo.InvariantCulture, TraceMessageHelper.EnteredMethod, SystemTypeName, MethodBase.GetCurrentMethod().Name);

            Logger.Info(CultureInfo.InvariantCulture, "Connection established successfully.");

            Logger.Info(CultureInfo.InvariantCulture, "Retrieving current user, business unit and organization.");
            var whoAmIResponse = (WhoAmIResponse)OrganizationService.Execute(new WhoAmIRequest());
            Logger.Info(CultureInfo.InvariantCulture, "Business unit: {0}", whoAmIResponse.BusinessUnitId);
            Logger.Info(CultureInfo.InvariantCulture, "Organization: {0}", whoAmIResponse.OrganizationId);
            Logger.Info(CultureInfo.InvariantCulture, "User: {0}", whoAmIResponse.UserId);

            Logger.Info(CultureInfo.InvariantCulture, "Retrieving the instance version.");
            var versionResponse = (RetrieveVersionResponse)OrganizationService.Execute(new RetrieveVersionRequest());
            Logger.Info(CultureInfo.InvariantCulture, "Instance version {0}.", versionResponse.Version);

            Logger.Trace(CultureInfo.InvariantCulture, TraceMessageHelper.ExitingMethod, SystemTypeName, MethodBase.GetCurrentMethod().Name);
        }
    }
}
