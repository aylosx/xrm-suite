﻿namespace Shared.BusinessLogic.Services.Note
{
    using Shared.BusinessLogic.Services.Data;

    using System;

    public interface IInitializeEntityService : IDisposable
    {
        /// <summary>
        /// Gets or sets the CRM service.
        /// </summary>
        /// <value>The CRM service object instance.</value>
        ICrmService CrmService { get; set; }

        /// <summary>
        /// Called by the plugin shell and initializes the entity. 
        /// </summary>
        void InitializeEntity();
    }
}
