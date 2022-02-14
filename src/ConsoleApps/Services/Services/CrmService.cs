namespace Aylos.Xrm.Sdk.ConsoleApps.Services
{
    using Microsoft.Xrm.Sdk;
    using Microsoft.Xrm.Sdk.Client;
    using Microsoft.Xrm.Sdk.Messages;

    using NLog;

    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;

    using Aylos.Xrm.Sdk.Common;
    using Aylos.Xrm.Sdk.ConsoleApps;

    using Shared.Models.Domain;

    public class CrmService : ConsoleService<CrmServiceContext>, ICrmService
    {
        bool disposedValue;

        string SystemTypeName { get { return GetType().UnderlyingSystemType.Name; } }

        public CrmService(CrmServiceContext organizationServiceContext, Connection connection) : base(organizationServiceContext, connection)
        {
            Logger.Trace(CultureInfo.InvariantCulture, TraceMessageHelper.EnteredMethod, SystemTypeName, MethodBase.GetCurrentMethod().Name);

            Logger.Trace(CultureInfo.InvariantCulture, TraceMessageHelper.ExitingMethod, SystemTypeName, MethodBase.GetCurrentMethod().Name);
        }

        public virtual void DeleteProcessContexts(IList<Guid> primaryKeys)
        {
            Logger.Trace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.EnteredMethod, SystemTypeName, MethodBase.GetCurrentMethod().Name));

            if (primaryKeys == null) throw new ArgumentNullException(nameof(primaryKeys));

            ExecuteMultipleRequest req = new ExecuteMultipleRequest()
            {
                Settings = new ExecuteMultipleSettings()
                {
                    ContinueOnError = false,
                    ReturnResponses = false
                },
                Requests = new OrganizationRequestCollection()
            };

            foreach (Guid primaryKey in primaryKeys)
            {
                req.Requests.Add(new DeleteRequest { 
                    Target = new EntityReference { Id = primaryKey, LogicalName = ExecutionContext.EntityLogicalName } 
                });
            }

            OrganizationService.Execute(req);

            Logger.Trace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.ExitingMethod, SystemTypeName, MethodBase.GetCurrentMethod().Name));
        }

        public virtual IEnumerable<ExecutionContext> GetActiveProcessContexts()
        {
            Logger.Trace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.EnteredMethod, SystemTypeName, MethodBase.GetCurrentMethod().Name));

            IQueryable<ExecutionContext> entities =
                from x in OrganizationServiceContext.ExecutionContextSet
                where x.State == ExecutionContextState.Active
                orderby x.Subject ascending
                select x; // TO-DO: Optimisation

            Logger.Trace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.ExitingMethod, SystemTypeName, MethodBase.GetCurrentMethod().Name));

            return entities;
        }

        public virtual SaveChangesResultCollection SaveChanges(OrganizationServiceContext organizationServiceContext, SaveChangesOptions saveChangesOptions)
        {
            Logger.Trace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.EnteredMethod, SystemTypeName, MethodBase.GetCurrentMethod().Name));

            if (organizationServiceContext == null) throw new ArgumentNullException(nameof(organizationServiceContext));

            Logger.Trace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.ExitingMethod, SystemTypeName, MethodBase.GetCurrentMethod().Name));

            return organizationServiceContext.SaveChanges(saveChangesOptions);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    
                }
                disposedValue = true;
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
    }
}
