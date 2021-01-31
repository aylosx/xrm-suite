namespace Aylos.Xrm.Sdk.BuildTools.Services
{
    using Aylos.Xrm.Sdk.BuildTools.Models.Domain;
    using Microsoft.Crm.Sdk.Messages;
    using Microsoft.Xrm.Sdk;
    using Microsoft.Xrm.Sdk.Client;
    using Microsoft.Xrm.Sdk.Messages;
    using System;
    using System.Collections.Generic;

    public interface ICrmService : IDisposable
    {
        IList<Note> GetAnnotations(Guid id);

        IList<Solution> GetSolutions(string uniqueName);

        IList<Solution> GetSolutionsEndsWith(string wildcard);

        IList<Solution> GetSolutionsStartsWith(string wildcard);

        IList<WebFile> GetWebsiteFiles(Guid id);

        IList<WebTemplate> GetWebTemplates(Guid id);

        Organization GetOrganizationSettings();

        ServiceEndpoint GetServiceEndpoint(Guid id);

        ImportJob GetImportJob(Guid id);

        SystemJob GetSystemJob(Guid id);

        DeleteAndPromoteResponse DeleteAndPromoteSolution(string uniqueName);

        ExecuteAsyncResponse DeleteAndPromoteSolutionAsync(string uniqueName);

        DeleteResponse DeleteSolution(Guid id);

        ExportSolutionResponse ExportSolution(string uniqueName, bool packageType);

        ImportSolutionResponse ImportSolution(Guid importJobId, byte[] content, bool holdingSolution, bool overwriteUnmanagedCustomizations, bool publishWorkflows, bool skipProductUpdateDependencies);

        ExecuteAsyncResponse ImportSolutionAsync(Guid importJobId, byte[] content, bool holdingSolution, bool overwriteUnmanagedCustomizations, bool publishWorkflows, bool skipProductUpdateDependencies);

        PublishAllXmlResponse PublishAllCustomizations();

        RemoveSolutionComponentResponse RemoveSolutionComponent(string solutionName, Guid componentId, int componentType);

        UpdateResponse UpdateEntity(Entity entity);

        SaveChangesResultCollection SaveChanges(OrganizationServiceContext organizationServiceContext, SaveChangesOptions saveChangesOptions);
    }
}
