namespace Shared.BusinessLogic.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;

    using Microsoft.Xrm.Sdk;
    using Microsoft.Xrm.Sdk.Client;

    using Aylos.Xrm.Sdk.Common;

    using Shared.Models.Domain;
    using Microsoft.Xrm.Sdk.Messages;

    /// <summary>
    /// This class service provides access to the CRM database.
    /// </summary>
    /// <remarks>
    /// Use the OrganizationServiceContext class https://msdn.microsoft.com/en-us/library/gg334504.aspx
    /// </remarks>
    public class CrmService : ICrmService
    {
        #region Members

        private string SystemTypeName { get { return GetType().UnderlyingSystemType.Name; } }

        private CrmServiceContext OrganizationServiceContext { get; set; }

        private ITracingService TracingService { get; set; }

        /// <summary>
        /// Traces the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        internal void Trace(string message)
        {
            if (TracingService == null || string.IsNullOrWhiteSpace(message)) return;
            TracingService.Trace(message);
        }

        #endregion

        #region Constructor

        /// <summary>
        /// CrmService Constructor
        /// </summary>
        /// <param name="tracingService">ITracingService</param>
        public CrmService(ITracingService tracingService)
        {
            TracingService = tracingService;
        }

        /// <summary>
        /// CrmService Constructor
        /// </summary>
        /// <param name="organizationServiceContext">CrmServiceContext</param>
        /// <param name="tracingService">ITracingService</param>
        public CrmService(CrmServiceContext organizationServiceContext, ITracingService tracingService)
            : this(tracingService)
        {
            OrganizationServiceContext = organizationServiceContext;
        }

        #endregion

        #region Methods

        /**
         * <summary>    Deletes the entity. </summary>
         *
         * <remarks>    Vangelis Xanthakis, 07/02/2022. </remarks>
         *
         * <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
         *
         * <param name="logicalName">   Name of the logical. </param>
         * <param name="primaryKey">    The primary key. </param>
         */

        public virtual void DeleteEntity(string logicalName, Guid primaryKey)
        {
            Trace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.EnteredMethod, SystemTypeName, MethodBase.GetCurrentMethod().Name));

            if (string.IsNullOrWhiteSpace(logicalName)) throw new ArgumentNullException(nameof(logicalName));
            if (Guid.Empty.Equals(primaryKey)) throw new ArgumentNullException(nameof(primaryKey));

            DeleteRequest req = new DeleteRequest { Target = new EntityReference (logicalName, primaryKey) };

            OrganizationServiceContext.Execute(req);

            Trace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.ExitingMethod, SystemTypeName, MethodBase.GetCurrentMethod().Name));
        }

        /// <summary>
        /// Retrieves Account entity for the given primary key
        /// </summary>
        /// <param name="primaryKey">The entity's primary key</param>
        /// <returns>BusinessEntity entity</returns>
        /// <remarks>
        /// SingleOrDefault: throws an exception if more than one record returned or null if no records returned
        /// http://www.technicaloverload.com/linq-single-vs-singleordefault-vs-first-vs-firstordefault/
        /// </remarks>
        public virtual Account GetAccountByKey(Guid primaryKey)
        {
            Trace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.EnteredMethod, SystemTypeName, MethodBase.GetCurrentMethod().Name));

            if (Guid.Empty.Equals(primaryKey)) throw new ArgumentNullException(nameof(primaryKey));

            IQueryable<Account> items =
                from x in OrganizationServiceContext.AccountSet
                where x.Id == primaryKey
                select new Account
                {
                    Id = x.Id,
                    AccountNumber = x.AccountNumber,
                    AccountName = x.AccountName,
                };

            Trace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.ExitingMethod, SystemTypeName, MethodBase.GetCurrentMethod().Name));

            return items.SingleOrDefault();
        }

        /// <summary>
        /// Retrieves active accounts records for the given parent account.
        /// </summary>
        /// <param name="primaryKey">Parent account primary key</param>
        /// <returns>A list of accounts.</returns>
        /// <remarks>
        /// So the difference between IQueryable and IEnumerable is about where the filter logic is executed. 
        /// One executes on the client side and the other executes on the database.
        /// So if you working with only in-memory data collection IEnumerable is a good choice but if you want 
        /// to query data collection which is connected with database IQueryable is a better choice as it reduces 
        /// network traffic and uses the power of SQL language.
        /// https://stackoverflow.com/questions/252785/what-is-the-difference-between-iqueryablet-and-ienumerablet
        /// </remarks>
        public virtual IEnumerable<Account> GetAccountDescendants(Guid primaryKey)
        {
            Trace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.EnteredMethod, SystemTypeName, MethodBase.GetCurrentMethod().Name));

            if (Guid.Empty.Equals(primaryKey)) throw new ArgumentNullException(nameof(primaryKey));

            IQueryable<Account> entities =
                from x in OrganizationServiceContext.AccountSet
                where x.Id == primaryKey && x.State == AccountState.Active
                select x;

            Trace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.ExitingMethod, SystemTypeName, MethodBase.GetCurrentMethod().Name));

            return entities;
        }

        /// <summary>
        /// Retrieves the execution context records for the given correlation and operation identifiers.
        /// </summary>
        /// <param name="correlationId">The correlation identifier</param>
        /// <param name="operationId">The operation identifier</param>
        /// <returns>A list of execution contexts.</returns>
        public virtual IEnumerable<ExecutionContext> GetExecutionContexts(string correlationId, string operationId)
        {
            Trace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.EnteredMethod, SystemTypeName, MethodBase.GetCurrentMethod().Name));

            if (string.IsNullOrWhiteSpace(correlationId)) throw new ArgumentNullException(nameof(correlationId));
            if (string.IsNullOrWhiteSpace(operationId)) throw new ArgumentNullException(nameof(operationId));

            IQueryable<ExecutionContext> entities =
                from x in OrganizationServiceContext.ExecutionContextSet
                where x.CorrelationId == correlationId && x.OperationId == operationId
                select x;

            Trace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.ExitingMethod, SystemTypeName, MethodBase.GetCurrentMethod().Name));

            return entities;
        }

        /// <summary>
        /// Saves the changes for the given CRM organization context.
        /// </summary>
        /// <param name="saveChangesOptions">SaveChangesOptions</param>
        /// <returns>A collection of the action results <see cref="SaveChangesResultCollection"/> class.</returns>
        public virtual SaveChangesResultCollection SaveChanges(SaveChangesOptions saveChangesOptions)
        {
            Trace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.EnteredMethod, SystemTypeName, MethodBase.GetCurrentMethod().Name));

            SaveChangesResultCollection scrc = OrganizationServiceContext.SaveChanges(saveChangesOptions);

            Trace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.ExitingMethod, SystemTypeName, MethodBase.GetCurrentMethod().Name));

            return scrc;
        }

        #endregion

        #region IDisposable Support

        private bool _disposed;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    
                }

                // TODO: free unmanaged resources (unmanaged objects).
                

                _disposed = true;
            }
        }

        ~CrmService()
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
