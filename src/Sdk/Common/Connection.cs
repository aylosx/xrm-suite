namespace Aylos.Xrm.Sdk.Common
{
    using Aylos.Xrm.Sdk.ConsoleApps;
    using Aylos.Xrm.Sdk.Exceptions;
    using Microsoft.Crm.Sdk.Messages;
    using Microsoft.IdentityModel.Clients.ActiveDirectory;
    using Microsoft.Xrm.Sdk;
    using Microsoft.Xrm.Sdk.Client;
    using Microsoft.Xrm.Sdk.WebServiceClient;
    using Microsoft.Xrm.Tooling.Connector;
    using NLog;
    using System;
    using System.Globalization;
    using System.IO;
    using System.Net;
    using System.Reflection;
    using System.Threading;
    using System.Threading.Tasks;

    public sealed class Connection : ConsoleService<OrganizationServiceContext>
    {
        const string XrmSdkAssemblyName = "Microsoft.Xrm.Sdk.dll";

        const string XrmToolingAssemblyName = "Microsoft.Xrm.Tooling.Connector.dll";

        const string UnknownConnectionErrorMessage = "Unknown connection error.";

        readonly int ConnectionPollingInterval = 15000;

        private string SystemTypeName { get { return GetType().UnderlyingSystemType.Name; } }

        private string AuthorityUrl { get; set; }

        private int ConnectionRetries { get; set; }

        private string ConnectionString { get; set; }

        private int ConnectionTimeout { get; set; }

        private string OrganizationUrl { get; set; }

        private string OrganizationUrlSuffix { get; set; }

        private string TenantId { get; set; }

        private string ServicePrincipalId { get; set; }

        private string ServicePrincipalSecret { get; set; }

        private Connection(int connectionRetries = 3, int connectionTimeout = 120)
        {
            Logger.Trace(CultureInfo.InvariantCulture, TraceMessageHelper.EnteredMethod, SystemTypeName, MethodBase.GetCurrentMethod().Name);

            ConnectionRetries = connectionRetries;

            ConnectionTimeout = connectionTimeout;

            FileInfo fi = new FileInfo(Assembly.GetExecutingAssembly().Location);
            string path = fi.Directory.FullName;

            string xrmSdkVersion = FileHelper.GetFileVersion($"{path}\\{XrmSdkAssemblyName}");
            Logger.Info(CultureInfo.InvariantCulture, "The XRM SDK version is {0}", xrmSdkVersion);

            string xrmToolingVersion = FileHelper.GetFileVersion($"{path}\\{XrmToolingAssemblyName}");
            Logger.Info(CultureInfo.InvariantCulture, "The XRM Tooling version : {0}", xrmToolingVersion);

            CrmServiceClient.MaxConnectionTimeout = TimeSpan.FromSeconds(ConnectionTimeout);
            Logger.Info(CultureInfo.InvariantCulture, "The maximum connection timeout is set to {0} seconds", CrmServiceClient.MaxConnectionTimeout.Seconds);

            Logger.Trace(CultureInfo.InvariantCulture, TraceMessageHelper.ExitingMethod, SystemTypeName, MethodBase.GetCurrentMethod().Name);
        }

        public Connection(string connectionString, int connectionRetries, int connectionTimeout) : this(connectionRetries, connectionTimeout)
        {
            Logger.Trace(CultureInfo.InvariantCulture, TraceMessageHelper.EnteredMethod, SystemTypeName, MethodBase.GetCurrentMethod().Name);

            ConnectionString = connectionString;

            SetSecurityProtocol();

            SetOrganizationService(connectionString);

            WhoAmI();

            Logger.Trace(CultureInfo.InvariantCulture, TraceMessageHelper.ExitingMethod, SystemTypeName, MethodBase.GetCurrentMethod().Name);
        }

        public Connection(string authorityUrl, string organizationUrl, string organizationUrlSuffix, string tenantId, string servicePrincipalId, string servicePrincipalSecret, int connectionRetries, int connectionTimeout) : this(connectionRetries, connectionTimeout)
        {
            Logger.Trace(CultureInfo.InvariantCulture, TraceMessageHelper.EnteredMethod, SystemTypeName, MethodBase.GetCurrentMethod().Name);

            AuthorityUrl = authorityUrl;

            OrganizationUrl = organizationUrl;

            OrganizationUrlSuffix = organizationUrlSuffix;

            ServicePrincipalId = servicePrincipalId;

            ServicePrincipalSecret = servicePrincipalSecret;

            TenantId = tenantId;

            SetSecurityProtocol();

            SetOrganizationService(authorityUrl, organizationUrl, organizationUrlSuffix, tenantId, servicePrincipalId, servicePrincipalSecret);

            WhoAmI();

            Logger.Trace(CultureInfo.InvariantCulture, TraceMessageHelper.ExitingMethod, SystemTypeName, MethodBase.GetCurrentMethod().Name);
        }

        private AuthenticationResult GetAuthenticationResult(string authorityUrl, string organizationUrl, string tenantId, string servicePrincipalId, string servicePrincipalSecret)
        {
            Logger.Trace(CultureInfo.InvariantCulture, TraceMessageHelper.EnteredMethod, SystemTypeName, MethodBase.GetCurrentMethod().Name);

            var loginAuthorityUrl = new UriBuilder(authorityUrl) { Path = tenantId };

            var authenticationContext = new AuthenticationContext(loginAuthorityUrl.Uri.AbsoluteUri, TokenCache.DefaultShared);

            var clientCredential = new ClientCredential(servicePrincipalId, servicePrincipalSecret);

            Task<AuthenticationResult> task = Task.Run(async () => await authenticationContext.AcquireTokenAsync(organizationUrl, clientCredential));

            AuthenticationResult authenticationResult = task.Result;

            Logger.Trace(CultureInfo.InvariantCulture, TraceMessageHelper.ExitingMethod, SystemTypeName, MethodBase.GetCurrentMethod().Name);

            return authenticationResult;
        }

        private void SetOrganizationService(string connectionString)
        {
            Logger.Trace(CultureInfo.InvariantCulture, TraceMessageHelper.EnteredMethod, SystemTypeName, MethodBase.GetCurrentMethod().Name);

            if (string.IsNullOrWhiteSpace(connectionString)) throw new ArgumentNullException(nameof(connectionString));

            for (int i = 0; i < ConnectionRetries; i++)
            {
                Logger.Info(CultureInfo.InvariantCulture, "Connection attempt: {0}", i + 1);

                CrmServiceClient = new CrmServiceClient(connectionString);

                if (CrmServiceClient == null || !CrmServiceClient.IsReady)
                {
                    if (CrmServiceClient != null)
                    {
                        Logger.Warn(CrmServiceClient.LastCrmError);
                        if (CrmServiceClient.LastCrmException != null) Logger.Error(CrmServiceClient.LastCrmException);
                    }

                    if (i < ConnectionRetries - 1) Thread.Sleep(ConnectionPollingInterval);
                }
                else
                {
                    OrganizationService = CrmServiceClient.OrganizationWebProxyClient ?? (IOrganizationService)CrmServiceClient.OrganizationServiceProxy;

                    Logger.Trace(CultureInfo.InvariantCulture, TraceMessageHelper.ExitingMethod, SystemTypeName, MethodBase.GetCurrentMethod().Name);
                    return;
                }
            }

            Logger.Trace(CultureInfo.InvariantCulture, TraceMessageHelper.AbortingMethod, SystemTypeName, MethodBase.GetCurrentMethod().Name);

            if (CrmServiceClient == null)
            {
                throw new PlatformException(message: UnknownConnectionErrorMessage);
            }
            else if (CrmServiceClient.LastCrmException == null)
            {
                throw new PlatformException(message: UnknownConnectionErrorMessage);
            }
            else
            {
                throw new PlatformException("Connection error.", CrmServiceClient.LastCrmException);
            }
        }

        private void SetOrganizationService(string authorityUrl, string organizationUrl, string organizationUrlSuffix, string tenantId, string servicePrincipalId, string servicePrincipalSecret)
        {
            Logger.Trace(CultureInfo.InvariantCulture, TraceMessageHelper.EnteredMethod, SystemTypeName, MethodBase.GetCurrentMethod().Name);

            if (string.IsNullOrWhiteSpace(authorityUrl)) throw new ArgumentNullException(nameof(authorityUrl));
            if (string.IsNullOrWhiteSpace(organizationUrl)) throw new ArgumentNullException(nameof(organizationUrl));
            if (string.IsNullOrWhiteSpace(servicePrincipalId)) throw new ArgumentNullException(nameof(servicePrincipalId));
            if (string.IsNullOrWhiteSpace(servicePrincipalSecret)) throw new ArgumentNullException(nameof(servicePrincipalSecret));
            if (string.IsNullOrWhiteSpace(tenantId)) throw new ArgumentNullException(nameof(tenantId));

            for (int i = 0; i < ConnectionRetries; i++)
            {
                Logger.Info(CultureInfo.InvariantCulture, "Connection attempt: {0}", i + 1);

                AuthenticationResult authenticationResult = GetAuthenticationResult(authorityUrl, organizationUrl, tenantId, servicePrincipalId, servicePrincipalSecret);

                if (authenticationResult == null || string.IsNullOrWhiteSpace(authenticationResult.AccessToken))
                {
                    Logger.Warn("Unable to obtain the access token.");

                    if (i < ConnectionRetries - 1) Thread.Sleep(ConnectionPollingInterval);
                }
                else
                {
                    var serviceUri = new Uri(string.Format(CultureInfo.InvariantCulture, "{0}{1}", organizationUrl, organizationUrlSuffix));

                    var organizationWebProxyClient = new OrganizationWebProxyClient(serviceUri, true)
                    {
                        HeaderToken = authenticationResult.AccessToken
                    };

                    CrmServiceClient = new CrmServiceClient(organizationWebProxyClient);

                    OrganizationService = CrmServiceClient.OrganizationWebProxyClient;

                    organizationWebProxyClient.Dispose();

                    Logger.Trace(CultureInfo.InvariantCulture, TraceMessageHelper.ExitingMethod, SystemTypeName, MethodBase.GetCurrentMethod().Name);

                    return;
                }
            }

            Logger.Trace(CultureInfo.InvariantCulture, TraceMessageHelper.AbortingMethod, SystemTypeName, MethodBase.GetCurrentMethod().Name);

            if (CrmServiceClient == null)
            {
                throw new PlatformException(message: UnknownConnectionErrorMessage);
            }
            else if (CrmServiceClient.LastCrmException == null)
            {
                throw new PlatformException(message: UnknownConnectionErrorMessage);
            }
            else
            {
                throw new PlatformException("Connection error.", CrmServiceClient.LastCrmException);
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

            Logger.Trace(CultureInfo.InvariantCulture, TraceMessageHelper.ExitingMethod, SystemTypeName, MethodBase.GetCurrentMethod().Name);
        }

        public void SetOrganizationService()
        {
            Logger.Trace(CultureInfo.InvariantCulture, TraceMessageHelper.EnteredMethod, SystemTypeName, MethodBase.GetCurrentMethod().Name);

            if (string.IsNullOrWhiteSpace(ConnectionString))
            {
                SetOrganizationService(AuthorityUrl, OrganizationUrl, OrganizationUrlSuffix, TenantId, ServicePrincipalId, ServicePrincipalSecret);
            }
            else
            {
                SetOrganizationService(ConnectionString);
            }

            WhoAmI();

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
