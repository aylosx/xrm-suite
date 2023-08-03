namespace Sample.MicroServices.BusinessLogic.Services.Sdk
{
    using Microsoft.Azure.WebJobs.Logging;
    using Microsoft.Extensions.Logging;

    using Newtonsoft.Json;

    using System;
    using System.Diagnostics;
    using System.Globalization;
    using System.Net;
    using System.Net.Http;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;

    public abstract class MicroServiceApi<I, O>
    {
        #region Constructor

        public MicroServiceApi() 
        {
        }

        public MicroServiceApi(ILoggerFactory loggerFactory) : this()
        {
            LoggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));

            Logger = loggerFactory.CreateLogger(LogCategories.CreateFunctionUserCategory(UnderlyingSystemTypeName));
        }

        #endregion

        #region Constants 

        public const string HttpRequestMessageBodyIsEmpty = "Looks like the HTTP request message is empty but it is required.";
        public const string InputContextIsEmpty = "Looks like the input context is empty but it is required.";

        #endregion

        #region Properties

        public I Input { get; set; }

        public O Output { get; set; }

        public ILogger Logger { get; set; }

        public ILoggerFactory LoggerFactory { get; set; }

        public HttpRequestMessage HttpRequestMessage { get; set; }

        protected Stopwatch Stopwatch { get; private set; }

        public string UnderlyingSystemTypeName { get { return GetType().UnderlyingSystemType.Name; } }

        #endregion

        #region Methods

        public async Task<HttpResponseMessage> Execute(HttpRequestMessage req)
        {
            Logger.LogTrace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.EnteredMethod, UnderlyingSystemTypeName, MethodBase.GetCurrentMethod().Name));

            HttpRequestMessage = req ?? throw new ArgumentNullException(nameof(req));

            var res = new HttpResponseMessage(HttpStatusCode.InternalServerError)
            {
                Content = new StringContent("An unexpected error has occured, request has been declined."),
                RequestMessage = req,
            };

            try
            {
                Stopwatch = Stopwatch.StartNew();

                string body = await HttpRequestMessage.Content.ReadAsStringAsync();
                if (string.IsNullOrWhiteSpace(body)) throw new InvalidOperationException(HttpRequestMessageBodyIsEmpty);

                Input = JsonConvert.DeserializeObject<I>(body);
                if (Input == null) throw new InvalidOperationException(InputContextIsEmpty);

                Validate(); // Calls the overriden validate method

                Execute(); // Calls the overriden execute method

                string rec = JsonConvert.SerializeObject(Output);

                res = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(rec, Encoding.UTF8, "application/json"), // TO-DO: Performance Considerations
                    RequestMessage = req,
                };
            }
            catch (Exception ex)
            {
                var sb = new StringBuilder();

                void le(Exception ex) { sb.AppendLine(ex.Message); sb.AppendLine(ex.StackTrace); if (ex.InnerException != null) le(ex.InnerException); }

                le(ex);

                Logger.LogError(sb.ToString());

                res = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent(ex.Message, Encoding.UTF8),
                    RequestMessage = req,
                };
            }
            finally
            {
                if (Stopwatch != null)
                {
                    Logger.LogTrace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.TracingCompleted, UnderlyingSystemTypeName, Stopwatch.Elapsed.TotalMilliseconds));
                    Stopwatch.Stop();
                    Stopwatch = null;
                }

                Logger.LogTrace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.StopTracing, UnderlyingSystemTypeName));
            }

            Logger.LogTrace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.ExitingMethod, UnderlyingSystemTypeName, MethodBase.GetCurrentMethod().Name));

            return res;
        }

        #endregion

        #region Overriden Methods

        public abstract void Execute();

        public abstract void Validate();

        #endregion
    }
}
