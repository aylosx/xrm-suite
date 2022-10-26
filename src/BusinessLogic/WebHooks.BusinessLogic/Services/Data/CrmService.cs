namespace WebHooks.BusinessLogic.Services.Data
{
    using Aylos.Xrm.Sdk.Common;

    using Microsoft.Azure.WebJobs.Logging;
    using Microsoft.Extensions.Logging;
    using Microsoft.Xrm.Sdk;
    using Microsoft.Xrm.Sdk.Client;
    using Microsoft.Xrm.Sdk.Messages;

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

        public static ILogger Logger { get; private set; }

        public string UnderlyingSystemTypeName { get { return GetType().UnderlyingSystemType.Name; } }

        #endregion

        #region Constructor

        private CrmService(ILoggerFactory loggerFactory)
        {
            if (loggerFactory == null) throw new ArgumentNullException(nameof(loggerFactory));

            Logger = loggerFactory.CreateLogger(LogCategories.CreateFunctionUserCategory(UnderlyingSystemTypeName));

            //[assembly: Microsoft.Xrm.Sdk.Client.ProxyTypesAssemblyAttribute()]
        }

        public CrmService(CrmServiceContext organizationServiceContext, ILoggerFactory loggerFactory) : this(loggerFactory)
        {
            OrganizationServiceContext = organizationServiceContext ?? throw new ArgumentNullException(nameof(organizationServiceContext));
        }

        #endregion

        #region Methods

        /// <summary>
        /// Retrieves the Note entity for the given primary key
        /// </summary>
        /// <param name="primaryKey">The entity's primary key</param>
        /// <returns>Note entity</returns>
        /// <remarks>
        /// SingleOrDefault: throws an exception if more than one record returned or null if no records returned
        /// http://www.technicaloverload.com/linq-single-vs-singleordefault-vs-first-vs-firstordefault/
        /// </remarks>
        public virtual Note GetNoteByKey(Guid primaryKey)
        {
            Logger.LogTrace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.EnteredMethod, UnderlyingSystemTypeName, MethodBase.GetCurrentMethod().Name));

            if (Guid.Empty.Equals(primaryKey)) throw new ArgumentNullException(nameof(primaryKey));

            IQueryable<Note> items =
                from x in OrganizationServiceContext.NoteSet
                where x.Id == primaryKey
                select x;

            Logger.LogTrace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.ExitingMethod, UnderlyingSystemTypeName, MethodBase.GetCurrentMethod().Name));

            return items.SingleOrDefault();
        }

        /// <summary>
        /// Saves the changes for the given organization context.
        /// </summary>
        /// <param name="saveChangesOptions">SaveChangesOptions</param>
        /// <returns>A collection of the action results <see cref="SaveChangesResultCollection"/> class.</returns>
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
