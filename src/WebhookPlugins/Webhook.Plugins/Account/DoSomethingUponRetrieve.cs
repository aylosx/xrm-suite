namespace Webhook.Plugins.Account
{
    using Aylos.Xrm.Sdk.Common;
    using Aylos.Xrm.Sdk.Core.WebhookPlugins;

    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Extensions.Http;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Microsoft.PowerPlatform.Dataverse.Client;
    using Microsoft.Xrm.Sdk;

    using Shared.Models.Domain;

    using System;
    using System.Globalization;
    using System.Net.Http;
    using System.Reflection;
    using System.Threading.Tasks;

    using Webhook.Plugins.BusinessLogic.Services.Account;
    using Webhook.Plugins.BusinessLogic.Services.Data;

    public class DoSomethingUponRetrieve : DataverseWebhookPlugin
    {
        #region Constructors

        public DoSomethingUponRetrieve() { }

        [ActivatorUtilitiesConstructor]
        public DoSomethingUponRetrieve(ICrmService crmService, ServiceClient serviceClient, ILoggerFactory loggerFactory) 
            : base(serviceClient, loggerFactory)
        {
            CrmService = crmService ?? throw new ArgumentNullException(nameof(crmService));
        }

        #endregion

        #region Constants 

        public const string PrimaryEntityLogicalName = Account.EntityLogicalName;

        public const string PluginMessageName = PlatformMessageHelper.Retrieve;

        public const int PluginPipelineStage = (int)SdkMessageProcessingStepStage.PreOperation;

        public const int MaximumAllowedExecutionDepth = 7;

        #endregion

        #region Properties

        public ICrmService CrmService { get; set; }

        public ISampleService SampleService { get; set; }

        #endregion

        #region Azure Function HttpTrigger

        [FunctionName("Account_DoSomethingUponRetrieve")]
        public async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "webhook/plugins/account/do-something-upon-retrieve")] HttpRequestMessage req)
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

            using (SampleService ??= new SampleService(CrmService, ServiceClient, RemoteExecutionContext, HttpRequestMessage, LoggerFactory))
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
                if (!RemoteExecutionContext.InputParameters.Contains(PlatformConstants.TargetText) ||
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
            if (RemoteExecutionContext.Stage != (int)SdkMessageProcessingStepStage.PreOperation)
            {
                throw new InvalidPluginExecutionException(string.Format(CultureInfo.InvariantCulture,
                    TraceMessageHelper.IncorrectPipelineStage, UnderlyingSystemTypeName,
                    (int)SdkMessageProcessingStepStage.PreOperation, RemoteExecutionContext.Stage));
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