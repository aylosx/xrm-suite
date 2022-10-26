namespace WebHooks.Functions.Note
{
    using Aylos.Xrm.Sdk.Common;
    using Aylos.Xrm.Sdk.Core.WebHooks;

    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Extensions.Http;
    using Microsoft.Extensions.Logging;
    using Microsoft.Xrm.Sdk;

    using Shared.Models.Domain;

    using System.Globalization;
    using System.Net.Http;
    using System.Reflection;
    using System.Threading.Tasks;

    using WebHooks.BusinessLogic.Services.Note;
    using WebHooks.BusinessLogic.Services.Data;
    using System;

    public class HandleFileDownloadUponRetrieve : DataverseWebHook
    {
        #region Constructors

        public HandleFileDownloadUponRetrieve(IHttpClientFactory httpClientFactory, ICrmService crmService, IOrganizationService organizationService, ILoggerFactory loggerFactory) 
            : base(organizationService, loggerFactory)
        {
            if (httpClientFactory == null) throw new ArgumentNullException(nameof(httpClientFactory));
            HttpClient = httpClientFactory.CreateClient("fileapi");

            CrmService = crmService ?? throw new ArgumentNullException(nameof(crmService));

            PrimaryEntityLogicalName = Note.EntityLogicalName;
            PluginMessageName = PlatformMessageHelper.Retrieve;
            PluginPipelineStage = (int)SdkMessageProcessingStepStage.PostOperation;
            MaximumAllowedExecutionDepth = 7;
        }

        #endregion

        #region Properties

        public ICrmService CrmService { get; private set; }

        public IFileHandlingService FileHandlingService { get; private set; }

        public HttpClient HttpClient { get; private set; }

        #endregion

        #region Azure Function HttpTrigger

        [FunctionName("Note_HandleFileDownloadUponRetrieve")]
        public async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "web-hooks/annotation/handle-file-download")] HttpRequestMessage req)
        {
            Logger.LogTrace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.EnteredMethod, UnderlyingSystemTypeName, MethodBase.GetCurrentMethod().Name));

            HttpResponseMessage res = await Execute(req);

            Logger.LogTrace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.ExitingMethod, UnderlyingSystemTypeName, MethodBase.GetCurrentMethod().Name));

            return res;
        }

        #endregion

        #region Override Base Plugin Methods

        protected override void Execute()
        {
            Logger.LogTrace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.EnteredMethod, UnderlyingSystemTypeName, MethodBase.GetCurrentMethod().Name));

            using (FileHandlingService ??= new FileHandlingService(HttpClient, CrmService, OrganizationService, RemoteExecutionContext, LoggerFactory))
            {
                Logger.LogInformation(string.Format(CultureInfo.InvariantCulture, "{0} | {1} started at {2} milliseconds", UnderlyingSystemTypeName, MethodBase.GetCurrentMethod().Name, Stopwatch.ElapsedMilliseconds));
                FileHandlingService.HandleFileDownload(HttpRequestMessage);
                Logger.LogInformation(string.Format(CultureInfo.InvariantCulture, "{0} | {1} ended at {2} milliseconds", UnderlyingSystemTypeName, MethodBase.GetCurrentMethod().Name, Stopwatch.ElapsedMilliseconds));
            }

            Logger.LogTrace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.ExitingMethod, UnderlyingSystemTypeName, MethodBase.GetCurrentMethod().Name));
        }

        protected override void Validate()
        {
            Logger.LogTrace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.EnteredMethod, UnderlyingSystemTypeName, MethodBase.GetCurrentMethod().Name));

            if (HttpMessageSizeExceeded)
            {
                // Check if the target entity logical name is the expected one
                if (HttpMessageEntityName != PrimaryEntityLogicalName)
                {
                    throw new InvalidPluginExecutionException(string.Format(CultureInfo.InvariantCulture,
                        TraceMessageHelper.WrongRegisteredEntity, UnderlyingSystemTypeName, HttpMessageEntityName));
                }
            }
            else 
            {
                // Check if the plugin execution context contains target entity
                if (!(RemoteExecutionContext.InputParameters.Contains(PlatformConstants.TargetText)) ||
                    !(RemoteExecutionContext.InputParameters[PlatformConstants.TargetText] is Entity))
                {
                    throw new InvalidPluginExecutionException(string.Format(CultureInfo.InvariantCulture,
                        TraceMessageHelper.TargetEntityIsRequired, UnderlyingSystemTypeName));
                }

                // Check if the target entity logical name is the expected one
                Entity target = (Entity)RemoteExecutionContext.InputParameters[PlatformConstants.TargetText];
                if (target.LogicalName != PrimaryEntityLogicalName)
                {
                    throw new InvalidPluginExecutionException(string.Format(CultureInfo.InvariantCulture,
                        TraceMessageHelper.WrongRegisteredEntity, UnderlyingSystemTypeName, target.LogicalName));
                }
            }

            // Check if the running message is the expected one
            if (RemoteExecutionContext.MessageName != PlatformMessageHelper.Retrieve)
            {
                throw new InvalidPluginExecutionException(string.Format(CultureInfo.InvariantCulture,
                    TraceMessageHelper.IncorrectMessageName, UnderlyingSystemTypeName,
                    PlatformMessageHelper.Retrieve, RemoteExecutionContext.MessageName));
            }

            // Check if the plugin current pipeline stage is the expected one
            if (RemoteExecutionContext.Stage != (int)SdkMessageProcessingStepStage.PostOperation)
            {
                throw new InvalidPluginExecutionException(string.Format(CultureInfo.InvariantCulture,
                    TraceMessageHelper.IncorrectPipelineStage, UnderlyingSystemTypeName,
                    (int)SdkMessageProcessingStepStage.PostOperation, RemoteExecutionContext.Stage));
            }

            // Check if the plugin depth is not exiting the max depth limit expectation
            if (RemoteExecutionContext.Depth > MaximumAllowedExecutionDepth)
            {
                throw new InvalidPluginExecutionException(string.Format(CultureInfo.InvariantCulture,
                    TraceMessageHelper.MaxDepthViolation, UnderlyingSystemTypeName, MaximumAllowedExecutionDepth, RemoteExecutionContext.Depth));
            }

            Logger.LogTrace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.ExitingMethod, UnderlyingSystemTypeName, MethodBase.GetCurrentMethod().Name));
        }

        #endregion
    }
}
