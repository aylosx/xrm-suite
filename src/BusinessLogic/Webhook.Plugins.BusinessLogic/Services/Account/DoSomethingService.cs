﻿namespace Webhook.Plugins.BusinessLogic.Services.Account
{
    using Aylos.Xrm.Sdk.Common;
    using Aylos.Xrm.Sdk.Core.WebhookPlugins;

    using Microsoft.Extensions.Logging;
    using Microsoft.PowerPlatform.Dataverse.Client;
    using Microsoft.Xrm.Sdk;
    using Microsoft.Xrm.Sdk.Client;

    using Shared.Models.Domain;

    using System;
    using System.Globalization;
    using System.Net.Http;
    using System.Reflection;

    using Webhook.Plugins.BusinessLogic.Services.Data;

    public class DoSomethingService : DataverseService<Account>, IDoSomethingService
    {
        #region Static Members 

        public const string TextFormat = "Code-{0}";
        public const string DateTimeFormat = "yyyyMMddHHmmss";
        public static readonly DateTime DateTimeNow = DateTime.UtcNow;

        public const string AccountNotExistMessage = "Something went wrong, I could not find an account with the given key.";
        public const string AccountNumberAlreadySetMessage = "The account number already set, you can only set the account number once.";

        #endregion

        #region Properties

        public ICrmService CrmService { get; set; }

        #endregion

        #region Constructor

        public DoSomethingService(ICrmService crmService, ServiceClient serviceClient, RemoteExecutionContext remoteExecutionContext, ILoggerFactory loggerFactory) 
            : base(serviceClient, remoteExecutionContext, loggerFactory)
        {
            CrmService = crmService ?? throw new ArgumentNullException(nameof(crmService));
        }

        #endregion

        #region Methods

        /// <summary>
        /// Called by the webhook to do something (create/update example). 
        /// </summary>
        public void DoSomething(HttpRequestMessage req)
        {
            Logger.LogTrace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.EnteredMethod, UnderlyingSystemTypeName, MethodBase.GetCurrentMethod().Name));

            HttpRequestMessage = req ?? throw new ArgumentNullException(nameof(req));

            switch (RemoteExecutionContext.MessageName)
            {
                case PlatformMessageHelper.Create:
                case PlatformMessageHelper.Update:
                    break;

                default: throw new InvalidOperationException(HttpMessageEventName);
            }

            Logger.LogTrace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.ExitingMethod, UnderlyingSystemTypeName, MethodBase.GetCurrentMethod().Name));
        }

        /// <summary>
        /// Called by the webhook to do manage deletion (deletion example). 
        /// </summary>
        public void ManageDeletion(HttpRequestMessage req)
        {
            Logger.LogTrace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.EnteredMethod, UnderlyingSystemTypeName, MethodBase.GetCurrentMethod().Name));

            HttpRequestMessage = req ?? throw new ArgumentNullException(nameof(req));

            switch (RemoteExecutionContext.MessageName)
            {
                case PlatformMessageHelper.Delete:
                    break;

                default: throw new InvalidOperationException(HttpMessageEventName);
            }

            Logger.LogTrace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.ExitingMethod, UnderlyingSystemTypeName, MethodBase.GetCurrentMethod().Name));
        }

        /// <summary>
        /// Called by the webhook to do manage deletion (deletion example). 
        /// </summary>
        public void ManageRetrieve(HttpRequestMessage req)
        {
            Logger.LogTrace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.EnteredMethod, UnderlyingSystemTypeName, MethodBase.GetCurrentMethod().Name));

            HttpRequestMessage = req ?? throw new ArgumentNullException(nameof(req));

            switch (RemoteExecutionContext.MessageName)
            {
                case PlatformMessageHelper.Retrieve:
                case PlatformMessageHelper.RetrieveMultiple:
                    break;

                default: throw new InvalidOperationException(HttpMessageEventName);
            }

            Logger.LogTrace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.ExitingMethod, UnderlyingSystemTypeName, MethodBase.GetCurrentMethod().Name));
        }

        #endregion

        #region Private Members

        #endregion

        #region IDisposable Support

        private bool _disposed;

        /// <summary>
        /// Consider disposing any unmanaged resources within the dispose method
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;

            if (disposing)
            {
                
            }

            // TODO: free unmanaged resources (unmanaged objects).

            _disposed = true;
        }

        ~DoSomethingService()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
