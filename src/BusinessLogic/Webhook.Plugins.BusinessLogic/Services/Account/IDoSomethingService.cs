namespace Webhook.Plugins.BusinessLogic.Services.Account
{
    using System;
    using System.Net.Http;

    using Webhook.Plugins.BusinessLogic.Services.Data;

    public interface IDoSomethingService : IDisposable
    {
        /// <summary>
        /// Gets or sets the CRM service.
        /// </summary>
        /// <value>The CRM service object instance.</value>
        ICrmService CrmService { get; set; }

        /// <summary>
        /// Called by the webhook to do something (create/update example). 
        /// </summary>
        void DoSomething(HttpRequestMessage req);

        /// <summary>
        /// Called by the webhook to manage account deletion (deletion example). 
        /// </summary>
        void ManageDeletion(HttpRequestMessage req);

        /// <summary>
        /// Called by the webhook to manage account retrieval (retrieve example). 
        /// </summary>
        void ManageRetrieve(HttpRequestMessage req);
    }
}
