namespace Aylos.Xrm.Sdk.Common
{
    using Aylos.Xrm.Sdk.Plugins;
    using System.Globalization;
    using System.Reflection;
    using Microsoft.Xrm.Sdk;
    using Microsoft.Xrm.Sdk.Client;
    using Microsoft.Xrm.Sdk.Workflow;
    using System;

    public abstract class GenericService
    {
        
        public IOrganizationService OrganizationService { get; set; }

        
        public IOrganizationService ImpersonatedOrganizationService { get; set; }

        
        public IWorkflowContext WorkflowContext { get; set; }

        public IPluginExecutionContext PluginExecutionContext { get; set; }

        public PluginExecutionContext ExecutionContext
        {
            get
            {
                return new PluginExecutionContext
                {
                    BusinessUnitId = PluginExecutionContext.BusinessUnitId,
                    CorrelationId = PluginExecutionContext.CorrelationId,
                    Depth = PluginExecutionContext.Depth,
                    InitiatingUserId = PluginExecutionContext.InitiatingUserId,
                    InputParameters = PluginExecutionContext.InputParameters,
                    IsExecutingOffline = PluginExecutionContext.IsExecutingOffline,
                    IsInTransaction = PluginExecutionContext.IsInTransaction,
                    IsOfflinePlayback = PluginExecutionContext.IsOfflinePlayback,
                    IsolationMode = PluginExecutionContext.IsolationMode,
                    MessageName = PluginExecutionContext.MessageName,
                    Mode = PluginExecutionContext.Mode,
                    OperationCreatedOn = PluginExecutionContext.OperationCreatedOn,
                    OperationId = PluginExecutionContext.OperationId,
                    OrganizationId = PluginExecutionContext.OrganizationId,
                    OrganizationName = PluginExecutionContext.OrganizationName,
                    OutputParameters = PluginExecutionContext.OutputParameters,
                    OwningExtension = PluginExecutionContext.OwningExtension,
                    PostEntityImages = PluginExecutionContext.PostEntityImages,
                    PreEntityImages = PluginExecutionContext.PreEntityImages,
                    PrimaryEntityId = PluginExecutionContext.PrimaryEntityId,
                    PrimaryEntityName = PluginExecutionContext.PrimaryEntityName,
                    RequestId = PluginExecutionContext.RequestId,
                    SecondaryEntityName = PluginExecutionContext.SecondaryEntityName,
                    SharedVariables = PluginExecutionContext.SharedVariables,
                    Stage = PluginExecutionContext.Stage,
                    UserId = PluginExecutionContext.UserId
                };
            }
        }

        public ITracingService TracingService { get; set; }

        public string UnderlyingSystemTypeName { get { return GetType().UnderlyingSystemType.Name; } }

        public void Trace(string message)
        {
            if (TracingService == null || string.IsNullOrWhiteSpace(message)) return;
            var utcNow = DateTime.UtcNow;
            TracingService.Trace(message + utcNow.ToString(" | HHmmss.fff", CultureInfo.InvariantCulture));
        }
    }

    public abstract class GenericService<T> : GenericService where T: OrganizationServiceContext
    {
        
        public T OrganizationServiceContext { get; set; }
    }

    public abstract class GenericService<T1, T2> : GenericService<T1>  where T1 : OrganizationServiceContext where T2 : Entity
    {
        Entity _targetEntity; Entity _preEntity; Entity _postEntity;
        EntityReference _targetEntityReference;
        Relationship _relationship; 
        EntityReferenceCollection _relatedEntities;
        EntityReference _leadEntityReference;
        T2 _targetBusinessEntity; T2 _preBusinessEntity; T2 _postBusinessEntity;

        public Entity TargetEntity
        {
            get
            {
                Trace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.EnteredMethod, UnderlyingSystemTypeName, MethodBase.GetCurrentMethod().Name));

                if (_targetEntity == null) _targetEntity = (Entity)PluginExecutionContext.InputParameters[PlatformConstants.TargetText];

                Trace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.ExitingMethod, UnderlyingSystemTypeName, MethodBase.GetCurrentMethod().Name));

                return _targetEntity;
            }
            set
            {
                Trace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.EnteredMethod, UnderlyingSystemTypeName, MethodBase.GetCurrentMethod().Name));

                _targetEntity = value;

                Trace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.ExitingMethod, UnderlyingSystemTypeName, MethodBase.GetCurrentMethod().Name));
            }
        }

        public EntityReference TargetEntityReference
        {
            get
            {
                Trace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.EnteredMethod, UnderlyingSystemTypeName, MethodBase.GetCurrentMethod().Name));

                if (_targetEntityReference == null) _targetEntityReference = (EntityReference)PluginExecutionContext.InputParameters[PlatformConstants.TargetEntityReferenceText];

                Trace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.ExitingMethod, UnderlyingSystemTypeName, MethodBase.GetCurrentMethod().Name));

                return _targetEntityReference;
            }
            set
            {
                Trace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.EnteredMethod, UnderlyingSystemTypeName, MethodBase.GetCurrentMethod().Name));

                _targetEntityReference = value;

                Trace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.ExitingMethod, UnderlyingSystemTypeName, MethodBase.GetCurrentMethod().Name));
            }
        }

        public Relationship Relationship
        {
            get
            {
                Trace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.EnteredMethod, UnderlyingSystemTypeName, MethodBase.GetCurrentMethod().Name));

                if (_relationship == null) _relationship = (Relationship)PluginExecutionContext.InputParameters[PlatformConstants.RelationshipText];

                Trace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.ExitingMethod, UnderlyingSystemTypeName, MethodBase.GetCurrentMethod().Name));

                return _relationship;
            }
            set
            {
                Trace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.EnteredMethod, UnderlyingSystemTypeName, MethodBase.GetCurrentMethod().Name));

                _relationship = value;

                Trace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.ExitingMethod, UnderlyingSystemTypeName, MethodBase.GetCurrentMethod().Name));
            }
        }

        public EntityReferenceCollection RelatedEntities
        {
            get
            {
                Trace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.EnteredMethod, UnderlyingSystemTypeName, MethodBase.GetCurrentMethod().Name));

                if (_relatedEntities == null) _relatedEntities = (EntityReferenceCollection)PluginExecutionContext.InputParameters[PlatformConstants.RelatedEntitiesText];

                Trace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.ExitingMethod, UnderlyingSystemTypeName, MethodBase.GetCurrentMethod().Name));

                return _relatedEntities;
            }
        }

        public EntityReference LeadEntityReference
        {
            get
            {
                Trace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.EnteredMethod, UnderlyingSystemTypeName, MethodBase.GetCurrentMethod().Name));

                if (_leadEntityReference == null) _leadEntityReference = (EntityReference)PluginExecutionContext.InputParameters[PlatformConstants.LeadIdText];

                Trace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.ExitingMethod, UnderlyingSystemTypeName, MethodBase.GetCurrentMethod().Name));

                return _leadEntityReference;
            }
            set
            {
                Trace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.EnteredMethod, UnderlyingSystemTypeName, MethodBase.GetCurrentMethod().Name));

                _leadEntityReference = value;

                Trace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.ExitingMethod, UnderlyingSystemTypeName, MethodBase.GetCurrentMethod().Name));
            }
        }

        public Entity PreEntity
        {
            get
            {
                Trace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.EnteredMethod, UnderlyingSystemTypeName, MethodBase.GetCurrentMethod().Name));

                if ((_preEntity == null) && (PluginExecutionContext != null))
                {
                    _preEntity = PluginExecutionContext.PreEntityImages[PlatformConstants.PreBusinessEntityText];
                }
                else if ((_preEntity == null) && (WorkflowContext != null))
                {
                    _preEntity = WorkflowContext.PreEntityImages[PlatformConstants.PreBusinessEntityText];
                }

                Trace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.ExitingMethod, UnderlyingSystemTypeName, MethodBase.GetCurrentMethod().Name));

                return _preEntity;
            }
            set
            {
                Trace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.EnteredMethod, UnderlyingSystemTypeName, MethodBase.GetCurrentMethod().Name));

                _preEntity = value;

                Trace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.ExitingMethod, UnderlyingSystemTypeName, MethodBase.GetCurrentMethod().Name));
            }
        }

        public Entity PostEntity
        {
            get
            {
                Trace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.EnteredMethod, UnderlyingSystemTypeName, MethodBase.GetCurrentMethod().Name));

                if ((_postEntity == null) && (PluginExecutionContext != null))
                {
                    _postEntity = PluginExecutionContext.PostEntityImages[PlatformConstants.PostBusinessEntityText];
                }
                else if ((_postEntity == null) && (WorkflowContext != null))
                {
                    _postEntity = WorkflowContext.PostEntityImages[PlatformConstants.PostBusinessEntityText];
                }

                Trace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.ExitingMethod, UnderlyingSystemTypeName, MethodBase.GetCurrentMethod().Name));

                return _postEntity;
            }
            set
            {
                Trace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.EnteredMethod, UnderlyingSystemTypeName, MethodBase.GetCurrentMethod().Name));

                _postEntity = value;

                Trace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.ExitingMethod, UnderlyingSystemTypeName, MethodBase.GetCurrentMethod().Name));
            }
        }

        public T2 TargetBusinessEntity
        {
            get
            {
                Trace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.EnteredMethod, UnderlyingSystemTypeName, MethodBase.GetCurrentMethod().Name));

                if (_targetBusinessEntity == null) _targetBusinessEntity = TargetEntity.ToEntity<T2>();

                Trace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.ExitingMethod, UnderlyingSystemTypeName, MethodBase.GetCurrentMethod().Name));

                return _targetBusinessEntity;
            }
            set
            {
                Trace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.EnteredMethod, UnderlyingSystemTypeName, MethodBase.GetCurrentMethod().Name));

                _targetBusinessEntity = value;

                Trace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.ExitingMethod, UnderlyingSystemTypeName, MethodBase.GetCurrentMethod().Name));
            }
        }

        public T2 PreBusinessEntity
        {
            get
            {
                Trace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.EnteredMethod, UnderlyingSystemTypeName, MethodBase.GetCurrentMethod().Name));

                if (_preBusinessEntity == null)
                {
                    _preBusinessEntity = PreEntity.ToEntity<T2>();
                }

                Trace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.ExitingMethod, UnderlyingSystemTypeName, MethodBase.GetCurrentMethod().Name));

                return _preBusinessEntity;
            }
            set
            {
                Trace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.EnteredMethod, UnderlyingSystemTypeName, MethodBase.GetCurrentMethod().Name));

                _preBusinessEntity = value;

                Trace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.ExitingMethod, UnderlyingSystemTypeName, MethodBase.GetCurrentMethod().Name));
            }
        }

        public T2 PostBusinessEntity
        {
            get
            {
                Trace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.EnteredMethod, UnderlyingSystemTypeName, MethodBase.GetCurrentMethod().Name));

                if (_postBusinessEntity == null) _postBusinessEntity = PostEntity.ToEntity<T2>();

                Trace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.ExitingMethod, UnderlyingSystemTypeName, MethodBase.GetCurrentMethod().Name));

                return _postBusinessEntity;
            }
            set
            {
                Trace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.EnteredMethod, UnderlyingSystemTypeName, MethodBase.GetCurrentMethod().Name));

                _postBusinessEntity = value;

                Trace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.ExitingMethod, UnderlyingSystemTypeName, MethodBase.GetCurrentMethod().Name));
            }
        }
    }
}
