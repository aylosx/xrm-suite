namespace Plugins.AnyEntity
{
    using System.Globalization;
    using System.Reflection;

    using Microsoft.Xrm.Sdk;

    using Aylos.Xrm.Sdk.Common;
    using Aylos.Xrm.Sdk.Plugins;

    using Shared.Models.Domain;
    using Shared.BusinessLogic.Services.Data;
    using Shared.BusinessLogic.Services.ExecutionContext;

    /// <summary>
    /// Stores the execution context. 
    /// </summary>
    public class StoreContextUponAnyEvent : CustomPlugin
    {
        #region Constructors

        /// <summary>
        /// StoreContextUponAnyEvent constructor initializes the context variables.
        /// </summary>
        public StoreContextUponAnyEvent()
        {
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
        /// Gets or sets the ManageExecutionContext service.
        /// </summary>
        /// <value>The ManageExecutionContext service object instance.</value>
        public IManageExecutionContext ManageExecutionContext { get; set; }

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
            using (ManageExecutionContext = ManageExecutionContext ?? new ManageExecutionContext(CrmService, OrganizationServiceContext, PluginExecutionContext, TracingService))
            {
                Trace(string.Format(CultureInfo.InvariantCulture, "{0} | {1} started at {2} milliseconds", UnderlyingSystemTypeName, MethodBase.GetCurrentMethod().Name, Stopwatch.ElapsedMilliseconds));
                ManageExecutionContext.StoreExecutionContext();
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
