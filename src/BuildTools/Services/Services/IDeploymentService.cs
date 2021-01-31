namespace Aylos.Xrm.Sdk.BuildTools.Services
{
    using Aylos.Xrm.Sdk.BuildTools.Models.Domain;
    using System;

    public interface IDeploymentService : IDisposable
    {
        void UpdateOrganizationSettings(Organization organization);

        void UpdateServiceEndpoint(string primaryKey, string namespaceAddress, string sharedAccessKey, string serviceNamespace);
    }
}