using Azure.Identity;

using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Sample.MicroServices.BusinessLogic.Services.Sample;
using System;
using System.IO;
using System.Net.Http.Headers;

[assembly: FunctionsStartup(typeof(Sample.MicroServices.Startup))]
namespace Sample.MicroServices
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            FunctionsHostBuilderContext context = builder.GetContext();

            IConfiguration configuration = context.Configuration;

            var sampleApiConfig = new SampleServiceConfig();

            context.Configuration.Bind(SampleServiceConfig.SampleApiConfig, sampleApiConfig);

            builder.Services.AddHttpClient<ISampleService, SampleService>("sampleapi", client =>
            {
                client.BaseAddress = new Uri(sampleApiConfig.BaseAddress);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.AcceptCharset.Add(new StringWithQualityHeaderValue("utf-8"));
                client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));
                client.DefaultRequestHeaders.AcceptLanguage.Add(new StringWithQualityHeaderValue("en-GB"));
                // client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("MyUserAgent"));
                client.DefaultRequestHeaders.Add("OData-MaxVersion", "4.0");
                client.DefaultRequestHeaders.Add("OData-Version", "4.0");
                // TO-DO: add other HTTP headers
                // client.DefaultRequestHeaders.Add("Authorization", "YOUR_ASSEMBLY_AI_TOKEN");

            })
            .AddTypedClient<SampleApiCall>()
            ;

            builder.Services.AddLogging();

            builder.Services.AddMemoryCache();

            builder.Services.AddSingleton(new DefaultAzureCredential());
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
