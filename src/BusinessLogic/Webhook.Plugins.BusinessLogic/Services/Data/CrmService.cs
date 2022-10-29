namespace Webhook.Plugins.BusinessLogic.Services.Data
{
    using Aylos.Xrm.Sdk.Common;

    using Microsoft.Azure.WebJobs.Logging;
    using Microsoft.Extensions.Logging;
    using Microsoft.PowerPlatform.Dataverse.Client;
    using Microsoft.Xrm.Sdk;
    using Microsoft.Xrm.Sdk.Client;
    using Microsoft.Xrm.Sdk.Query;

    using Shared.Models.Domain;

    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// This class service provides access to the CRM database.
    /// </summary>
    /// <remarks>
    /// Use the OrganizationServiceContext class https://msdn.microsoft.com/en-us/library/gg334504.aspx
    /// </remarks>
    public class CrmService : ICrmService
    {
        #region Members

        public CrmServiceContext OrganizationServiceContext { get; set; }

        public ServiceClient ServiceClient { get; set; }

        public static ILogger Logger { get; private set; }

        public string UnderlyingSystemTypeName { get { return GetType().UnderlyingSystemType.Name; } }

        #endregion

        #region Constructor

        private CrmService(ILoggerFactory loggerFactory)
        {
            if (loggerFactory == null) throw new ArgumentNullException(nameof(loggerFactory));

            Logger = loggerFactory.CreateLogger(LogCategories.CreateFunctionUserCategory(UnderlyingSystemTypeName));
        }

        private CrmService(CrmServiceContext organizationServiceContext, ILoggerFactory loggerFactory) : this(loggerFactory)
        {
            OrganizationServiceContext = organizationServiceContext ?? throw new ArgumentNullException(nameof(organizationServiceContext));
        }

        public CrmService(ServiceClient serviceClient, CrmServiceContext organizationServiceContext, ILoggerFactory loggerFactory) : this(organizationServiceContext, loggerFactory)
        {
            ServiceClient = serviceClient ?? throw new ArgumentNullException(nameof(serviceClient));
        }

        #endregion

        #region Methods

        public virtual Note GetNoteByKey(Guid primaryKey, Guid callerId)
        {
            Logger.LogTrace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.EnteredMethod, UnderlyingSystemTypeName, MethodBase.GetCurrentMethod().Name));

            if (Guid.Empty.Equals(primaryKey)) throw new ArgumentNullException(nameof(primaryKey));

            ServiceClient.CallerId = callerId;

            var columnSet = new ColumnSet { AllColumns = true };

            Note annotation = ServiceClient
                .Retrieve(Note.EntityLogicalName, primaryKey, columnSet)?
                .ToEntity<Note>();

            Logger.LogTrace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.ExitingMethod, UnderlyingSystemTypeName, MethodBase.GetCurrentMethod().Name));

            return annotation;
        }

        public virtual SaveChangesResultCollection SaveChanges(SaveChangesOptions saveChangesOptions)
        {
            Logger.LogTrace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.EnteredMethod, UnderlyingSystemTypeName, MethodBase.GetCurrentMethod().Name));

            SaveChangesResultCollection scrc = OrganizationServiceContext.SaveChanges(saveChangesOptions);

            Logger.LogTrace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.ExitingMethod, UnderlyingSystemTypeName, MethodBase.GetCurrentMethod().Name));

            return scrc;
        }

        #endregion

        #region IDisposable Support

        private bool _disposed;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                }
                // TODO: free unmanaged resources (unmanaged objects).
                _disposed = true;
            }
        }

        ~CrmService()
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
