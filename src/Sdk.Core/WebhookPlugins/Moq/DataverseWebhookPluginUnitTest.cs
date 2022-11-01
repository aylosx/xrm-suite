namespace Aylos.Xrm.Sdk.Core.WebhookPlugins.MoqTests
{
    using Aylos.Xrm.Sdk.Common;
    using Aylos.Xrm.Sdk.Core.WebhookPlugins;

    using Microsoft.Extensions.Logging;
    using Microsoft.Xrm.Sdk;

    using Moq;

    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Text;

    public abstract class DataverseWebhookPluginUnitTest<T>  where T : DataverseWebhookPlugin
    {
        protected T DataverseWebhookPlugin { get; set; }

        protected RemoteExecutionContext CreateRemoteExecutionContext()
        {
            var rec = new RemoteExecutionContext
            {
                BusinessUnitId = Guid.NewGuid(),
                CorrelationId = Guid.NewGuid(),
                Depth = 1,
                //InitiatingUserAzureActiveDirectoryObjectId = Guid.Empty,
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
                //UserAzureActiveDirectoryObjectId = Guid.Empty,
                UserId = Guid.NewGuid()
            };

            return rec;
        }

        protected RemoteExecutionContext CreateRemoteExecutionContext(Entity targetEntity)
        {
            RemoteExecutionContext rec = CreateRemoteExecutionContext();
            rec.InputParameters[PlatformConstants.TargetText] = targetEntity;
            return rec;
        }

        protected RemoteExecutionContext CreateRemoteExecutionContext(Entity targetEntity, Entity preEntity, Entity postEntity)
        {
            RemoteExecutionContext rec = CreateRemoteExecutionContext(targetEntity);
            if (preEntity != null) rec.PreEntityImages[PlatformConstants.PreBusinessEntityText] = preEntity;
            if (postEntity != null) rec.PostEntityImages[PlatformConstants.PostBusinessEntityText] = postEntity;
            return rec;
        }

        protected RemoteExecutionContext CreateRemoteExecutionContext(EntityReference targetEntityReference)
        {
            RemoteExecutionContext rec = CreateRemoteExecutionContext();
            rec.InputParameters[PlatformConstants.TargetEntityReferenceText] = targetEntityReference;
            return rec;
        }

        protected RemoteExecutionContext CreateRemoteExecutionContext(EntityReference targetEntityReference, Entity preEntity, Entity postEntity)
        {
            RemoteExecutionContext rec = CreateRemoteExecutionContext(targetEntityReference);
            if (preEntity != null) rec.PreEntityImages[PlatformConstants.PreBusinessEntityText] = preEntity;
            if (postEntity != null) rec.PostEntityImages[PlatformConstants.PostBusinessEntityText] = postEntity;
            return rec;
        }

        protected RemoteExecutionContext CreateRemoteExecutionContext(string messageName, int depth, string primaryEntityName, int pipelineStage)
        {
            RemoteExecutionContext rec = CreateRemoteExecutionContext();
            rec.Depth = depth;
            rec.MessageName = messageName;
            rec.PrimaryEntityName = primaryEntityName;
            rec.Stage = pipelineStage;
            return rec;
        }

        protected RemoteExecutionContext CreateRemoteExecutionContext(string messageName, int depth, string primaryEntityName, int pipelineStage, Entity targetEntity)
        {
            RemoteExecutionContext rec = CreateRemoteExecutionContext(messageName, depth, primaryEntityName, pipelineStage);
            rec.InputParameters[PlatformConstants.TargetText] = targetEntity;
            rec.PrimaryEntityId = targetEntity.Id;
            return rec;
        }

        protected RemoteExecutionContext CreateRemoteExecutionContext(string messageName, int depth, string primaryEntityName, int pipelineStage, Entity targetEntity, Entity preEntity, Entity postEntity)
        {
            RemoteExecutionContext rec = CreateRemoteExecutionContext(messageName, depth, primaryEntityName, pipelineStage, targetEntity);
            if (preEntity != null) rec.PreEntityImages[PlatformConstants.PreBusinessEntityText] = preEntity;
            if (postEntity != null) rec.PostEntityImages[PlatformConstants.PostBusinessEntityText] = postEntity;
            return rec;
        }

        protected RemoteExecutionContext CreateRemoteExecutionContext(string messageName, int depth, string primaryEntityName, int pipelineStage, EntityReference targetEntityReference)
        {
            RemoteExecutionContext rec = CreateRemoteExecutionContext(messageName, depth, primaryEntityName, pipelineStage);
            rec.InputParameters[PlatformConstants.TargetEntityReferenceText] = targetEntityReference;
            return rec;
        }

        protected RemoteExecutionContext CreateRemoteExecutionContext(string messageName, int depth, string primaryEntityName, int pipelineStage, EntityReference targetEntityReference, Entity preEntity, Entity postEntity)
        {
            RemoteExecutionContext rec = CreateRemoteExecutionContext(messageName, depth, primaryEntityName, pipelineStage, targetEntityReference);
            if (preEntity != null) rec.PreEntityImages[PlatformConstants.PreBusinessEntityText] = preEntity;
            if (postEntity != null) rec.PostEntityImages[PlatformConstants.PostBusinessEntityText] = postEntity;
            return rec;
        }

        protected RemoteExecutionContext CreateRemoteExecutionContextEntityMoniker(EntityReference entityMoniker, Entity preEntity, Entity postEntity)
        {
            RemoteExecutionContext rec = CreateRemoteExecutionContext();
            if (entityMoniker != null) rec.InputParameters[PlatformConstants.EntityMonikerText] = entityMoniker;
            if (preEntity != null) rec.PreEntityImages[PlatformConstants.PreBusinessEntityText] = preEntity;
            if (postEntity != null) rec.PostEntityImages[PlatformConstants.PostBusinessEntityText] = postEntity;
            return rec;
        }

        private HttpRequestMessage CreateHttpRequestMessage(Dictionary<string, string> headers, RemoteExecutionContext context)
        {
            var mock = new Mock<HttpRequestMessage>().SetupAllProperties();

            foreach (var pair in headers)
            {
                mock.Object.Headers.Add(pair.Key, headers[pair.Key]);
            }

            string body = SerializationHelper.SerializeJson(context);

            mock.Object.Content = new StringContent(body, Encoding.UTF8, "application/json");

            return mock.Object;
        }

        private Microsoft.Xrm.Sdk.RemoteExecutionContext CreateMockForRemoteExecutionContext(RemoteExecutionContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            string body = SerializationHelper.SerializeJson(context);

            return SerializationHelper.DeserializeJson<Microsoft.Xrm.Sdk.RemoteExecutionContext>(body);
        }

        private Microsoft.PowerPlatform.Dataverse.Client.ServiceClient CreateMockForServiceClient(ServiceClient client)
        {
            if (client == null) throw new ArgumentNullException(nameof(client));

            string body = SerializationHelper.SerializeJson(client);

            return SerializationHelper.DeserializeJson<Microsoft.PowerPlatform.Dataverse.Client.ServiceClient>(body);
        }

        protected void InitializeUnitTest(Dictionary<string, string> headers, RemoteExecutionContext context)
        {
            if (headers == null) throw new ArgumentNullException(nameof(headers));
            if (context == null) throw new ArgumentNullException(nameof(context));

            var mock = new Mock<T>().SetupAllProperties();

            mock.Setup(x => x.Validate()).CallBase();
            mock.Setup(x => x.Execute()).CallBase();

            DataverseWebhookPlugin = mock.Object;
            DataverseWebhookPlugin.HttpRequestMessage = CreateHttpRequestMessage(headers, context);
            DataverseWebhookPlugin.Logger = Mock.Of<ILogger>();
            DataverseWebhookPlugin.LoggerFactory = Mock.Of<ILoggerFactory>();
            DataverseWebhookPlugin.RemoteExecutionContext = CreateMockForRemoteExecutionContext(context);
            DataverseWebhookPlugin.ServiceClient = CreateMockForServiceClient(new ServiceClient());
        }

        protected abstract void SetupMockObjectsForPlugin();
    }
}