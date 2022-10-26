namespace WebHooks.BusinessLogic.Services.Data
{
    using System;
    using System.Collections.Generic;

    using Microsoft.Xrm.Sdk;
    using Microsoft.Xrm.Sdk.Client;

    using Shared.Models.Domain;

    public interface ICrmService : IDisposable
    {
        public CrmServiceContext OrganizationServiceContext { get; set; }

        /// <summary>
        /// Retrieves the Note entity for the given primary key
        /// </summary>
        /// <param name="primaryKey">The entity's primary key</param>
        /// <returns>Note entity</returns>
        Note GetNoteByKey(Guid primaryKey);

        /// <summary>
        /// Saves the changes for the given organization service context.
        /// </summary>
        /// <param name="saveChangesOptions">SaveChangesOptions</param>
        /// <returns>A collection of the action results <see cref="SaveChangesResultCollection"/> class.</returns>
        SaveChangesResultCollection SaveChanges(SaveChangesOptions saveChangesOptions);
    }
}
