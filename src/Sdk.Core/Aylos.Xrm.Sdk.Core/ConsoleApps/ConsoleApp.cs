namespace Aylos.Xrm.Sdk.Core.ConsoleApps
{
    using Aylos.Xrm.Sdk.Common;
    using Microsoft.Xrm.Sdk;
    using Microsoft.Xrm.Sdk.Client;
    using Microsoft.PowerPlatform.Dataverse.Client;
    using NLog;
    using System;

    public abstract class ConsoleApp
    {
        string SystemTypeName { get { return GetType().UnderlyingSystemType.Name; } }

        public Logger Logger { get; private set; }

        public LogFactory LogFactory { get; private set; }

        protected ConsoleApp()
        {
            LogFactory = new LogFactory();
            Logger = LogFactory.GetLogger(SystemTypeName);
        }
    }

    public abstract class ConsoleService<T> : ConsoleApp where T : OrganizationServiceContext
    {
        public IOrganizationService OrganizationService { get; set; }

        public ServiceClient ServiceClient { get; set; }

        public Connection Connection { get; set; }

        public T OrganizationServiceContext { get; set; }

        protected ConsoleService() : base()
        { 
        }

        public ConsoleService(Connection connection) : this()
        {
            Connection = connection ?? throw new ArgumentNullException(nameof(connection));
            OrganizationService = connection.OrganizationService;
        }

        public ConsoleService(ServiceClient serviceClient) : this()
        {
            ServiceClient = serviceClient ?? throw new ArgumentNullException(nameof(serviceClient));
        }

        public ConsoleService(T organizationServiceContext) : this()
        {
            OrganizationServiceContext = organizationServiceContext ?? throw new ArgumentNullException(nameof(organizationServiceContext));
        }

        public ConsoleService(T organizationServiceContext, Connection connection) : this(connection)
        {
            OrganizationServiceContext = organizationServiceContext ?? throw new ArgumentNullException(nameof(organizationServiceContext));
        }
    }
}