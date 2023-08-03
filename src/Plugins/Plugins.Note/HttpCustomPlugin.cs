namespace Plugins.Note
{
    using Aylos.Xrm.Sdk.Plugins;

    using Microsoft.Extensions.DependencyInjection;

    using Shared.BusinessLogic.Services.Note;

    using System;
    using System.Net.Http;
    using System.Net.Http.Headers;

    public abstract class HttpCustomPlugin : CustomPlugin
    {
        #region Static Members

        private static IHttpClientFactory _httpClientFactory;

        private static readonly object _httpLock = new object();

        public static IHttpClientFactory HttpClientFactory  // Parameters from the SecureConfig
        {
            get
            {
                if (_httpClientFactory == null) // or Expired
                {
                    lock (_httpLock)
                    {
                        if (_httpClientFactory == null) // or Expired
                        {
                            var services = new ServiceCollection();

                            services.AddHttpClient<IInitializeEntityService, InitializeEntityService>(client =>
                            {
                                // https://func-np-uks-aylos-webhook-dev-01.azurewebsites.net/api/webhook/plugins/annotation/handle-file-upload?code=TDe2Bbo5cu_Tkl7LqkK78O99wP6ho8SuheyY_AjPDiVjAzFuVjz-eg==
                                client.BaseAddress = new Uri("https://func-np-uks-aylos-webhook-dev-01.azurewebsites.net/");
                                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                                client.DefaultRequestHeaders.AcceptCharset.Add(new StringWithQualityHeaderValue("utf-8"));
                                client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));
                                client.DefaultRequestHeaders.AcceptLanguage.Add(new StringWithQualityHeaderValue("en-GB"));
                                client.DefaultRequestHeaders.Add("OData-MaxVersion", "4.0");
                                client.DefaultRequestHeaders.Add("OData-Version", "4.0");
                                client.DefaultRequestHeaders.Add("x-functions-key", "TDe2Bbo5cu_Tkl7LqkK78O99wP6ho8SuheyY_AjPDiVjAzFuVjz-eg==");
                                client.Timeout = new TimeSpan(0, 1, 0);
                            });

                            var serviceProvider = services.BuildServiceProvider();

                            _httpClientFactory = serviceProvider.GetService<IHttpClientFactory>();
                        }
                    }
                }
                return _httpClientFactory;
            }
        }

        #endregion
    }
}
