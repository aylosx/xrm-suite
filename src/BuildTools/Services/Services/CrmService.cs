namespace Aylos.Xrm.Sdk.BuildTools.Services
{
    using Aylos.Xrm.Sdk.BuildTools.Models.Domain;
    using Aylos.Xrm.Sdk.Common;
    using Aylos.Xrm.Sdk.ConsoleApps;
    using Microsoft.Crm.Sdk.Messages;
    using Microsoft.Xrm.Sdk;
    using Microsoft.Xrm.Sdk.Client;
    using Microsoft.Xrm.Sdk.Messages;
    using NLog;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;

    public class CrmService : ConsoleService<CrmServiceContext>, ICrmService
    {
        bool disposedValue;

        string SystemTypeName { get { return GetType().UnderlyingSystemType.Name; } }

        public CrmService(CrmServiceContext organizationServiceContext, Connection connection) : base(organizationServiceContext, connection)
        {
            Logger.Trace(CultureInfo.InvariantCulture, TraceMessageHelper.EnteredMethod, SystemTypeName, MethodBase.GetCurrentMethod().Name);

            Logger.Trace(CultureInfo.InvariantCulture, TraceMessageHelper.ExitingMethod, SystemTypeName, MethodBase.GetCurrentMethod().Name);
        }

        public virtual IList<Note> GetAnnotations(Guid id)
        {
            Logger.Trace(CultureInfo.InvariantCulture, TraceMessageHelper.EnteredMethod, SystemTypeName, MethodBase.GetCurrentMethod().Name);

            if (Guid.Empty.Equals(id)) throw new ArgumentNullException(nameof(id));

            IQueryable<Note> items =
                from x in OrganizationServiceContext.NoteSet
                where x.Regarding.Id == id
                select x;

            Logger.Trace(CultureInfo.InvariantCulture, TraceMessageHelper.ExitingMethod, SystemTypeName, MethodBase.GetCurrentMethod().Name);

            return items.ToList();
        }

        public virtual IList<Solution> GetSolutionsEndsWith(string wildcard)
        {
            Logger.Trace(CultureInfo.InvariantCulture, TraceMessageHelper.EnteredMethod, SystemTypeName, MethodBase.GetCurrentMethod().Name);

            if (string.IsNullOrWhiteSpace(wildcard)) throw new ArgumentNullException(nameof(wildcard));

            IQueryable<Solution> items =
                from x in OrganizationServiceContext.SolutionSet
                where x.Name.EndsWith(wildcard)
                select x;

            Logger.Trace(CultureInfo.InvariantCulture, TraceMessageHelper.ExitingMethod, SystemTypeName, MethodBase.GetCurrentMethod().Name);

            return items.ToList();
        }

        public virtual IList<Solution> GetSolutionsStartsWith(string wildcard)
        {
            Logger.Trace(CultureInfo.InvariantCulture, TraceMessageHelper.EnteredMethod, SystemTypeName, MethodBase.GetCurrentMethod().Name);

            if (string.IsNullOrWhiteSpace(wildcard)) throw new ArgumentNullException(nameof(wildcard));

            IQueryable<Solution> items =
                from x in OrganizationServiceContext.SolutionSet
                where x.Name.StartsWith(wildcard)
                select x;

            Logger.Trace(CultureInfo.InvariantCulture, TraceMessageHelper.ExitingMethod, SystemTypeName, MethodBase.GetCurrentMethod().Name);

            return items.ToList();
        }

        public virtual IList<Solution> GetSolutions(string uniqueName)
        {
            Logger.Trace(CultureInfo.InvariantCulture, TraceMessageHelper.EnteredMethod, SystemTypeName, MethodBase.GetCurrentMethod().Name);

            if (string.IsNullOrWhiteSpace(uniqueName)) throw new ArgumentNullException(nameof(uniqueName));

            IQueryable<Solution> items =
                from x in OrganizationServiceContext.SolutionSet
                where x.Name == uniqueName
                select x;

            Logger.Trace(CultureInfo.InvariantCulture, TraceMessageHelper.ExitingMethod, SystemTypeName, MethodBase.GetCurrentMethod().Name);

            return items.ToList();
        }

        public virtual IList<WebFile> GetWebsiteFiles(Guid id)
        {
            Logger.Trace(CultureInfo.InvariantCulture, TraceMessageHelper.EnteredMethod, SystemTypeName, MethodBase.GetCurrentMethod().Name);

            if (Guid.Empty.Equals(id)) throw new ArgumentNullException(nameof(id));

            IQueryable<WebFile> items =
                from x in OrganizationServiceContext.WebFileSet
                where x.Website.Id == id
                select x;

            Logger.Trace(CultureInfo.InvariantCulture, TraceMessageHelper.ExitingMethod, SystemTypeName, MethodBase.GetCurrentMethod().Name);

            return items.ToList();
        }

        public virtual IList<WebTemplate> GetWebTemplates(Guid id)
        {
            Logger.Trace(CultureInfo.InvariantCulture, TraceMessageHelper.EnteredMethod, SystemTypeName, MethodBase.GetCurrentMethod().Name);

            if (Guid.Empty.Equals(id)) throw new ArgumentNullException(nameof(id));

            IQueryable<WebTemplate> items =
                from x in OrganizationServiceContext.WebTemplateSet
                where x.Website.Id == id
                select x;

            Logger.Trace(CultureInfo.InvariantCulture, TraceMessageHelper.ExitingMethod, SystemTypeName, MethodBase.GetCurrentMethod().Name);

            return items.ToList();
        }

        public virtual Organization GetOrganizationSettings()
        {
            Logger.Trace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.EnteredMethod, SystemTypeName, MethodBase.GetCurrentMethod().Name));

            IQueryable<Organization> items =
                from x in OrganizationServiceContext.OrganizationSet
                select x;

            Logger.Trace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.ExitingMethod, SystemTypeName, MethodBase.GetCurrentMethod().Name));

            return items.SingleOrDefault();
        }

        public virtual ServiceEndpoint GetServiceEndpoint(Guid id)
        {
            Logger.Trace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.EnteredMethod, SystemTypeName, MethodBase.GetCurrentMethod().Name));

            if (Guid.Empty.Equals(id)) throw new ArgumentNullException(nameof(id));

            IQueryable<ServiceEndpoint> items =
                from x in OrganizationServiceContext.ServiceEndpointSet
                where x.Id == id
                select x;

            Logger.Trace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.ExitingMethod, SystemTypeName, MethodBase.GetCurrentMethod().Name));

            return items.SingleOrDefault();
        }

        public virtual ImportJob GetImportJob(Guid id)
        {
            Logger.Trace(CultureInfo.InvariantCulture, TraceMessageHelper.EnteredMethod, SystemTypeName, MethodBase.GetCurrentMethod().Name);

            if (Guid.Empty.Equals(id)) throw new ArgumentNullException(nameof(id));

            IQueryable<ImportJob> items =
                from x in OrganizationServiceContext.ImportJobSet
                where x.ImportJobId == id
                select x;

            Logger.Trace(CultureInfo.InvariantCulture, TraceMessageHelper.ExitingMethod, SystemTypeName, MethodBase.GetCurrentMethod().Name);

            return items.SingleOrDefault();
        }

        public virtual SystemJob GetSystemJob(Guid id)
        {
            Logger.Trace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.EnteredMethod, SystemTypeName, MethodBase.GetCurrentMethod().Name));

            if (Guid.Empty.Equals(id)) throw new ArgumentNullException(nameof(id));

            IQueryable<SystemJob> items =
                from x in OrganizationServiceContext.SystemJobSet
                where x.Id == id
                select x;

            Logger.Trace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.ExitingMethod, SystemTypeName, MethodBase.GetCurrentMethod().Name));

            return items.SingleOrDefault();
        }

        public virtual DeleteAndPromoteResponse DeleteAndPromoteSolution(string uniqueName)
        {
            Logger.Trace(CultureInfo.InvariantCulture, TraceMessageHelper.EnteredMethod, SystemTypeName, MethodBase.GetCurrentMethod().Name);

            if (string.IsNullOrWhiteSpace(uniqueName)) throw new ArgumentNullException(nameof(uniqueName));

            DeleteAndPromoteRequest req = new DeleteAndPromoteRequest
            {
                UniqueName = uniqueName
            };
            DeleteAndPromoteResponse res = (DeleteAndPromoteResponse)OrganizationServiceContext.Execute(req);

            Logger.Trace(CultureInfo.InvariantCulture, TraceMessageHelper.ExitingMethod, SystemTypeName, MethodBase.GetCurrentMethod().Name);

            return res;
        }

        public virtual ExecuteAsyncResponse DeleteAndPromoteSolutionAsync(string uniqueName)
        {
            Logger.Trace(CultureInfo.InvariantCulture, TraceMessageHelper.EnteredMethod, SystemTypeName, MethodBase.GetCurrentMethod().Name);

            if (string.IsNullOrWhiteSpace(uniqueName)) throw new ArgumentNullException(nameof(uniqueName));

            ExecuteAsyncRequest req = new ExecuteAsyncRequest
            {
                Request = new DeleteAndPromoteRequest
                {
                    UniqueName = uniqueName
                }
            };
            ExecuteAsyncResponse res = (ExecuteAsyncResponse)OrganizationServiceContext.Execute(req);

            Logger.Trace(CultureInfo.InvariantCulture, TraceMessageHelper.ExitingMethod, SystemTypeName, MethodBase.GetCurrentMethod().Name);

            return res;
        }

        public virtual DeleteResponse DeleteSolution(Guid id)
        {
            Logger.Trace(CultureInfo.InvariantCulture, TraceMessageHelper.EnteredMethod, SystemTypeName, MethodBase.GetCurrentMethod().Name);

            if (Guid.Empty.Equals(id)) throw new ArgumentNullException(nameof(id));

            DeleteRequest req = new DeleteRequest
            {
                Target = new EntityReference(Solution.EntityLogicalName, id)
            };
            DeleteResponse res = (DeleteResponse)OrganizationServiceContext.Execute(req);

            Logger.Trace(CultureInfo.InvariantCulture, TraceMessageHelper.ExitingMethod, SystemTypeName, MethodBase.GetCurrentMethod().Name);

            return res;
        }

        public virtual ExportSolutionResponse ExportSolution(string uniqueName, bool packageType)
        {
            Logger.Trace(CultureInfo.InvariantCulture, TraceMessageHelper.EnteredMethod, SystemTypeName, MethodBase.GetCurrentMethod().Name);

            if (string.IsNullOrWhiteSpace(uniqueName)) throw new ArgumentNullException(nameof(uniqueName));

            ExportSolutionRequest req = new ExportSolutionRequest
            {
                Managed = packageType,
                SolutionName = uniqueName
            };
            ExportSolutionResponse res = (ExportSolutionResponse)OrganizationServiceContext.Execute(req);

            Logger.Trace(CultureInfo.InvariantCulture, TraceMessageHelper.ExitingMethod, SystemTypeName, MethodBase.GetCurrentMethod().Name);

            return res;
        }

        public virtual ImportSolutionResponse ImportSolution(Guid importJobId, byte[] content, bool holdingSolution, bool overwriteUnmanagedCustomizations, bool publishWorkflows, bool skipProductUpdateDependencies)
        {
            Logger.Trace(CultureInfo.InvariantCulture, TraceMessageHelper.EnteredMethod, SystemTypeName, MethodBase.GetCurrentMethod().Name);

            if (Guid.Empty.Equals(importJobId)) throw new ArgumentNullException(nameof(importJobId));
            if (content == null) throw new ArgumentNullException(nameof(content));

            ImportSolutionRequest req = new ImportSolutionRequest()
            {
                CustomizationFile = content,
                HoldingSolution = holdingSolution,
                ImportJobId = importJobId,
                OverwriteUnmanagedCustomizations = overwriteUnmanagedCustomizations,
                PublishWorkflows = publishWorkflows,
                SkipProductUpdateDependencies = skipProductUpdateDependencies
            };
            ImportSolutionResponse res = (ImportSolutionResponse)OrganizationService.Execute(req);

            Logger.Trace(CultureInfo.InvariantCulture, TraceMessageHelper.ExitingMethod, SystemTypeName, MethodBase.GetCurrentMethod().Name);

            return res;
        }

        public virtual ExecuteAsyncResponse ImportSolutionAsync(Guid importJobId, byte[] content, bool holdingSolution, bool overwriteUnmanagedCustomizations, bool publishWorkflows, bool skipProductUpdateDependencies)
        {
            Logger.Trace(CultureInfo.InvariantCulture, TraceMessageHelper.EnteredMethod, SystemTypeName, MethodBase.GetCurrentMethod().Name);

            if (Guid.Empty.Equals(importJobId)) throw new ArgumentNullException(nameof(importJobId));
            if (content == null) throw new ArgumentNullException(nameof(content));

            ExecuteAsyncRequest req = new ExecuteAsyncRequest
            {
                Request = new ImportSolutionRequest
                {
                    CustomizationFile = content,
                    HoldingSolution = holdingSolution,
                    ImportJobId = importJobId,
                    OverwriteUnmanagedCustomizations = overwriteUnmanagedCustomizations,
                    PublishWorkflows = publishWorkflows,
                    SkipProductUpdateDependencies = skipProductUpdateDependencies
                }
            };

            ExecuteAsyncResponse res = (ExecuteAsyncResponse)OrganizationService.Execute(req);

            Logger.Trace(CultureInfo.InvariantCulture, TraceMessageHelper.ExitingMethod, SystemTypeName, MethodBase.GetCurrentMethod().Name);

            return res;
        }

        public virtual PublishAllXmlResponse PublishAllCustomizations()
        {
            Logger.Trace(CultureInfo.InvariantCulture, TraceMessageHelper.EnteredMethod, SystemTypeName, MethodBase.GetCurrentMethod().Name);

            PublishAllXmlResponse res = (PublishAllXmlResponse)OrganizationService.Execute(new PublishAllXmlRequest());

            Logger.Trace(CultureInfo.InvariantCulture, TraceMessageHelper.ExitingMethod, SystemTypeName, MethodBase.GetCurrentMethod().Name);

            return res;
        }

        public virtual RemoveSolutionComponentResponse RemoveSolutionComponent(string solutionName, Guid componentId, int componentType)
        {
            Logger.Trace(CultureInfo.InvariantCulture, TraceMessageHelper.EnteredMethod, SystemTypeName, MethodBase.GetCurrentMethod().Name);

            RemoveSolutionComponentResponse res = (RemoveSolutionComponentResponse)OrganizationService.Execute(new RemoveSolutionComponentRequest
            {
                ComponentId = componentId,
                ComponentType = componentType,
                SolutionUniqueName = solutionName
            });

            Logger.Trace(CultureInfo.InvariantCulture, TraceMessageHelper.ExitingMethod, SystemTypeName, MethodBase.GetCurrentMethod().Name);

            return res;
        }

        public virtual UpdateResponse UpdateEntity(Entity entity)
        {
            Logger.Trace(CultureInfo.InvariantCulture, TraceMessageHelper.EnteredMethod, SystemTypeName, MethodBase.GetCurrentMethod().Name);

            if (entity == null) throw new ArgumentNullException(nameof(entity));

            UpdateRequest req = new UpdateRequest
            {
                Target = entity
            };
            UpdateResponse res = (UpdateResponse)OrganizationServiceContext.Execute(req);

            Logger.Trace(CultureInfo.InvariantCulture, TraceMessageHelper.ExitingMethod, SystemTypeName, MethodBase.GetCurrentMethod().Name);

            return res;
        }

        public virtual SaveChangesResultCollection SaveChanges(OrganizationServiceContext organizationServiceContext, SaveChangesOptions saveChangesOptions)
        {
            Logger.Trace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.EnteredMethod, SystemTypeName, MethodBase.GetCurrentMethod().Name));

            if (organizationServiceContext == null) throw new ArgumentNullException(nameof(organizationServiceContext));

            Logger.Trace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.ExitingMethod, SystemTypeName, MethodBase.GetCurrentMethod().Name));

            return organizationServiceContext.SaveChanges(saveChangesOptions);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    
                }
                disposedValue = true;
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
    }
}
