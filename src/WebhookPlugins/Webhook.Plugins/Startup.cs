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
        public override void Configure(IFunctionsHostBuilder builder)
        {
            FunctionsHostBuilderContext context = builder.GetContext();

            var dataverseServiceConfig = new DataverseServiceConfig();
            context.Configuration.Bind(DataverseServiceConfig.DataverseApiConfig, dataverseServiceConfig);

            var fileApiConfig = new FileHandlingServiceConfig();
            context.Configuration.Bind(FileHandlingServiceConfig.FileApiConfig, fileApiConfig);

            builder.Services.AddHttpClient<IFileHandlingService, FileHandlingService>("fileapi", client =>
            {
                client.BaseAddress = new Uri(fileApiConfig.BaseAddress);
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

            builder.Services.AddSingleton(provider => // DataverseServiceClient
            {
                var cache = provider.GetRequiredService<IMemoryCache>();
                var credential = provider.GetRequiredService<DefaultAzureCredential>();
                var dataverseServiceClient = new DataverseServiceClient(dataverseServiceConfig, credential, cache);
                return dataverseServiceClient;
            });

            builder.Services.AddTransient(provider => // ServiceClient
            { 
                var dataverseServiceClient = provider.GetRequiredService<DataverseServiceClient>();
                var serviceClient = dataverseServiceClient.instance.Clone();
                return serviceClient;
            });

            builder.Services.AddTransient(provider => // CrmServiceContext
            {
                var serviceClient = provider.GetRequiredService<ServiceClient>();
                var organizationServiceContext = new CrmServiceContext(serviceClient);
                return organizationServiceContext;
            });

            builder.Services.AddTransient<ICrmService>(provider => // CrmService
            {
                var serviceClient = provider.GetRequiredService<ServiceClient>();
                var organizationServiceContext = provider.GetRequiredService<CrmServiceContext>();
                var loggerFactory = provider.GetRequiredService<ILoggerFactory>();
                var crmService = new CrmService(serviceClient, organizationServiceContext, loggerFactory);
                return crmService;
            });
        }

        public override void ConfigureAppConfiguration(IFunctionsConfigurationBuilder builder)
        {
            base.ConfigureAppConfiguration(builder);

            var context = builder.GetContext();

            builder.ConfigurationBuilder
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(Path.Combine(context.ApplicationRootPath, "appsettings.json"), optional: false, reloadOnChange: true)
                .AddJsonFile(Path.Combine(context.ApplicationRootPath, "local.settings.json"), optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();
        }
    }
}
