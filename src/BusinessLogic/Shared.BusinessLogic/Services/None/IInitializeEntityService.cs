namespace Shared.BusinessLogic.Services.None
{
    using System;

    using Shared.BusinessLogic.Services.Data;
    using Shared.Models.Responses.Account;

    public interface IInitializeEntityService : IDisposable
    {
        /// <summary>
        /// Gets or sets the CRM service.
        /// </summary>
        /// <value>The CRM service object instance.</value>
        ICrmService CrmService { get; set; }

        /// <summary>
        /// Main function of this module to be called within the code activity implementation.
        /// </summary>
        /// <param name="businessEntityPrimaryKey">BusinessEntity Primary Key.</param>
        /// <returns>Returns a string JSON formatted representation of the output object.</returns>
        InitializedEntity InitializeEntity(string businessEntityPrimaryKey);
    }
}
