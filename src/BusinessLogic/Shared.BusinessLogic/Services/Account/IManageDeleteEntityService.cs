namespace Shared.BusinessLogic.Services.Account
{
    using System;
    using Data;

    public interface IManageDeleteEntityService : IDisposable 
    {
        /// <summary>
        /// Gets or sets the CRM service.
        /// </summary>
        /// <value>The CRM service object instance.</value>
        ICrmService CrmService { get; set; }

        /// <summary>
        /// Called by the plugin and blocks deletion if the account is marked as a preferred customer.
        /// </summary>
        void BlockDeletion();

        /// <summary>
        /// Called by the plugin and dissassociates the descendants of the entity. 
        /// </summary>
        void DisassociateDescendants();
    }
}
