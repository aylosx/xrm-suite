namespace Aylos.Xrm.Sdk.BuildTools.Services
{
    using Aylos.Xrm.Sdk.BuildTools.Models.Domain;
    using Aylos.Xrm.Sdk.Common;
    using Aylos.Xrm.Sdk.ConsoleApps;
    using Aylos.Xrm.Sdk.Exceptions;
    using Microsoft.Xrm.Sdk.Client;
    using System;
    using System.Globalization;
    using System.Reflection;

    public sealed class DeploymentService : ConsoleService<CrmServiceContext>, IDeploymentService
    {
        ICrmService CrmService { get; set; }

        bool disposedValue;

        string SystemTypeName { get { return GetType().UnderlyingSystemType.Name; } }

        public DeploymentService(CrmServiceContext organizationServiceContext, ICrmService crmService) : base(organizationServiceContext)
        {
            Logger.Trace(CultureInfo.InvariantCulture, TraceMessageHelper.EnteredMethod, SystemTypeName, MethodBase.GetCurrentMethod().Name);

            CrmService = crmService ?? throw new ArgumentNullException(nameof(crmService));

            Logger.Trace(CultureInfo.InvariantCulture, TraceMessageHelper.ExitingMethod, SystemTypeName, MethodBase.GetCurrentMethod().Name);
        }

        public void UpdateOrganizationSettings(Organization organization)
        {
            Logger.Trace(CultureInfo.InvariantCulture, TraceMessageHelper.EnteredMethod, SystemTypeName, MethodBase.GetCurrentMethod().Name);

            if (organization == null) throw new ArgumentNullException(nameof(organization));

            Organization organizationSettings = CrmService.GetOrganizationSettings();
            if (organizationSettings == null) throw new EntityNotFoundException("No organization record can be found in the system.");

            OrganizationServiceContext.ClearChanges();

            //TO-DO: this is in experimental mode 

            Organization updatedOrganizationSettings = new Organization
            {
                Id = organizationSettings.Id,
                //BlockAttachments = organization.BlockAttachments,
                OrganizationName = organization.OrganizationName
            };

            OrganizationServiceContext.Attach(updatedOrganizationSettings);

            OrganizationServiceContext.UpdateObject(updatedOrganizationSettings);

            CrmService.SaveChanges(OrganizationServiceContext, SaveChangesOptions.None);

            //Logger.Info(CultureInfo.InvariantCulture, "Blocked attachments updated to: {0}.", updatedOrganizationSettings.BlockAttachments);

            Logger.Trace(CultureInfo.InvariantCulture, TraceMessageHelper.ExitingMethod, SystemTypeName, MethodBase.GetCurrentMethod().Name);
        }

        public void UpdateServiceEndpoint(string primaryKey, string namespaceAddress, string sharedAccessKey, string serviceNamespace)
        {
            Logger.Trace(CultureInfo.InvariantCulture, TraceMessageHelper.EnteredMethod, SystemTypeName, MethodBase.GetCurrentMethod().Name);

            if (string.IsNullOrWhiteSpace(primaryKey)) throw new ArgumentNullException(nameof(primaryKey));
            Logger.Info(CultureInfo.InvariantCulture, "Primary key: {0}.", primaryKey);

            bool isValidGuid = Guid.TryParse(primaryKey, out Guid key);
            if (!isValidGuid) throw new ArgumentException("Primary key argument is invalid.");

            if (string.IsNullOrWhiteSpace(namespaceAddress)) throw new ArgumentNullException(nameof(namespaceAddress));
            Logger.Info(CultureInfo.InvariantCulture, "Namespace address: {0}.", namespaceAddress);

            if (string.IsNullOrWhiteSpace(sharedAccessKey)) throw new ArgumentNullException(nameof(sharedAccessKey));

            if (string.IsNullOrWhiteSpace(serviceNamespace)) throw new ArgumentNullException(nameof(serviceNamespace));
            Logger.Info(CultureInfo.InvariantCulture, "Service namespace: {0}.", serviceNamespace);

            ServiceEndpoint serviceEndpoint = CrmService.GetServiceEndpoint(key);
            if (serviceEndpoint == null) throw new EntityNotFoundException("Service endpoint not found.");

            OrganizationServiceContext.ClearChanges();

            ServiceEndpoint se = new ServiceEndpoint
            {
                ServiceEndpointId = serviceEndpoint.Id,
                ContentTypeOfTheMessage = ServiceEndpointMessageFormat.Json,
                NamespaceAddress = namespaceAddress,
                //SASKey = sharedAccessKey, //TO-DO
                ServiceNamespace = serviceNamespace
            };

            OrganizationServiceContext.Attach(se);

            OrganizationServiceContext.UpdateObject(se);

            CrmService.SaveChanges(OrganizationServiceContext, SaveChangesOptions.None);

            Logger.Trace(CultureInfo.InvariantCulture, TraceMessageHelper.ExitingMethod, SystemTypeName, MethodBase.GetCurrentMethod().Name);
        }

        void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (CrmService != null) CrmService.Dispose();
                    if (OrganizationServiceContext != null) OrganizationServiceContext.Dispose();
                }
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}