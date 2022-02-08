namespace Plugins.ExecutionContext
{
    using System.Globalization;
    using System.Reflection;

    using Microsoft.Xrm.Sdk;

    using Aylos.Xrm.Sdk.Common;
    using Aylos.Xrm.Sdk.Plugins;

    using Shared.Models.Domain;
    using Shared.BusinessLogic.Services.ExecutionContext;
    using Shared.BusinessLogic.Services.Data;

    /// <summary>
    /// Posts the message content upon creation. 
    /// </summary>
    public class PostMessageUponCreate : CustomPlugin
    {
        #region Constructors

        /// <summary>
        /// InitializeEntityUponCreation constructor initializes the context variables.
        /// </summary>
        public PostMessageUponCreate()
        {
            PrimaryEntityLogicalName = Shared.Models.Domain.ExecutionContext.EntityLogicalName;
            PluginMessageName = PlatformMessageHelper.Create;
            PluginPipelineStage = (int)SdkMessageProcessingStepStage.PostOperation;
            MaximumAllowedExecutionDepth = 7;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the organization service context.
        /// </summary>
        /// <value>The organization service context object instance.</value>
        public CrmServiceContext OrganizationServiceContext { get; set; }

        /// <summary>
        /// Gets or sets the CRM service.
        /// </summary>
        /// <value>The CRM service object instance.</value>
        public ICrmService CrmService { get; set; }

        /// <summary>
        /// Gets or sets the PostMessageService service.
        /// </summary>
        /// <value>The PostMessageService service object instance.</value>
        public IPostMessageService PostMessageService { get; set; }

        #endregion

        #region Override Base Plugin Methods

        /// <summary>
        /// Executes the business logic. 
        /// </summary>
        public override void Execute()
        {
            Trace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.EnteredMethod, UnderlyingSystemTypeName, MethodBase.GetCurrentMethod().Name));

            using (OrganizationServiceContext = OrganizationServiceContext ?? new CrmServiceContext(OrganizationService))
            using (CrmService = CrmService ?? new CrmService(OrganizationServiceContext, TracingService))
            using (PostMessageService = PostMessageService ?? new PostMessageService(CrmService, OrganizationServiceContext, PluginExecutionContext, TracingService))
            {
                Trace(string.Format(CultureInfo.InvariantCulture, "{0} | {1} started at {2} milliseconds", UnderlyingSystemTypeName, MethodBase.GetCurrentMethod().Name, Stopwatch.ElapsedMilliseconds));
                PostMessageService.PostMessage();
                Trace(string.Format(CultureInfo.InvariantCulture, "{0} | {1} ended at {2} milliseconds", UnderlyingSystemTypeName, MethodBase.GetCurrentMethod().Name, Stopwatch.ElapsedMilliseconds));
            }

            Trace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.ExitingMethod, UnderlyingSystemTypeName, MethodBase.GetCurrentMethod().Name));
        }

        /// <summary>
        /// Validates the execution context.
        /// </summary>
        public override void Validate()
        {
            Trace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.EnteredMethod, UnderlyingSystemTypeName, MethodBase.GetCurrentMethod().Name));

            // Check if the plugin execution context contains target entity
            if (!(PluginExecutionContext.InputParameters.Contains(PlatformConstants.TargetText)) ||
                !(PluginExecutionContext.InputParameters[PlatformConstants.TargetText] is Entity))
            {
                throw new InvalidPluginExecutionException(string.Format(CultureInfo.InvariantCulture,
                    TraceMessageHelper.TargetEntityIsRequired, UnderlyingSystemTypeName));
            }

            // Check if the target entity logical name is the expected one
            Entity target = (Entity)PluginExecutionContext.InputParameters[PlatformConstants.TargetText];
            if (target.LogicalName != PrimaryEntityLogicalName)
            {
                throw new InvalidPluginExecutionException(string.Format(CultureInfo.InvariantCulture,
                    TraceMessageHelper.WrongRegisteredEntity, UnderlyingSystemTypeName, target.LogicalName));
            }

            // Check if the running message is the expected one
            if (PluginExecutionContext.MessageName != PlatformMessageHelper.Create)
            {
                throw new InvalidPluginExecutionException(string.Format(CultureInfo.InvariantCulture,
                    TraceMessageHelper.IncorrectMessageName, UnderlyingSystemTypeName,
                    PlatformMessageHelper.Create, PluginExecutionContext.MessageName));
            }

            // Check if the plugin current pipeline stage is the expected one
            if (PluginExecutionContext.Stage != (int)SdkMessageProcessingStepStage.PostOperation)
            {
                throw new InvalidPluginExecutionException(string.Format(CultureInfo.InvariantCulture,
                    TraceMessageHelper.IncorrectPipelineStage, UnderlyingSystemTypeName,
                    (int)SdkMessageProcessingStepStage.PostOperation, PluginExecutionContext.Stage));
            }

            // Check if the plugin depth is not exiting the max depth limit expectation
            if (PluginExecutionContext.Depth > MaximumAllowedExecutionDepth)
            {
                throw new InvalidPluginExecutionException(string.Format(CultureInfo.InvariantCulture,
                    TraceMessageHelper.MaxDepthViolation, UnderlyingSystemTypeName, MaximumAllowedExecutionDepth, PluginExecutionContext.Depth));
            }

            Trace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.ExitingMethod, UnderlyingSystemTypeName, MethodBase.GetCurrentMethod().Name));
        }

        #endregion
    }
}
