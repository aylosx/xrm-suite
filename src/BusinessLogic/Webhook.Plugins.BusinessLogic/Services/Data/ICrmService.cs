namespace Webhook.Plugins.BusinessLogic.Services.Data
{
    using Microsoft.PowerPlatform.Dataverse.Client;
    using Microsoft.Xrm.Sdk;
    using Microsoft.Xrm.Sdk.Client;

    using Shared.Models.Domain;

    using System;

    public interface ICrmService : IDisposable
    {
        public CrmServiceContext OrganizationServiceContext { get; set; }

        ServiceClient ServiceClient { get; set; }

        Note GetNoteByKey(Guid primaryKey, Guid callerId);

        SaveChangesResultCollection SaveChanges(SaveChangesOptions saveChangesOptions);
    }
}
