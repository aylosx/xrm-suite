namespace Aylos.Xrm.Sdk.Plugins.RhinoMocks
{
    using Aylos.Xrm.Sdk.Common;
    using Microsoft.Xrm.Sdk;
    using Rhino.Mocks;
    using System;

    public abstract class CustomPluginUnitTest<T>  where T : CustomPlugin
    {
        protected T Plugin { get; set; }

        protected InvalidPluginExecutionException ActualException { get; set; }

        protected void InitializeUnitTest(PluginExecutionContext context)
        {
            SetupMockObjects();
            SetupMockResponseForServiceProvider();
            SetupMockResponseForPluginExecutionContext(context);
            SetupMockResponseForOrganizationServiceFactory();
            SetupMockResponseForOverriddenMethods();
        }

        protected PluginExecutionContext CreatePluginExecutionContext()
        {
            PluginExecutionContext pec = new PluginExecutionContext
            {
                BusinessUnitId = Guid.NewGuid(),
                CorrelationId = Guid.NewGuid(),
                Depth = 1,
                InitiatingUserId = Guid.NewGuid(),
                InputParameters = new ParameterCollection(),
                IsExecutingOffline = false,
                IsInTransaction = false,
                IsOfflinePlayback = false,
                IsolationMode = (int)PluginAssemblyIsolationMode.Sandbox,
                MessageName = "Message",
                Mode = 0,
                OperationCreatedOn = DateTime.Now,
                OperationId = Guid.NewGuid(),
                OrganizationId = Guid.NewGuid(),
                OrganizationName = "Organization",
                OutputParameters = new ParameterCollection(),
                OwningExtension = new EntityReference { Id = Guid.NewGuid(), LogicalName = "none" },
                ParentContext = null,
                PostEntityImages = new EntityImageCollection(),
                PreEntityImages = new EntityImageCollection(),
                PrimaryEntityId = Guid.Empty,
                PrimaryEntityName = "none",
                RequestId = Guid.Empty,
                SecondaryEntityName = string.Empty,
                SharedVariables = new ParameterCollection(),
                Stage = 0,
                UserId = Guid.NewGuid()
            };

            return pec;
        }

        protected PluginExecutionContext CreatePluginExecutionContext(Entity targetEntity)
        {
            PluginExecutionContext pec = CreatePluginExecutionContext();
            pec.InputParameters[PlatformConstants.TargetText] = targetEntity;
            return pec;
        }

        protected PluginExecutionContext CreatePluginExecutionContext(Entity targetEntity, Entity preEntity, Entity postEntity)
        {
            PluginExecutionContext pec = CreatePluginExecutionContext(targetEntity);
            if (preEntity != null) pec.PreEntityImages[PlatformConstants.PreBusinessEntityText] = preEntity;
            if (postEntity != null) pec.PostEntityImages[PlatformConstants.PostBusinessEntityText] = postEntity;
            return pec;
        }

        protected PluginExecutionContext CreatePluginExecutionContext(EntityReference targetEntityReference)
        {
            PluginExecutionContext pec = CreatePluginExecutionContext();
            pec.InputParameters[PlatformConstants.TargetEntityReferenceText] = targetEntityReference;
            return pec;
        }

        protected PluginExecutionContext CreatePluginExecutionContext(EntityReference targetEntityReference, Entity preEntity, Entity postEntity)
        {
            PluginExecutionContext pec = CreatePluginExecutionContext(targetEntityReference);
            if (preEntity != null) pec.PreEntityImages[PlatformConstants.PreBusinessEntityText] = preEntity;
            if (postEntity != null) pec.PostEntityImages[PlatformConstants.PostBusinessEntityText] = postEntity;
            return pec;
        }

        protected PluginExecutionContext CreatePluginExecutionContext(string messageName, int depth, string primaryEntityName, int pipelineStage)
        {
            PluginExecutionContext pec = CreatePluginExecutionContext();
            pec.Depth = depth;
            pec.MessageName = messageName;
            pec.PrimaryEntityName = primaryEntityName;
            pec.Stage = pipelineStage;
            return pec;
        }

        protected PluginExecutionContext CreatePluginExecutionContext(string messageName, int depth, string primaryEntityName, int pipelineStage, Entity targetEntity)
        {
            PluginExecutionContext pec = CreatePluginExecutionContext(messageName, depth, primaryEntityName, pipelineStage);
            pec.InputParameters[PlatformConstants.TargetText] = targetEntity;
            pec.PrimaryEntityId = targetEntity.Id;
            return pec;
        }

        protected PluginExecutionContext CreatePluginExecutionContext(string messageName, int depth, string primaryEntityName, int pipelineStage, Entity targetEntity, Entity preEntity, Entity postEntity)
        {
            PluginExecutionContext pec = CreatePluginExecutionContext(messageName, depth, primaryEntityName, pipelineStage, targetEntity);
            if (preEntity != null) pec.PreEntityImages[PlatformConstants.PreBusinessEntityText] = preEntity;
            if (postEntity != null) pec.PostEntityImages[PlatformConstants.PostBusinessEntityText] = postEntity;
            return pec;
        }

        protected PluginExecutionContext CreatePluginExecutionContext(string messageName, int depth, string primaryEntityName, int pipelineStage, EntityReference targetEntityReference)
        {
            PluginExecutionContext pec = CreatePluginExecutionContext(messageName, depth, primaryEntityName, pipelineStage);
            pec.InputParameters[PlatformConstants.TargetEntityReferenceText] = targetEntityReference;
            return pec;
        }

        protected PluginExecutionContext CreatePluginExecutionContext(string messageName, int depth, string primaryEntityName, int pipelineStage, EntityReference targetEntityReference, Entity preEntity, Entity postEntity)
        {
            PluginExecutionContext pec = CreatePluginExecutionContext(messageName, depth, primaryEntityName, pipelineStage, targetEntityReference);
            if (preEntity != null) pec.PreEntityImages[PlatformConstants.PreBusinessEntityText] = preEntity;
            if (postEntity != null) pec.PostEntityImages[PlatformConstants.PostBusinessEntityText] = postEntity;
            return pec;
        }

        protected PluginExecutionContext CreatePluginUnitTestContextEntityMoniker(EntityReference entityMoniker, Entity preEntity, Entity postEntity)
        {
            PluginExecutionContext pec = CreatePluginExecutionContext();
            if (entityMoniker != null) pec.InputParameters[PlatformConstants.EntityMonikerText] = entityMoniker;
            if (preEntity != null) pec.PreEntityImages[PlatformConstants.PreBusinessEntityText] = preEntity;
            if (postEntity != null) pec.PostEntityImages[PlatformConstants.PostBusinessEntityText] = postEntity;
            return pec;
        }

        protected void SetupMockObjects()
        {
            Plugin = MockRepository.GenerateMock<T>();
            Plugin.SystemUserService = MockRepository.GenerateStub<IOrganizationService>();
            Plugin.CurrentUserService = MockRepository.GenerateStub<IOrganizationService>();
            Plugin.OrganizationServiceFactory = MockRepository.GenerateStub<IOrganizationServiceFactory>();
            Plugin.NotificationService = MockRepository.GenerateStub<IServiceEndpointNotificationService>();
            Plugin.TracingService = MockRepository.GenerateStub<ITracingService>();
            Plugin.PluginExecutionContext = MockRepository.GenerateStub<IPluginExecutionContext>();
            Plugin.ServiceProvider = MockRepository.GenerateStub<IServiceProvider>();
            ActualException = null;
        }

        protected void SetupMockResponseForServiceProvider()
        {
            Plugin.ServiceProvider.Stub(x => x.GetService(typeof(ITracingService))).Return(Plugin.TracingService);
            Plugin.ServiceProvider.Stub(x => x.GetService(typeof(IPluginExecutionContext))).Return(Plugin.PluginExecutionContext);
            Plugin.ServiceProvider.Stub(x => x.GetService(typeof(IOrganizationServiceFactory))).Return(Plugin.OrganizationServiceFactory);
            Plugin.ServiceProvider.Stub(x => x.GetService(typeof(IServiceEndpointNotificationService))).Return(Plugin.NotificationService);
        }

        protected void SetupMockResponseForOrganizationServiceFactory()
        {
            Plugin.OrganizationServiceFactory.Stub(x => x.CreateOrganizationService(Plugin.PluginExecutionContext.UserId)).Return(Plugin.CurrentUserService);
            Plugin.OrganizationServiceFactory.Stub(x => x.CreateOrganizationService(null)).Return(Plugin.SystemUserService);
        }

        protected void SetupMockResponseForPluginExecutionContext(PluginExecutionContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            Plugin.PluginExecutionContext.Stub(x => x.BusinessUnitId).Return(context.BusinessUnitId);
            Plugin.PluginExecutionContext.Stub(x => x.CorrelationId).Return(context.CorrelationId);
            Plugin.PluginExecutionContext.Stub(x => x.Depth).Return(context.Depth);
            Plugin.PluginExecutionContext.Stub(x => x.InitiatingUserId).Return(context.InitiatingUserId);
            Plugin.PluginExecutionContext.Stub(x => x.InputParameters).Return(context.InputParameters);
            Plugin.PluginExecutionContext.Stub(x => x.IsExecutingOffline).Return(context.IsExecutingOffline);
            Plugin.PluginExecutionContext.Stub(x => x.IsInTransaction).Return(context.IsInTransaction);
            Plugin.PluginExecutionContext.Stub(x => x.IsOfflinePlayback).Return(context.IsOfflinePlayback);
            Plugin.PluginExecutionContext.Stub(x => x.IsolationMode).Return(context.IsolationMode);
            Plugin.PluginExecutionContext.Stub(x => x.MessageName).Return(context.MessageName);
            Plugin.PluginExecutionContext.Stub(x => x.Mode).Return(context.Mode);
            Plugin.PluginExecutionContext.Stub(x => x.OperationCreatedOn).Return(context.OperationCreatedOn);
            Plugin.PluginExecutionContext.Stub(x => x.OperationId).Return(context.OperationId);
            Plugin.PluginExecutionContext.Stub(x => x.OrganizationId).Return(context.OrganizationId);
            Plugin.PluginExecutionContext.Stub(x => x.OrganizationName).Return(context.OrganizationName);
            Plugin.PluginExecutionContext.Stub(x => x.OutputParameters).Return(context.OutputParameters);
            Plugin.PluginExecutionContext.Stub(x => x.OwningExtension).Return(context.OwningExtension);
            Plugin.PluginExecutionContext.Stub(x => x.ParentContext).Return(context.ParentContext);
            Plugin.PluginExecutionContext.Stub(x => x.PostEntityImages).Return(context.PostEntityImages);
            Plugin.PluginExecutionContext.Stub(x => x.PreEntityImages).Return(context.PreEntityImages);
            Plugin.PluginExecutionContext.Stub(x => x.PrimaryEntityId).Return(context.PrimaryEntityId);
            Plugin.PluginExecutionContext.Stub(x => x.PrimaryEntityName).Return(context.PrimaryEntityName);
            Plugin.PluginExecutionContext.Stub(x => x.RequestId).Return(context.RequestId);
            Plugin.PluginExecutionContext.Stub(x => x.SecondaryEntityName).Return(context.SecondaryEntityName);
            Plugin.PluginExecutionContext.Stub(x => x.SharedVariables).Return(context.SharedVariables);
            Plugin.PluginExecutionContext.Stub(x => x.Stage).Return(context.Stage);
            Plugin.PluginExecutionContext.Stub(x => x.UserId).Return(context.UserId);
        }

        protected void SetupMockResponseForOverriddenMethods()
        {
            Plugin.Stub(x => x.Validate()).CallOriginalMethod(Rhino.Mocks.Interfaces.OriginalCallOptions.NoExpectation);
            Plugin.Stub(x => x.Execute()).CallOriginalMethod(Rhino.Mocks.Interfaces.OriginalCallOptions.NoExpectation);
        }

        public abstract void SetupMockObjectsForPlugin();
    }
}