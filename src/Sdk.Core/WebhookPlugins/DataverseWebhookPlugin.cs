namespace Aylos.Xrm.Sdk.Core.WebhookPlugins
{
    using Aylos.Xrm.Sdk.Common;

    using Microsoft.Azure.WebJobs.Logging;
    using Microsoft.Crm.Sdk.Messages;
    using Microsoft.Extensions.Logging;
    using Microsoft.PowerPlatform.Dataverse.Client;
    using Microsoft.Xrm.Sdk;

    using System;
    using System.Diagnostics;
    using System.Globalization;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Reflection;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;

    using TraceHelper = Plugins.TraceHelper;

    public abstract class DataverseWebhookPlugin
    {
        #region Constructor

        public DataverseWebhookPlugin(ServiceClient serviceClient, ILoggerFactory loggerFactory)
        {
            ServiceClient = serviceClient ?? throw new ArgumentNullException(nameof(serviceClient));

            LoggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));

            Logger = loggerFactory.CreateLogger(LogCategories.CreateFunctionUserCategory(UnderlyingSystemTypeName));

            WhoAmI(serviceClient);
        }

        #endregion

        #region Constants 

        public const string RemoteExecutionContextMessage = "Unable to get the remote execution context.";

        #endregion

        #region Properties

        public string PrimaryEntityLogicalName { get; protected set; }

        public string PluginMessageName { get; protected set; }

        public int PluginPipelineStage { get; protected set; }

        public int MaximumAllowedExecutionDepth { get; protected set; }

        public ILoggerFactory LoggerFactory { get; private set; }

        public ILogger Logger { get; private set; }

        public HttpRequestMessage HttpRequestMessage { get; private set; }

        public ServiceClient ServiceClient { get; private set; }

        public RemoteExecutionContext RemoteExecutionContext { get; private set; }

        protected Stopwatch Stopwatch { get; private set; }

        public string UnderlyingSystemTypeName { get { return GetType().UnderlyingSystemType.Name; } }

        public string HttpMessageOrganisationName 
        { 
            get 
            {
                string ret = null;
                if (HttpRequestMessage.Headers.Contains("x-ms-dynamics-organization"))
                {
                    ret = HttpRequestMessage.Headers.GetValues("x-ms-dynamics-organization").SingleOrDefault();
                }
                return ret; 
            } 
        }

        public string HttpMessageEntityName
        { 
            get
            {
                string ret = null;
                if (HttpRequestMessage.Headers.Contains("x-ms-dynamics-entity-name"))
                {
                    ret = HttpRequestMessage.Headers.GetValues("x-ms-dynamics-entity-name").SingleOrDefault();
                }
                return ret; 
            } 
        }

        public string HttpMessageEventName
        { 
            get
            {
                string ret = null;
                if (HttpRequestMessage.Headers.Contains("x-ms-dynamics-request-name"))
                {
                    ret = HttpRequestMessage.Headers.GetValues("x-ms-dynamics-request-name").SingleOrDefault();
                }
                return ret; 
            } 
        }

        public Guid? HttpMessageCorrelationId
        {
            get
            {
                Guid? ret = null;
                if (HttpRequestMessage.Headers.Contains("x-ms-dynamics-request-name"))
                {
                    ret = Guid.Parse(HttpRequestMessage.Headers.GetValues("x-ms-correlation-request-id").SingleOrDefault());
                }
                return ret;
            }
        }

        public bool HttpMessageSizeExceeded => HttpRequestMessage.Headers.Contains("x-ms-dynamics-msg-size-exceeded");

        #endregion

        #region Methods

        private static T DeserializeJson<T>(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) throw new ArgumentNullException(nameof(input));

            string formattedJSON = Regex.Unescape(input.Trim('"'));
            string regexDatePattern = @"([\/][D][a][t][e][(][0-9]+[)][\/])+";
            string regexDatePatternIn = @"([0-9]+)+";
            foreach (var match in new Regex(regexDatePattern).Matches(formattedJSON))
            {
                string dtout = ((Match)match).Value;
                long dtin = long.Parse(Regex.Match(dtout, regexDatePatternIn).Value);
                formattedJSON = formattedJSON.Replace(dtout, DateTimeOffset.FromUnixTimeMilliseconds(dtin).DateTime.ToString(SerializationHelper.DateTimeFormatText));
            }

            return SerializationHelper.DeserializeJson<T>(formattedJSON);
        }

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

                Logger.LogInformation("Event: ", HttpMessageEventName);
                Logger.LogInformation("Entity: ", HttpMessageEntityName);
                Logger.LogInformation("Organisation: ", HttpMessageOrganisationName);
                Logger.LogInformation("Correlation: ", HttpMessageCorrelationId);
                Logger.LogInformation("Size Exceeded: ", HttpMessageSizeExceeded);

                string body = await HttpRequestMessage.Content.ReadAsStringAsync();
                if (string.IsNullOrWhiteSpace(body)) throw new InvalidOperationException(nameof(body));

                RemoteExecutionContext = DeserializeJson<RemoteExecutionContext>(body);
                if (RemoteExecutionContext == null) throw new InvalidOperationException(RemoteExecutionContextMessage);

                Logger.LogTrace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.StartTracing, UnderlyingSystemTypeName, Stopwatch.ElapsedMilliseconds));
                Logger.LogTrace(TraceHelper.Trace(RemoteExecutionContext)); // Trace the remote execution context
                Logger.LogTrace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.ExecutionContextTracingCompleted, UnderlyingSystemTypeName, Stopwatch.ElapsedMilliseconds));

                Validate(); // Call the overriden validate method

                Execute(); // Call the overriden execute method

                string rec = SerializationHelper.SerializeJson(RemoteExecutionContext);

                res = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(rec), // TO-DO: Performance Considerations
                    RequestMessage = req,
                };
            }
            catch (Exception ex)
            {
                var sb = new StringBuilder();

                sb.AppendLine(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.UndefinedFault, UnderlyingSystemTypeName, ex.Message, ex.StackTrace));

                void le(Exception ex) { sb.AppendLine(ex.Message); sb.AppendLine(ex.StackTrace); if (ex.InnerException != null) le(ex.InnerException); }

                le(ex);

                Logger.LogError(sb.ToString());

                res = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent(sb.ToString()),
                    RequestMessage = req,
                };
            }
            finally
            {
                if (Stopwatch != null)
                {
                    Logger.LogTrace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.ExecutionContextTracingCompleted, UnderlyingSystemTypeName, Stopwatch.Elapsed.TotalMilliseconds));
                    Stopwatch.Stop();
                    Stopwatch = null;
                }

                Logger.LogTrace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.StopTracing, UnderlyingSystemTypeName));
            }

            Logger.LogTrace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.ExitingMethod, UnderlyingSystemTypeName, MethodBase.GetCurrentMethod().Name));

            return res;
        }

        private void WhoAmI(IOrganizationService organizationService)
        {
            Logger.LogTrace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.EnteredMethod, UnderlyingSystemTypeName, MethodBase.GetCurrentMethod().Name));

            Logger.LogInformation(string.Format(CultureInfo.InvariantCulture, "Connection established successfully."));

            Logger.LogInformation(string.Format(CultureInfo.InvariantCulture, "Retrieving current user, business unit and organization."));
            var whoAmIResponse = (WhoAmIResponse)organizationService.Execute(new WhoAmIRequest());
            Logger.LogInformation(string.Format(CultureInfo.InvariantCulture, "Business unit: {0}", whoAmIResponse.BusinessUnitId));
            Logger.LogInformation(string.Format(CultureInfo.InvariantCulture, "Organization: {0}", whoAmIResponse.OrganizationId));
            Logger.LogInformation(string.Format(CultureInfo.InvariantCulture, "User: {0}", whoAmIResponse.UserId));

            Logger.LogInformation(string.Format(CultureInfo.InvariantCulture, "Retrieving the instance version."));
            var versionResponse = (RetrieveVersionResponse)organizationService.Execute(new RetrieveVersionRequest());
            Logger.LogInformation(string.Format(CultureInfo.InvariantCulture, "Instance version {0}.", versionResponse.Version));

            Logger.LogTrace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.ExitingMethod, UnderlyingSystemTypeName, MethodBase.GetCurrentMethod().Name));
        }

        #endregion

        #region Overriden Methods

        protected abstract void Execute();

        protected abstract void Validate();

        #endregion
    }
}
