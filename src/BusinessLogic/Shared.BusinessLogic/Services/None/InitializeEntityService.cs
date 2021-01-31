namespace Shared.BusinessLogic.Services.None
{
    using System;
    using System.Globalization;
    using System.Reflection;
    using System.Text;
    using Microsoft.Xrm.Sdk;
    using Microsoft.Xrm.Sdk.Client;
    using Microsoft.Xrm.Sdk.Workflow;

    using Aylos.Xrm.Sdk.Common;

    using Shared.BusinessLogic.Services.Data;
    using Shared.Models.Domain;
    using Shared.Models.Responses.Account;

    /// <summary>
    /// The main purpose of this module is to initialize the entity 
    /// and return the new data to the caller.
    /// </summary>
    public class InitializeEntityService : GenericService<CrmServiceContext>, IInitializeEntityService
    {
        #region Static Members 

        public static readonly DateTime DateTimeNow = DateTime.UtcNow;
        public const string TextFormat = "Code-{0}";
        public const string DateTimeFormat = "yyyyMMddHHmmss";
        public const string AccountNotExistMessage = "Something went wrong, I could not find an account with the given key.";
        public const string AccountNumberAlreadySetMessage = "The account number already set, you can only set the account number once.";

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the CRM service.
        /// </summary>
        /// <value>The CRM service object instance.</value>
        public ICrmService CrmService { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// InitializeEntityService Constructor
        /// </summary>
        /// <param name="tracingService">ITracingService</param>
        public InitializeEntityService(ITracingService tracingService)
        {
            TracingService = tracingService;
        }

        /// <summary>
        /// InitializeEntityService Constructor
        /// </summary>
        /// <param name="workflowContext">IWorkflowContext</param>
        /// <param name="tracingService">ITracingService</param>
        public InitializeEntityService(IWorkflowContext workflowContext, ITracingService tracingService) 
            : this(tracingService)
        {
            WorkflowContext = workflowContext;
        }

        /// <summary>
        /// InitializeEntityService Constructor
        /// </summary>
        /// <param name="crmService">ICrmService</param>
        /// <param name="workflowContext">IWorkflowContext</param>
        /// <param name="tracingService">ITracingService</param>
        public InitializeEntityService(ICrmService crmService, IWorkflowContext workflowContext, ITracingService tracingService)
            : this(workflowContext, tracingService)
        {
            CrmService = crmService;
        }

        /// <summary>
        /// InitializeEntityService Constructor
        /// </summary>
        /// <param name="crmService">ICrmService</param>
        /// <param name="organizationServiceContext">CrmServiceContext</param>
        /// <param name="workflowContext">IWorkflowContext</param>
        /// <param name="tracingService">ITracingService</param>
        public InitializeEntityService(ICrmService crmService, CrmServiceContext organizationServiceContext, IWorkflowContext workflowContext, ITracingService tracingService)
            : this(crmService, workflowContext, tracingService)
        {
            OrganizationServiceContext = organizationServiceContext;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Generates the account number and saves the result to the entity.
        /// </summary>
        /// <param name="businessEntityPrimaryKey">BusinessEntity Primary Key.</param>
        /// <returns>Returns a string JSON formatted representation of the output object.</returns>
        public InitializedEntity InitializeEntity(string businessEntityPrimaryKey)
        {
            Trace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.EnteredMethod, UnderlyingSystemTypeName, MethodBase.GetCurrentMethod().Name));

            if (string.IsNullOrWhiteSpace(businessEntityPrimaryKey)) throw new ArgumentNullException(nameof(businessEntityPrimaryKey));

            Account account = CrmService.GetAccountByKey(new Guid(businessEntityPrimaryKey));

            if (account == null)
            {
                throw new InvalidPluginExecutionException(AccountNotExistMessage);
            }

            if (!string.IsNullOrWhiteSpace(account.AccountNumber))
            {
                throw new InvalidPluginExecutionException(AccountNumberAlreadySetMessage);
            }

            account.AccountNumber = string.Format(CultureInfo.InvariantCulture, TextFormat, DateTimeNow.ToString(DateTimeFormat, CultureInfo.InvariantCulture));

            // Save changes in a single call to include all in an atomic transaction
            Trace(string.Format(CultureInfo.InvariantCulture, "{0}: Saving the entity in CRM. at {1}", UnderlyingSystemTypeName, MethodBase.GetCurrentMethod().Name));
            CrmService.SaveChanges(SaveChangesOptions.None);

            // Create the output
            InitializedEntity output = new InitializedEntity(account);

            Trace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.ExitingMethod, UnderlyingSystemTypeName, MethodBase.GetCurrentMethod().Name));

            return output;
        }

        #endregion

        #region Private Methods

        #endregion

        #region IDisposable Support

        private bool _disposed = false; 

        /// <summary>
        /// Consider disposing any unmanaged resources within the dispose method
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;

            if (disposing)
            {
                
            }

            // TODO: free unmanaged resources (unmanaged objects).

            _disposed = true;
        }

        ~InitializeEntityService()
        {
            Dispose(false);
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
