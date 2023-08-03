namespace Sample.MicroServices
{
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Extensions.Http;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

    using Sample.MicroServices.Models.Domain.Sample;
    using Sample.MicroServices.BusinessLogic.Services.Sample;
    using Sample.MicroServices.BusinessLogic.Services.Sdk;

    using System;
    using System.Threading.Tasks;
    using System.Net.Http;
    using System.Globalization;
    using System.Reflection;

    public class SampleApiCall : MicroServiceApi<SampleInput, SampleOutput>
    {
        #region Constructors

        public SampleApiCall() { }

        [ActivatorUtilitiesConstructor]
        public SampleApiCall(IHttpClientFactory httpClientFactory, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            if (httpClientFactory == null) throw new ArgumentNullException(nameof(httpClientFactory));
            HttpClient = httpClientFactory.CreateClient(SampleServiceConfig.SampleApiConfig);
        }

        #endregion

        #region Constants 

        public readonly string SampleErrorMessageAbc = "The Abc value must be greater than 100.";
        public readonly string SampleErrorMessageEfg = "The Efg value must be greater than 100.";
        public readonly string SampleErrorIdRequired = "The Id value is required.";

        #endregion

        #region Properties

        public ISampleService SampleService { get; set; }

        public HttpClient HttpClient { get; set; }

        #endregion

        #region Azure Function HttpTrigger

        [FunctionName("MicroServices_SampleApiCall")]
        public async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "microservices/sample-api-call")] HttpRequestMessage req)
        {
            Logger.LogTrace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.EnteredMethod, UnderlyingSystemTypeName, MethodBase.GetCurrentMethod().Name));

            HttpResponseMessage res = await Execute(req);

            Logger.LogTrace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.ExitingMethod, UnderlyingSystemTypeName, MethodBase.GetCurrentMethod().Name));

            return res;
        }

        #endregion

        #region Override Base Methods

        public override void Execute()
        {
            Logger.LogTrace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.EnteredMethod, UnderlyingSystemTypeName, MethodBase.GetCurrentMethod().Name));

            using (SampleService ??= new SampleService(LoggerFactory))
            {
                Logger.LogInformation(string.Format(CultureInfo.InvariantCulture, "{0} | {1} started at {2} milliseconds", UnderlyingSystemTypeName, MethodBase.GetCurrentMethod().Name, Stopwatch.ElapsedMilliseconds));
                SampleService.DoSomething();
                Logger.LogInformation(string.Format(CultureInfo.InvariantCulture, "{0} | {1} ended at {2} milliseconds", UnderlyingSystemTypeName, MethodBase.GetCurrentMethod().Name, Stopwatch.ElapsedMilliseconds));
            }

            Logger.LogTrace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.ExitingMethod, UnderlyingSystemTypeName, MethodBase.GetCurrentMethod().Name));
        }

        public override void Validate()
        {
            Logger.LogTrace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.EnteredMethod, UnderlyingSystemTypeName, MethodBase.GetCurrentMethod().Name));

            if (Input.Abc <= 100) throw new InvalidOperationException(SampleErrorMessageAbc);
            if (Input.Efg <= 100) throw new InvalidOperationException(SampleErrorMessageEfg);
            if (Input.Id.Equals(Guid.Empty)) throw new InvalidOperationException(SampleErrorIdRequired);

            Logger.LogTrace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.ExitingMethod, UnderlyingSystemTypeName, MethodBase.GetCurrentMethod().Name));
        }

        #endregion
    }
}
