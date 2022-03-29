namespace CodeActivities.BusinessEntity
{
    using System;
    using System.Activities;
    using System.Globalization;
    using System.Reflection;

    using Microsoft.Xrm.Sdk;
    using Microsoft.Xrm.Sdk.Workflow;

    using Aylos.Xrm.Sdk.CodeActivities;
    using Aylos.Xrm.Sdk.Common;

    using Shared.BusinessLogic.Services.Data;
    using Shared.BusinessLogic.Services.None;

    using Shared.Models.Domain;
    using Shared.Models.Responses.Account;

    /// <summary>
    /// Provide information about the business purpose for the particular code activity.
    /// </summary>
    public class InitializeEntity : CustomCodeActivity
    {
        #region Static Members

        public const string BusinessEntityPrimaryKeyText = "BusinessEntityPrimaryKey";
        public const string BusinessEntityPrimaryKeyIsRequiredMessage = "The primary key is required.";
        public const string BusinessEntityPrimaryKeyIsInvalidMessage = "The given primary key is invalid.";
        public const string InitializedEntityText = "InitializedEntity";

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
        /// Gets or sets the copy someEntity service.
        /// </summary>
        /// <value>The copy someEntity service object instance.</value>
        public IInitializeEntityService InitializeEntityService { get; set; }

        #endregion

        #region Override Base Custom Code Activity Methods

        public override void Execute()
        {
            Trace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.EnteredMethod, SystemTypeName, MethodBase.GetCurrentMethod().Name));

            using (OrganizationServiceContext = OrganizationServiceContext ?? new CrmServiceContext(CurrentUserService))
            using (CrmService = CrmService ?? new CrmService(OrganizationServiceContext, TracingService))
            using (InitializeEntityService = InitializeEntityService  ?? new InitializeEntityService(CrmService, OrganizationServiceContext, WorkflowContext, TracingService))
            {
                Trace(string.Format(CultureInfo.InvariantCulture, "{0} : {1} started at {2} milliseconds", SystemTypeName, MethodBase.GetCurrentMethod().Name, Stopwatch.ElapsedMilliseconds));

                // Call the business logic
                InitializedEntity output = InitializeEntityService.InitializeEntity(
                    BusinessEntityPrimaryKey.Get<string>(CodeActivityContext)
                    );

                // Serialize the output in JSON data format
                string json = SerializationHelper.SerializeJson(output);

                // Return the JSON data to the caller
                InitializedEntity.Set(CodeActivityContext, json);

                Trace(string.Format(CultureInfo.InvariantCulture, "{0} : {1} ended at {2} milliseconds", SystemTypeName, MethodBase.GetCurrentMethod().Name, Stopwatch.ElapsedMilliseconds));
            }

            Trace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.ExitingMethod, SystemTypeName, MethodBase.GetCurrentMethod().Name));
        }

        public override void Validate()
        {
            Trace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.EnteredMethod, SystemTypeName, MethodBase.GetCurrentMethod().Name));

            // Exceptions will be handled by the caller
            ThrowsException = false;

            string someEntityPrimaryKey = BusinessEntityPrimaryKey.Get<string>(CodeActivityContext);
            if (string.IsNullOrWhiteSpace(someEntityPrimaryKey)) throw new InvalidPluginExecutionException(BusinessEntityPrimaryKeyIsRequiredMessage);

            Guid key; bool isValidGuid = Guid.TryParse(someEntityPrimaryKey, out key);
            if (!isValidGuid) throw new InvalidPluginExecutionException(BusinessEntityPrimaryKeyIsInvalidMessage);

            Trace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.ExitingMethod, SystemTypeName, MethodBase.GetCurrentMethod().Name));
        }

        #endregion

        #region Workflow Input Arguments

        [Input("BusinessEntityPrimaryKey")]
        [RequiredArgument]
        public InArgument<string> BusinessEntityPrimaryKey { get; set; }

        #endregion

        #region Workflow Output Arguments

        [Output("InitializedEntity")]
        public OutArgument<string> InitializedEntity { get; set; }

        #endregion
    }
}
