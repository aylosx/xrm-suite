using Aylos.Xrm.Sdk.Core.WebhookPlugins;
using Azure.Core;
using Azure.Identity;

using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.PowerPlatform.Dataverse.Client;
using Microsoft.Xrm.Sdk;

using Shared.Models.Domain;

using System;
using System.IO;
using System.Net.Http.Headers;
using System.Threading.Tasks;

using Webhook.Plugins.BusinessLogic.Services.Data;
using Webhook.Plugins.BusinessLogic.Services.Note;
using Webhook.Plugins.Note;

[assembly: FunctionsStartup(typeof(Webhook.Plugins.Startup))]
namespace Webhook.Plugins
{
    public class Startup : FunctionsStartup
    {
        DataverseServiceConfig _dataverseServiceConfig;
        FileHandlingServiceConfig _fileApiConfig;

        public override void Configure(IFunctionsHostBuilder builder)
        {
            FunctionsHostBuilderContext context = builder.GetContext();

            _dataverseServiceConfig = new DataverseServiceConfig();
            context.Configuration.Bind(DataverseServiceConfig.DataverseApiConfig, _dataverseServiceConfig);

            _fileApiConfig = new FileHandlingServiceConfig();
            context.Configuration.Bind(FileHandlingServiceConfig.FileApiConfig, _fileApiConfig);

            builder.Services.AddHttpClient<IFileHandlingService, FileHandlingService>("fileapi", client =>
            {
                client.BaseAddress = new Uri(_fileApiConfig.BaseAddress);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.AcceptCharset.Add(new StringWithQualityHeaderValue("utf-8"));
                client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));
                client.DefaultRequestHeaders.AcceptLanguage.Add(new StringWithQualityHeaderValue("en-GB"));
                // client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("DataverseWebHooks"));
                client.DefaultRequestHeaders.Add("OData-MaxVersion", "4.0");
                client.DefaultRequestHeaders.Add("OData-Version", "4.0");
                // TO-DO: add other HTTP headers
                // client.DefaultRequestHeaders.Add("Authorization", "YOUR_ASSEMBLY_AI_TOKEN");
                
            })
            .AddTypedClient<HandleFileDeletionUponDelete>()
            .AddTypedClient<HandleFileDownloadUponRetrieve>()
            .AddTypedClient<HandleFileUploadUponCreate>()
            ;

            builder.Services.AddLogging();

            builder.Services.AddMemoryCache();

            builder.Services.AddSingleton(new DefaultAzureCredential());

            builder.Services.AddSingleton<IOrganizationService, ServiceClient>(provider =>
            {
                var cache = provider.GetService<IMemoryCache>();
                var credential = provider.GetRequiredService<DefaultAzureCredential>();
                return new ServiceClient(
                    tokenProviderFunction: f => GetDataverseToken(_dataverseServiceConfig.OrganizationUrl, _dataverseServiceConfig.TokenExpiryTime, credential, cache),
                    instanceUrl: new Uri(_dataverseServiceConfig.OrganizationUrl),
                    useUniqueInstance: true
                );
            });

            builder.Services.AddSingleton(provider => {
                var organizationService = provider.GetRequiredService<IOrganizationService>();
                return new CrmServiceContext(organizationService);
            });

            builder.Services.AddSingleton<ICrmService>(provider => {
                var organizationServiceContext = provider.GetRequiredService<CrmServiceContext>();
                var loggerFactory = provider.GetRequiredService<ILoggerFactory>();
                return new CrmService(organizationServiceContext, loggerFactory);
            });
        }

        public override void ConfigureAppConfiguration(IFunctionsConfigurationBuilder builder)
        {
            base.ConfigureAppConfiguration(builder);

            var context = builder.GetContext();

            builder.ConfigurationBuilder
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(Path.Combine(context.ApplicationRootPath, "appsettings.json"), optional: false, reloadOnChange: true)
                .AddJsonFile(Path.Combine(context.ApplicationRootPath, "local.settings.json"), optional: false, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();
        }

        private static async Task<string> GetDataverseToken(string dataverseUrl, int dataverseTokenExpiryTime, DefaultAzureCredential credential, IMemoryCache cache)
        {
            var accessToken = await cache.GetOrCreateAsync(dataverseUrl, async (cacheEntry) => {
                cacheEntry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(dataverseTokenExpiryTime);
                return await credential.GetTokenAsync(new TokenRequestContext(new[] { $"{dataverseUrl}.default" }));
            });
            return accessToken.Token;
        }
    }
}
