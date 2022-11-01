namespace Aylos.Xrm.Sdk.Plugins.MoqTests
{
    using Aylos.Xrm.Sdk.Common;
    using Microsoft.Xrm.Sdk;
    using Moq;
    using System;

    public abstract class CustomPluginUnitTest<T>  where T : CustomPlugin
    {
        protected T Plugin { get; set; }

        protected InvalidPluginExecutionException ActualException { get; set; }

        protected PluginExecutionContext CreatePluginExecutionContext()
        {
            var pec = new PluginExecutionContext
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

        protected PluginExecutionContext CreatePluginExecutionContextEntityMoniker(EntityReference entityMoniker, Entity preEntity, Entity postEntity)
        {
            PluginExecutionContext pec = CreatePluginExecutionContext();
            if (entityMoniker != null) pec.InputParameters[PlatformConstants.EntityMonikerText] = entityMoniker;
            if (preEntity != null) pec.PreEntityImages[PlatformConstants.PreBusinessEntityText] = preEntity;
            if (postEntity != null) pec.PostEntityImages[PlatformConstants.PostBusinessEntityText] = postEntity;
            return pec;
        }

        protected void InitializeUnitTest(PluginExecutionContext context)
        {
            var mock = new Mock<T>().SetupAllProperties();

            mock.Setup(x => x.Validate()).CallBase();
            mock.Setup(x => x.Execute()).CallBase();

            Plugin = mock.Object;
            Plugin.SystemUserService = Mock.Of<IOrganizationService>();
            Plugin.CurrentUserService = Mock.Of<IOrganizationService>();
            Plugin.NotificationService = Mock.Of<IServiceEndpointNotificationService>();
            Plugin.TracingService = Mock.Of<ITracingService>();

            SetupMockForPluginExecutionContext(context);
            SetupMockForOrganizationServiceFactory();
            SetupMockForServiceProvider();
        }

        protected void SetupMockForServiceProvider()
        {
            var mock = new Mock<IServiceProvider>().SetupAllProperties();

            mock.Setup(x => x.GetService(typeof(ITracingService))).Returns(Plugin.TracingService);
            mock.Setup(x => x.GetService(typeof(IPluginExecutionContext))).Returns(Plugin.PluginExecutionContext);
            mock.Setup(x => x.GetService(typeof(IOrganizationServiceFactory))).Returns(Plugin.OrganizationServiceFactory);
            mock.Setup(x => x.GetService(typeof(IServiceEndpointNotificationService))).Returns(Plugin.NotificationService);

            Plugin.ServiceProvider = mock.Object;
        }

        protected void SetupMockForPluginExecutionContext(PluginExecutionContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            var mock = new Mock<IPluginExecutionContext>().SetupAllProperties();

            mock.Setup(x => x.BusinessUnitId).Returns(context.BusinessUnitId);
            mock.Setup(x => x.CorrelationId).Returns(context.CorrelationId);
            mock.Setup(x => x.Depth).Returns(context.Depth);
            mock.Setup(x => x.InitiatingUserId).Returns(context.InitiatingUserId);
            mock.Setup(x => x.InputParameters).Returns(context.InputParameters);
            mock.Setup(x => x.IsExecutingOffline).Returns(context.IsExecutingOffline);
            mock.Setup(x => x.IsInTransaction).Returns(context.IsInTransaction);
            mock.Setup(x => x.IsOfflinePlayback).Returns(context.IsOfflinePlayback);
            mock.Setup(x => x.IsolationMode).Returns(context.IsolationMode);
            mock.Setup(x => x.MessageName).Returns(context.MessageName);
            mock.Setup(x => x.Mode).Returns(context.Mode);
            mock.Setup(x => x.OperationCreatedOn).Returns(context.OperationCreatedOn);
            mock.Setup(x => x.OperationId).Returns(context.OperationId);
            mock.Setup(x => x.OrganizationId).Returns(context.OrganizationId);
            mock.Setup(x => x.OrganizationName).Returns(context.OrganizationName);
            mock.Setup(x => x.OutputParameters).Returns(context.OutputParameters);
            mock.Setup(x => x.OwningExtension).Returns(context.OwningExtension);
            mock.Setup(x => x.ParentContext).Returns(context.ParentContext);
            mock.Setup(x => x.PostEntityImages).Returns(context.PostEntityImages);
            mock.Setup(x => x.PreEntityImages).Returns(context.PreEntityImages);
            mock.Setup(x => x.PrimaryEntityId).Returns(context.PrimaryEntityId);
            mock.Setup(x => x.PrimaryEntityName).Returns(context.PrimaryEntityName);
            mock.Setup(x => x.RequestId).Returns(context.RequestId);
            mock.Setup(x => x.SecondaryEntityName).Returns(context.SecondaryEntityName);
            mock.Setup(x => x.SharedVariables).Returns(context.SharedVariables);
            mock.Setup(x => x.Stage).Returns(context.Stage);
            mock.Setup(x => x.UserId).Returns(context.UserId);

            Plugin.PluginExecutionContext = mock.Object;
        }

        protected void SetupMockForOrganizationServiceFactory()
        {
            var mock = new Mock<IOrganizationServiceFactory>().SetupAllProperties();

            mock.Setup(x => x.CreateOrganizationService(It.IsAny<Guid?>()))
                .Returns((Guid? x) => {
                    if (x == null)
                    {
                        return Plugin.SystemUserService;
                    }
                    else
                    {
                        return Plugin.CurrentUserService;
                    }
                });

            Plugin.OrganizationServiceFactory = mock.Object;
        }

        public abstract void SetupMockObjectsForPlugin();
    }
}