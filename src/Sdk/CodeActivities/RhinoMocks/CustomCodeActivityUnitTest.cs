namespace Aylos.Xrm.Sdk.CodeActivities.RhinoMocks
{
    using Aylos.Xrm.Sdk.Common;
    using Microsoft.Xrm.Sdk;
    using Microsoft.Xrm.Sdk.Workflow;
    using Rhino.Mocks;
    using System;
    using System.Activities;
    using System.Collections.Generic;

    public abstract class CustomCodeActivityUnitTest<T>  where T : CustomCodeActivity
    {
        protected const string ExceptionJsonText = "ExceptionJson";
        protected const string ExceptionText = "Exception";
        protected const string TraceOutputText = "TraceOutput";

        protected IOrganizationServiceFactory OrganizationServiceFactory { get; set; }

        protected IOrganizationService OrganizationService { get; set; }

        protected IOrganizationService ImpersonatedOrganizationService { get; set; }

        protected ITracingService TracingService { get; set; }

        protected IWorkflowContext WorkflowContext { get; set; }

        protected IDictionary<string, object> CodeActivityInput { get; set; }

        protected IDictionary<string, object> CodeActivityOutput { get; set; }

        protected WorkflowInvoker WorkflowInvoker { get; set; }

        protected T CustomCodeActivity { get; set; }

        protected InvalidPluginExecutionException ExpectedException { get; set; }

        protected void InitializeUnitTest(WorkflowExecutionContext context, T customCodeActivity)
        {
            CustomCodeActivity = customCodeActivity;

            SetupMockObjects();
            SetupWorkflowInvoker(customCodeActivity);
            SetupMockResponseForWorkflowContext(context);
            SetupMockResponseForOrganizationServiceFactory();
        }

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

        protected void SetupMockObjects()
        {
            ImpersonatedOrganizationService = MockRepository.GenerateStub<IOrganizationService>();
            OrganizationService = MockRepository.GenerateStub<IOrganizationService>();
            OrganizationServiceFactory = MockRepository.GenerateStub<IOrganizationServiceFactory>();
            TracingService = MockRepository.GenerateStub<ITracingService>();
            WorkflowContext = MockRepository.GenerateStub<IWorkflowContext>();
            CodeActivityInput = new Dictionary<string, object>();
            CodeActivityOutput = new Dictionary<string, object>();
        }

        protected void SetupWorkflowInvoker(T codeActivity)
        {
            WorkflowInvoker = new WorkflowInvoker(codeActivity);
            WorkflowInvoker.Extensions.Add(() => OrganizationServiceFactory);
            WorkflowInvoker.Extensions.Add(() => TracingService);
            WorkflowInvoker.Extensions.Add(() => WorkflowContext);
        }

        protected void SetupMockResponseForWorkflowContext(WorkflowExecutionContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            WorkflowContext.Stub(x => x.BusinessUnitId).Return(context.BusinessUnitId);
            WorkflowContext.Stub(x => x.CorrelationId).Return(context.CorrelationId);
            WorkflowContext.Stub(x => x.Depth).Return(context.Depth);
            WorkflowContext.Stub(x => x.InitiatingUserId).Return(context.InitiatingUserId);
            WorkflowContext.Stub(x => x.InputParameters).Return(context.InputParameters);
            WorkflowContext.Stub(x => x.IsExecutingOffline).Return(context.IsExecutingOffline);
            WorkflowContext.Stub(x => x.IsInTransaction).Return(context.IsInTransaction);
            WorkflowContext.Stub(x => x.IsOfflinePlayback).Return(context.IsOfflinePlayback);
            WorkflowContext.Stub(x => x.IsolationMode).Return(context.IsolationMode);
            WorkflowContext.Stub(x => x.MessageName).Return(context.MessageName);
            WorkflowContext.Stub(x => x.Mode).Return(context.Mode);
            WorkflowContext.Stub(x => x.OperationCreatedOn).Return(context.OperationCreatedOn);
            WorkflowContext.Stub(x => x.OperationId).Return(context.OperationId);
            WorkflowContext.Stub(x => x.OrganizationId).Return(context.OrganizationId);
            WorkflowContext.Stub(x => x.OrganizationName).Return(context.OrganizationName);
            WorkflowContext.Stub(x => x.OutputParameters).Return(context.OutputParameters);
            WorkflowContext.Stub(x => x.OwningExtension).Return(context.OwningExtension);
            WorkflowContext.Stub(x => x.ParentContext).Return(context.ParentContext);
            WorkflowContext.Stub(x => x.PostEntityImages).Return(context.PostEntityImages);
            WorkflowContext.Stub(x => x.PreEntityImages).Return(context.PreEntityImages);
            WorkflowContext.Stub(x => x.PrimaryEntityId).Return(context.PrimaryEntityId);
            WorkflowContext.Stub(x => x.PrimaryEntityName).Return(context.PrimaryEntityName);
            WorkflowContext.Stub(x => x.RequestId).Return(context.RequestId);
            WorkflowContext.Stub(x => x.SecondaryEntityName).Return(context.SecondaryEntityName);
            WorkflowContext.Stub(x => x.SharedVariables).Return(context.SharedVariables);
            WorkflowContext.Stub(x => x.StageName).Return(context.StageName);
            WorkflowContext.Stub(x => x.UserId).Return(context.UserId);
            WorkflowContext.Stub(x => x.WorkflowCategory).Return(context.WorkflowCategory);
            WorkflowContext.Stub(x => x.WorkflowMode).Return(context.WorkflowMode);
        }

        protected void SetupMockResponseForOrganizationServiceFactory()
        {
            OrganizationServiceFactory.Stub(x => x.CreateOrganizationService(WorkflowContext.UserId)).Return(OrganizationService);
            OrganizationServiceFactory.Stub(x => x.CreateOrganizationService(null)).Return(ImpersonatedOrganizationService);
        }

        public abstract void SetupMockObjectsForCustomCodeActivity();
    }
}