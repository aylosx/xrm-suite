namespace Sample.MicroServices
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Extensions.Http;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;
    using System.Threading;
    using System.Net;
    using System.Net.Http;

    public class CallMe
    {
        private readonly HttpClient _httpClient;

        private readonly ILogger<CallMe> _log;

        public CallMe(IHttpClientFactory httpClientFactory, ILoggerFactory loggerFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
            _log = loggerFactory.CreateLogger<CallMe>();
        }

        [FunctionName("MicroServices_CallMe")]
        public async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "micro-services/call-me")] HttpRequest req)
        {
            try {
                _log.LogInformation("CallMe received a HTTP request.");

                var response = await _httpClient.GetAsync("https://microsoft.com");
                Thread.Sleep(5000); // just wait for 5 seconds

                _log.LogInformation("CallMe completed serving the HTTP request.");

                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            catch (Exception ex) {
                _log.LogError(ex, ex.Message);
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
        }
    }
}
