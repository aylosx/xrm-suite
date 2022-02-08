namespace Shared.BusinessLogic.Services.ExecutionContext
{
    using Shared.BusinessLogic.Services.Data;

    using System;

    public interface IPostMessageService : IDisposable
    {
        /// <summary>
        /// Gets or sets the CRM service.
        /// </summary>
        /// <value>The CRM service object instance.</value>
        ICrmService CrmService { get; set; }

        /// <summary>
        /// Post the messages to the service bus queue and deletes 
        /// the records from the platform entity.
        /// </summary>
        void PostMessage();
    }
}
