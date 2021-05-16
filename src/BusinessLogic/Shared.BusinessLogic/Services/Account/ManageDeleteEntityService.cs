namespace Shared.BusinessLogic.Services.Account
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;

    using Microsoft.Xrm.Sdk;
    using Microsoft.Xrm.Sdk.Client;

    using Aylos.Xrm.Sdk.Common;

    using Models.Domain;
    using Data;

    public class ManageDeleteEntityService : GenericService<CrmServiceContext, Account>, IManageDeleteEntityService
    {
        #region Static Members 

        public static readonly DateTime DateTimeNow = DateTime.UtcNow;
        public const string BusinessEntityCannotBeDeletedErrorMessage = "The account cannot be deleted because it is a preferred customer."; 

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
        /// ManageDeleteEntityService Constructor
        /// </summary>
        /// <param name="tracingService">ITracingService</param>
        public ManageDeleteEntityService(ITracingService tracingService)
        {
            TracingService = tracingService;
        }

        /// <summary>
        /// ManageDeleteEntityService Constructor
        /// </summary>
        /// <param name="pluginExecutionContext">IPluginExecutionContext</param>
        /// <param name="tracingService">ITracingService</param>
        public ManageDeleteEntityService(IPluginExecutionContext pluginExecutionContext, ITracingService tracingService)
            : this(tracingService)
        {
            PluginExecutionContext = pluginExecutionContext;
        }

        /// <summary>
        /// ManageDeleteEntityService Constructor
        /// </summary>
        /// <param name="crmService">ICrmService</param>
        /// <param name="pluginExecutionContext">IPluginExecutionContext</param>
        /// <param name="tracingService">ITracingService</param>
        public ManageDeleteEntityService(ICrmService crmService, IPluginExecutionContext pluginExecutionContext, ITracingService tracingService)
            : this(pluginExecutionContext, tracingService)
        {
            CrmService = crmService;
        }

        /// <summary>
        /// ManageDeleteEntityService Constructor
        /// </summary>
        /// <param name="crmService">ICrmService</param>
        /// <param name="organizationServiceContext">OrganizationServiceContext</param>
        /// <param name="pluginExecutionContext">IPluginExecutionContext</param>
        /// <param name="tracingService">ITracingService</param>
        public ManageDeleteEntityService(ICrmService crmService, CrmServiceContext organizationServiceContext, IPluginExecutionContext pluginExecutionContext, ITracingService tracingService)
            : this(crmService, pluginExecutionContext, tracingService)
        {
            OrganizationServiceContext = organizationServiceContext;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Called by the plugin and blocks deletion if the account is marked as a preferred customer.
        /// </summary>
        public void BlockDeletion()
        {
            Trace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.EnteredMethod, UnderlyingSystemTypeName, MethodBase.GetCurrentMethod().Name));

            if (PreBusinessEntity.Category == AccountCategory.PreferredCustomer)
            {
                throw new InvalidPluginExecutionException(BusinessEntityCannotBeDeletedErrorMessage);
            }

            Trace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.ExitingMethod, UnderlyingSystemTypeName, MethodBase.GetCurrentMethod().Name));
        }

        /// <summary>
        /// Called by the plugin and disassociates the descendants of the account. 
        /// </summary>
        public void DisassociateDescendants()
        {
            Trace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.EnteredMethod, UnderlyingSystemTypeName, MethodBase.GetCurrentMethod().Name));

            IList<Account> descendants = CrmService.GetAccountDescendants(PreBusinessEntity.Id).ToList();

            foreach (Account account in descendants)
            {
                account.ParentAccount = null;

                if (OrganizationServiceContext.IsAttached(account))
                {
                    OrganizationServiceContext.UpdateObject(account);
                }
                else
                {
                    OrganizationServiceContext.AddObject(account);
                }
            }

            if (descendants.Any())
            {
                // Save changes in a single call to include all in an atomic transaction
                Trace(string.Format(CultureInfo.InvariantCulture, "{0}: Saving the entity in CRM. at {1}", UnderlyingSystemTypeName, MethodBase.GetCurrentMethod().Name));
                CrmService.SaveChanges(SaveChangesOptions.None);
            }

            Trace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.ExitingMethod, UnderlyingSystemTypeName, MethodBase.GetCurrentMethod().Name));
        }

        #endregion

        #region Private Members

        #endregion

        #region IDisposable Support

        private bool _disposed;

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

        ~ManageDeleteEntityService()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
