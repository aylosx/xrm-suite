namespace Shared.BusinessLogic.Services.ExecutionContext
{
    using System;

    using Shared.BusinessLogic.Services.Data;

    public interface IManageExecutionContext : IDisposable
    {
        /// <summary>
        /// Gets or sets the CRM service.
        /// </summary>
        /// <value>The CRM service object instance.</value>
        ICrmService CrmService { get; set; }

        /// <summary>
        /// Creates a new record in the ExecutionContext entity and populates all the needed fields 
        /// including a serialized version of the PluginExecutioncontext.
        /// </summary>
        /// <returns>void()</returns>
        void StoreExecutionContext();
    }
}
