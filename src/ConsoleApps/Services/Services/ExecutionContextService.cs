namespace Aylos.Xrm.Sdk.ConsoleApps.Services
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Reflection;

    using Aylos.Xrm.Sdk.Common;
    using Aylos.Xrm.Sdk.ConsoleApps;

    using Shared.Models.Domain;

    public sealed class ExecutionContextService : ConsoleService<CrmServiceContext>, IExecutionContextService
    {
        bool disposedValue;

        string SystemTypeName { get { return GetType().UnderlyingSystemType.Name; } }

        public ExecutionContextService(CrmServiceContext organizationServiceContext, Connection connection, ICrmService crmService) : base(organizationServiceContext, connection)
        {
            Logger.Trace(CultureInfo.InvariantCulture, TraceMessageHelper.EnteredMethod, SystemTypeName, MethodBase.GetCurrentMethod().Name);

            CrmService = crmService ?? throw new ArgumentNullException(nameof(crmService));

            Logger.Trace(CultureInfo.InvariantCulture, TraceMessageHelper.ExitingMethod, SystemTypeName, MethodBase.GetCurrentMethod().Name);
        }

        ICrmService CrmService { get; set; }

        public void ProcessExecutionContexts()
        {
            Logger.Trace(CultureInfo.InvariantCulture, TraceMessageHelper.EnteredMethod, SystemTypeName, MethodBase.GetCurrentMethod().Name);

            IList<Guid> primaryKeys = new List<Guid>();
            IEnumerable<ExecutionContext> executionContexts = CrmService.GetActiveProcessContexts();

            foreach (ExecutionContext ec in executionContexts)
            {
                // submit message to the queue
                Logger.Info(CultureInfo.InvariantCulture, "{0} | {2} {3} {4} {5} | {1}.", SystemTypeName, MethodBase.GetCurrentMethod().Name, ec.EntityName, ec.MessageName, ec.Depth, ec.CorrelationId);
                primaryKeys.Add(ec.Id);
            }

            if (primaryKeys.Count > 0) CrmService.DeleteProcessContexts(primaryKeys);

            Logger.Trace(CultureInfo.InvariantCulture, TraceMessageHelper.ExitingMethod, SystemTypeName, MethodBase.GetCurrentMethod().Name);
        }

        void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (CrmService != null) CrmService.Dispose();
                    if (OrganizationServiceContext != null) OrganizationServiceContext.Dispose();
                }
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}