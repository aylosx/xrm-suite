namespace Shared.BusinessLogic.Services.Data
{
    using Microsoft.Xrm.Sdk;
    using Microsoft.Xrm.Sdk.Client;
    using Microsoft.Xrm.Sdk.Query;

    using Shared.Models.Domain;

    using System;
    using System.Collections.Generic;

    public interface ICrmService : IDisposable
    {
        /// <summary>
        /// Deletes the entity for the given primary key and logical name.
        /// </summary>
        /// <param name="logicalName">The entity's logical name</param>
        /// <param name="primaryKey">The entity's primary key</param>
        /// <returns>BusinessEntity entity</returns>
        void DeleteEntity(string logicalName, Guid primaryKey);

        /// <summary>
        /// Retrieves the entity for the given primary key and logical name.
        /// </summary>
        /// <param name="logicalName">The entity's logical name</param>
        /// <param name="primaryKey">The entity's primary key</param>
        /// <returns>Entity entity</returns>
        Entity RetrieveEntity(string logicalName, Guid primaryKey);

        /// <summary>
        /// Retrieves the entity for the given primary key and logical name.
        /// </summary>
        /// <param name="logicalName">The entity's logical name</param>
        /// <param name="primaryKey">The entity's primary key</param>
        /// <param name="columnSet">The set of the columns to be retrieved</param>
        /// <returns>Entity entity</returns>
        Entity RetrieveEntity(string logicalName, Guid primaryKey, ColumnSet columnSet);

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
        /// Retrieves the execution context records for the given correlation and operation identifiers.
        /// </summary>
        /// <param name="correlationId">The correlation identifier</param>
        /// <param name="operationId">The operation identifier</param>
        /// <returns>A list of execution contexts.</returns>
        IEnumerable<ExecutionContext> GetExecutionContexts(string correlationId, string operationId);

        /// <summary>
        /// Saves the changes for the given CRM organization context.
        /// </summary>
        /// <param name="saveChangesOptions">SaveChangesOptions</param>
        /// <returns>A collection of the action results <see cref="SaveChangesResultCollection"/> class.</returns>
        SaveChangesResultCollection SaveChanges(SaveChangesOptions saveChangesOptions);
    }
}
