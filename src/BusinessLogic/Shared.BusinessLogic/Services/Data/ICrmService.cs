namespace Shared.BusinessLogic.Services.Data
{
    using System;
    using System.Collections.Generic;

    using Microsoft.Xrm.Sdk;
    using Microsoft.Xrm.Sdk.Client;

    using Shared.Models.Domain;

    public interface ICrmService : IDisposable
    {
        /// <summary>
        /// Retrieves Account entity for the given primary key
        /// </summary>
        /// <param name="key">The entity's primary key</param>
        /// <returns>BusinessEntity entity</returns>
        Account GetAccountByKey(Guid key);

        /// <summary>
        /// Retrieves active accounts records for the given parent account.
        /// </summary>
        /// <param name="primaryKey">Parent account primary key</param>
        /// <returns>A list of accounts.</returns>
        IEnumerable<Account> GetAccountDescendants(Guid primaryKey);

        /// <summary>
        /// Saves the changes for the given CRM organization context.
        /// </summary>
        /// <param name="saveChangesOptions">SaveChangesOptions</param>
        /// <returns>A collection of the action results <see cref="SaveChangesResultCollection"/> class.</returns>
        SaveChangesResultCollection SaveChanges(SaveChangesOptions saveChangesOptions);
    }
}
