namespace Aylos.Xrm.Sdk.CodeActivities.MoqTests
{
    using Aylos.Xrm.Sdk.Common;
    using Microsoft.Xrm.Sdk;
    using Microsoft.Xrm.Sdk.Workflow;
    using Moq;
    using Moq.Protected;
    using System;
    using System.Activities;
    using System.Collections.Generic;

    public abstract class CustomCodeActivityUnitTest<T>  where T : CustomCodeActivity
    {
        protected const string ExceptionJsonText = "ExceptionJson";
        protected const string ExceptionText = "Exception";
        protected const string TraceOutputText = "TraceOutput";

        protected IOrganizationServiceFactory OrganizationServiceFactory { get; set; }

        protected IOrganizationService CurrentUserService { get; set; }

        protected IOrganizationService SystemUserService { get; set; }

        protected ITracingService TracingService { get; set; }

        protected IWorkflowContext WorkflowContext { get; set; }

        protected IDictionary<string, object> CodeActivityInput { get; set; }

        protected IDictionary<string, object> CodeActivityOutput { get; set; }

        protected WorkflowInvoker WorkflowInvoker { get; set; }

        protected T CustomCodeActivity { get; set; }

        protected InvalidPluginExecutionException ExpectedException { get; set; }

        protected WorkflowExecutionContext CreateWorkflowExecutionContext()
        {
            WorkflowExecutionContext workflowExecutionContext = new WorkflowExecutionContext
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
                StageName = string.Empty,
                UserId = Guid.NewGuid()
            };

            return workflowExecutionContext;
        }

        protected WorkflowExecutionContext CreateWorkflowExecutionContext(Entity targetEntity, Entity preEntity, Entity postEntity)
        {
            WorkflowExecutionContext workflowExecutionContext = CreateWorkflowExecutionContext();
            if (targetEntity != null) workflowExecutionContext.InputParameters[PlatformConstants.TargetText] = targetEntity;
            if (preEntity != null) workflowExecutionContext.PreEntityImages[PlatformConstants.PreBusinessEntityText] = preEntity;
            if (postEntity != null) workflowExecutionContext.PostEntityImages[PlatformConstants.PostBusinessEntityText] = postEntity;
            return workflowExecutionContext;
        }

        protected WorkflowExecutionContext CreateWorkflowExecutionContext(string messageName, int depth, string primaryEntityName)
        {
            WorkflowExecutionContext workflowExecutionContext = CreateWorkflowExecutionContext();
            workflowExecutionContext.Depth = depth;
            workflowExecutionContext.MessageName = messageName;
            workflowExecutionContext.PrimaryEntityName = primaryEntityName;
            return workflowExecutionContext;
        }

        protected WorkflowExecutionContext CreateWorkflowExecutionContext(string messageName, int depth, string primaryEntityName, Entity targetEntity)
        {
            WorkflowExecutionContext workflowExecutionContext = CreateWorkflowExecutionContext(messageName, depth, primaryEntityName);
            workflowExecutionContext.InputParameters[PlatformConstants.TargetText] = targetEntity;
            return workflowExecutionContext;
        }

        protected WorkflowExecutionContext CreateWorkflowExecutionContext(string messageName, int depth, string primaryEntityName, Entity targetEntity, Entity preEntity, Entity postEntity)
        {
            WorkflowExecutionContext workflowExecutionContext = CreateWorkflowExecutionContext(messageName, depth, primaryEntityName, targetEntity);
            if (preEntity != null) workflowExecutionContext.PreEntityImages[PlatformConstants.PreBusinessEntityText] = preEntity;
            if (postEntity != null) workflowExecutionContext.PostEntityImages[PlatformConstants.PostBusinessEntityText] = postEntity;
            return workflowExecutionContext;
        }

        protected void InitializeUnitTest(WorkflowExecutionContext context, T customCodeActivity)
        {
            CustomCodeActivity = customCodeActivity;

            SystemUserService = Mock.Of<IOrganizationService>();
            CurrentUserService = Mock.Of<IOrganizationService>();
            TracingService = Mock.Of<ITracingService>();
            WorkflowContext = Mock.Of<IWorkflowContext>();
            CodeActivityInput = new Dictionary<string, object>();
            CodeActivityOutput = new Dictionary<string, object>();

            SetupMockResponseForWorkflowContext(context);
            SetupWorkflowInvoker(customCodeActivity);
            SetupMockForOrganizationServiceFactory();
        }

        protected void SetupMockResponseForWorkflowContext(WorkflowExecutionContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            var mock = new Mock<IWorkflowContext>().SetupAllProperties();

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
            mock.Setup(x => x.StageName).Returns(context.StageName);
            mock.Setup(x => x.UserId).Returns(context.UserId);
            mock.Setup(x => x.WorkflowCategory).Returns(context.WorkflowCategory);
            mock.Setup(x => x.WorkflowMode).Returns(context.WorkflowMode);

            WorkflowContext = mock.Object;
        }

        protected void SetupWorkflowInvoker(T codeActivity)
        {
            WorkflowInvoker = new WorkflowInvoker(codeActivity);
            WorkflowInvoker.Extensions.Add(() => OrganizationServiceFactory);
            WorkflowInvoker.Extensions.Add(() => TracingService);
            WorkflowInvoker.Extensions.Add(() => WorkflowContext);
        }

        protected void SetupMockForOrganizationServiceFactory()
        {
            var mock = new Mock<IOrganizationServiceFactory>().SetupAllProperties();

            mock.Setup(x => x.CreateOrganizationService(It.IsAny<Guid?>()))
                .Returns((Guid? x) => {
                    if (x == null)
                    {
                        return SystemUserService;
                    }
                    else
                    {
                        return CurrentUserService;
                    }
                });

            OrganizationServiceFactory = mock.Object;
        }

        public abstract void SetupMockObjectsForCustomCodeActivity();
    }
}