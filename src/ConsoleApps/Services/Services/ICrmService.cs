namespace Aylos.Xrm.Sdk.ConsoleApps.Services
{
    using Microsoft.Crm.Sdk.Messages;
    using Microsoft.Xrm.Sdk;
    using Microsoft.Xrm.Sdk.Client;
    using Microsoft.Xrm.Sdk.Messages;

    using System;
    using System.Collections.Generic;

    using Shared.Models.Domain;

    public interface ICrmService : IDisposable
    {
        void DeleteProcessContexts(IList<Guid> primaryKeys);

        IEnumerable<ExecutionContext> GetActiveProcessContexts();

        SaveChangesResultCollection SaveChanges(OrganizationServiceContext organizationServiceContext, SaveChangesOptions saveChangesOptions);
    }
}
