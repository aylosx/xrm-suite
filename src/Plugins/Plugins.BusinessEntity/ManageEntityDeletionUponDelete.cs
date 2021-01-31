namespace Plugins.BusinessEntity
{
    using System.Globalization;
    using System.Reflection;

    using Microsoft.Xrm.Sdk;

    using Aylos.Xrm.Sdk.Common;
    using Aylos.Xrm.Sdk.Plugins;

    using Shared.Models.Domain;
    using Shared.BusinessLogic.Services.Account;
    using Shared.BusinessLogic.Services.Data;

    /// <summary>
    /// Manages deletion tasks of the business entity.
    /// - Blocks deletion. 
    /// - Dissassociates descendants upon delete.
    /// </summary>
    public class ManageEntityDeletionUponDelete : CustomPlugin
    {
        #region Constructors

        /// <summary>
        /// ManageEntityDeletionUponDelete constructor initializes the context variables.
        /// </summary>
        public ManageEntityDeletionUponDelete()
        {
            PrimaryEntityLogicalName = Account.EntityLogicalName;
            PluginMessageName = PlatformMessageHelper.Delete;
            PluginPipelineStage = (int)SdkMessageProcessingStepStage.PreOperation;
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
        /// Gets or sets the ManageDeleteEntityService service.
        /// </summary>
        /// <value>The ManageDeleteEntityService service object instance.</value>
        public IManageDeleteEntityService ManageDeleteEntityService { get; set; }

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
            using (ManageDeleteEntityService = ManageDeleteEntityService ?? new ManageDeleteEntityService(CrmService, OrganizationServiceContext, PluginExecutionContext, TracingService))
            {
                Trace(string.Format(CultureInfo.InvariantCulture, "{0} | {1} started at {2} milliseconds", UnderlyingSystemTypeName, MethodBase.GetCurrentMethod().Name, Stopwatch.ElapsedMilliseconds));
                ManageDeleteEntityService.BlockDeletion();
                Trace(string.Format(CultureInfo.InvariantCulture, "{0} | {1} ended at {2} milliseconds", UnderlyingSystemTypeName, MethodBase.GetCurrentMethod().Name, Stopwatch.ElapsedMilliseconds));

                Trace(string.Format(CultureInfo.InvariantCulture, "{0} | {1} started at {2} milliseconds", UnderlyingSystemTypeName, MethodBase.GetCurrentMethod().Name, Stopwatch.ElapsedMilliseconds));
                ManageDeleteEntityService.DisassociateDescendants();
                Trace(string.Format(CultureInfo.InvariantCulture, "{0} | {1} ended at {2} milliseconds", UnderlyingSystemTypeName, MethodBase.GetCurrentMethod().Name, Stopwatch.ElapsedMilliseconds));
            }

            Trace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.ExitingMethod, UnderlyingSystemTypeName, MethodBase.GetCurrentMethod().Name));
        }

        /// <summary>
        /// Validates the execution context.
        /// </summary>
        public override void Validate()
        {
            // Check if the plugin execution context contains target entity reference
            if (!(PluginExecutionContext.InputParameters.Contains(PlatformConstants.TargetEntityReferenceText)) ||
                !(PluginExecutionContext.InputParameters[PlatformConstants.TargetEntityReferenceText] is EntityReference))
            {
                throw new InvalidPluginExecutionException(string.Format(CultureInfo.InvariantCulture,
                    TraceMessageHelper.TargetEntityReferenceIsRequired, UnderlyingSystemTypeName));
            }

            // Check if the plugin execution context contains pre business entity
            if (!(PluginExecutionContext.PreEntityImages.Contains(PlatformConstants.PreBusinessEntityText)) ||
                !(PluginExecutionContext.PreEntityImages[PlatformConstants.PreBusinessEntityText] is Entity))
            {
                throw new InvalidPluginExecutionException(string.Format(CultureInfo.InvariantCulture,
                    TraceMessageHelper.PreEntityIsRequired, UnderlyingSystemTypeName));
            }

            // Check if the pre entity logical name is the expected one
            Entity preEntity = (Entity)PluginExecutionContext.PreEntityImages[PlatformConstants.PreBusinessEntityText];
            if (preEntity.LogicalName != PrimaryEntityLogicalName)
            {
                throw new InvalidPluginExecutionException(string.Format(CultureInfo.InvariantCulture,
                    TraceMessageHelper.WrongRegisteredEntity, UnderlyingSystemTypeName, preEntity.LogicalName));
            }

            // Check if the running message is the expected one
            if (PluginExecutionContext.MessageName != PlatformMessageHelper.Delete)
            {
                throw new InvalidPluginExecutionException(string.Format(CultureInfo.InvariantCulture,
                    TraceMessageHelper.IncorrectMessageName, UnderlyingSystemTypeName,
                    PlatformMessageHelper.Delete, PluginExecutionContext.MessageName));
            }

            // Check if the plugin current pipeline stage is the expected one
            if (PluginExecutionContext.Stage != (int)SdkMessageProcessingStepStage.PreOperation)
            {
                throw new InvalidPluginExecutionException(string.Format(CultureInfo.InvariantCulture,
                    TraceMessageHelper.IncorrectPipelineStage, UnderlyingSystemTypeName,
                    (int)SdkMessageProcessingStepStage.PreOperation, PluginExecutionContext.Stage));
            }

            // Check if the plugin depth is not exiting the max depth limit expectation
            if (PluginExecutionContext.Depth > MaximumAllowedExecutionDepth)
            {
                throw new InvalidPluginExecutionException(string.Format(CultureInfo.InvariantCulture,
                    TraceMessageHelper.MaxDepthViolation, UnderlyingSystemTypeName, MaximumAllowedExecutionDepth, PluginExecutionContext.Depth));
            }
        }

        #endregion
    }
}
