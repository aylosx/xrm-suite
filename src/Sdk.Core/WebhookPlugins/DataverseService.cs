namespace Aylos.Xrm.Sdk.Core.WebhookPlugins
{
    using Aylos.Xrm.Sdk.Common;

    using Microsoft.Azure.WebJobs.Logging;
    using Microsoft.Extensions.Logging;
    using Microsoft.PowerPlatform.Dataverse.Client;
    using Microsoft.Xrm.Sdk;

    using System;
    using System.Globalization;
    using System.Linq;
    using System.Net.Http;
    using System.Reflection;

    public abstract class DataverseService
    {
        #region Constructor

        public DataverseService(ServiceClient serviceClient, RemoteExecutionContext remoteExecutionContext, ILoggerFactory loggerFactory)
        {
            ServiceClient = serviceClient ?? throw new ArgumentNullException(nameof(serviceClient));

            RemoteExecutionContext = remoteExecutionContext ?? throw new ArgumentNullException(nameof(remoteExecutionContext));

            ServiceClient.SessionTrackingId = RemoteExecutionContext.CorrelationId;

            LoggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));

            Logger = loggerFactory.CreateLogger(LogCategories.CreateFunctionUserCategory(UnderlyingSystemTypeName));
        }

        #endregion

        #region Properties

        public static ILogger Logger { get; private set; }

        public ILoggerFactory LoggerFactory { get; private set; }

        public ServiceClient ServiceClient { get; private set; }

        public RemoteExecutionContext RemoteExecutionContext { get; private set; }

        public HttpRequestMessage HttpRequestMessage { get; protected set; }

        public string HttpMessageOrganisationName
        {
            get
            {
                string ret = null;
                if (HttpRequestMessage.Headers.Contains("x-ms-dynamics-organization"))
                {
                    ret = HttpRequestMessage.Headers.GetValues("x-ms-dynamics-organization").SingleOrDefault();
                }
                return ret;
            }
        }

        public string HttpMessageEntityName
        {
            get
            {
                string ret = null;
                if (HttpRequestMessage.Headers.Contains("x-ms-dynamics-entity-name"))
                {
                    ret = HttpRequestMessage.Headers.GetValues("x-ms-dynamics-entity-name").SingleOrDefault();
                }
                return ret;
            }
        }

        public string HttpMessageEventName
        {
            get
            {
                string ret = null;
                if (HttpRequestMessage.Headers.Contains("x-ms-dynamics-request-name"))
                {
                    ret = HttpRequestMessage.Headers.GetValues("x-ms-dynamics-request-name").SingleOrDefault();
                }
                return ret;
            }
        }

        public Guid? HttpMessageCorrelationId
        {
            get
            {
                Guid? ret = null;
                if (HttpRequestMessage.Headers.Contains("x-ms-dynamics-request-name"))
                {
                    ret = Guid.Parse(HttpRequestMessage.Headers.GetValues("x-ms-correlation-request-id").SingleOrDefault());
                }
                return ret;
            }
        }

        public bool HttpMessageSizeExceeded => HttpRequestMessage.Headers.Contains("x-ms-dynamics-msg-size-exceeded");

        public string UnderlyingSystemTypeName { get { return GetType().UnderlyingSystemType.Name; } }

        #endregion
    }

    public abstract class DataverseService<T> : DataverseService where T : Entity
    {
        #region Constructor

        public DataverseService(ServiceClient serviceClient, RemoteExecutionContext remoteExecutionContext, ILoggerFactory loggerFactory) 
            : base(serviceClient, remoteExecutionContext, loggerFactory)
        {

        }

        #endregion

        #region Properties

        Entity _targetEntity; Entity _preEntity; Entity _postEntity;
        EntityReference _targetEntityReference;
        Relationship _relationship;
        EntityReferenceCollection _relatedEntities;
        EntityReference _leadEntityReference;
        T _targetBusinessEntity; T _preBusinessEntity; T _postBusinessEntity;

        public Entity TargetEntity
        {
            get
            {
                Logger.LogTrace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.EnteredMethod, UnderlyingSystemTypeName, MethodBase.GetCurrentMethod().Name));

                if (_targetEntity == null && !HttpMessageSizeExceeded)
                {
                    _targetEntity = (Entity)RemoteExecutionContext.InputParameters[PlatformConstants.TargetText];
                }

                Logger.LogTrace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.ExitingMethod, UnderlyingSystemTypeName, MethodBase.GetCurrentMethod().Name));

                return _targetEntity;
            }
            set
            {
                Logger.LogTrace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.EnteredMethod, UnderlyingSystemTypeName, MethodBase.GetCurrentMethod().Name));

                _targetEntity = value;

                Logger.LogTrace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.ExitingMethod, UnderlyingSystemTypeName, MethodBase.GetCurrentMethod().Name));
            }
        }

        public EntityReference TargetEntityReference
        {
            get
            {
                Logger.LogTrace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.EnteredMethod, UnderlyingSystemTypeName, MethodBase.GetCurrentMethod().Name));

                if (_targetEntityReference == null && !HttpMessageSizeExceeded)
                {
                    _targetEntityReference = (EntityReference)RemoteExecutionContext.InputParameters[PlatformConstants.TargetEntityReferenceText];
                }

                Logger.LogTrace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.ExitingMethod, UnderlyingSystemTypeName, MethodBase.GetCurrentMethod().Name));

                return _targetEntityReference;
            }
            set
            {
                Logger.LogTrace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.EnteredMethod, UnderlyingSystemTypeName, MethodBase.GetCurrentMethod().Name));

                _targetEntityReference = value;

                Logger.LogTrace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.ExitingMethod, UnderlyingSystemTypeName, MethodBase.GetCurrentMethod().Name));
            }
        }

        public Relationship Relationship
        {
            get
            {
                Logger.LogTrace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.EnteredMethod, UnderlyingSystemTypeName, MethodBase.GetCurrentMethod().Name));

                if (_relationship == null && !HttpMessageSizeExceeded)
                {
                    _relationship = (Relationship)RemoteExecutionContext.InputParameters[PlatformConstants.RelationshipText];
                }

                Logger.LogTrace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.ExitingMethod, UnderlyingSystemTypeName, MethodBase.GetCurrentMethod().Name));

                return _relationship;
            }
            set
            {
                Logger.LogTrace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.EnteredMethod, UnderlyingSystemTypeName, MethodBase.GetCurrentMethod().Name));

                _relationship = value;

                Logger.LogTrace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.ExitingMethod, UnderlyingSystemTypeName, MethodBase.GetCurrentMethod().Name));
            }
        }

        public EntityReferenceCollection RelatedEntities
        {
            get
            {
                Logger.LogTrace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.EnteredMethod, UnderlyingSystemTypeName, MethodBase.GetCurrentMethod().Name));

                if (_relatedEntities == null && !HttpMessageSizeExceeded)
                {
                    _relatedEntities = (EntityReferenceCollection)RemoteExecutionContext.InputParameters[PlatformConstants.RelatedEntitiesText];
                }

                Logger.LogTrace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.ExitingMethod, UnderlyingSystemTypeName, MethodBase.GetCurrentMethod().Name));

                return _relatedEntities;
            }
        }

        public EntityReference LeadEntityReference
        {
            get
            {
                Logger.LogTrace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.EnteredMethod, UnderlyingSystemTypeName, MethodBase.GetCurrentMethod().Name));

                if (_leadEntityReference == null && !HttpMessageSizeExceeded)
                {
                    _leadEntityReference = (EntityReference)RemoteExecutionContext.InputParameters[PlatformConstants.LeadIdText];
                }

                Logger.LogTrace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.ExitingMethod, UnderlyingSystemTypeName, MethodBase.GetCurrentMethod().Name));

                return _leadEntityReference;
            }
            set
            {
                Logger.LogTrace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.EnteredMethod, UnderlyingSystemTypeName, MethodBase.GetCurrentMethod().Name));

                _leadEntityReference = value;

                Logger.LogTrace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.ExitingMethod, UnderlyingSystemTypeName, MethodBase.GetCurrentMethod().Name));
            }
        }

        public Entity PreEntity
        {
            get
            {
                Logger.LogTrace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.EnteredMethod, UnderlyingSystemTypeName, MethodBase.GetCurrentMethod().Name));

                if (_preEntity == null && !HttpMessageSizeExceeded)
                {
                    _preEntity = RemoteExecutionContext?.PreEntityImages[PlatformConstants.PreBusinessEntityText];
                }

                Logger.LogTrace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.ExitingMethod, UnderlyingSystemTypeName, MethodBase.GetCurrentMethod().Name));

                return _preEntity;
            }
            set
            {
                Logger.LogTrace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.EnteredMethod, UnderlyingSystemTypeName, MethodBase.GetCurrentMethod().Name));

                _preEntity = value;

                Logger.LogTrace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.ExitingMethod, UnderlyingSystemTypeName, MethodBase.GetCurrentMethod().Name));
            }
        }

        public Entity PostEntity
        {
            get
            {
                Logger.LogTrace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.EnteredMethod, UnderlyingSystemTypeName, MethodBase.GetCurrentMethod().Name));

                if (_postEntity == null && !HttpMessageSizeExceeded)
                {
                    _postEntity = RemoteExecutionContext?.PostEntityImages[PlatformConstants.PostBusinessEntityText];
                }

                Logger.LogTrace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.ExitingMethod, UnderlyingSystemTypeName, MethodBase.GetCurrentMethod().Name));

                return _postEntity;
            }
            set
            {
                Logger.LogTrace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.EnteredMethod, UnderlyingSystemTypeName, MethodBase.GetCurrentMethod().Name));

                _postEntity = value;

                Logger.LogTrace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.ExitingMethod, UnderlyingSystemTypeName, MethodBase.GetCurrentMethod().Name));
            }
        }

        public T TargetBusinessEntity
        {
            get
            {
                Logger.LogTrace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.EnteredMethod, UnderlyingSystemTypeName, MethodBase.GetCurrentMethod().Name));

                _targetBusinessEntity ??= TargetEntity?.ToEntity<T>();

                Logger.LogTrace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.ExitingMethod, UnderlyingSystemTypeName, MethodBase.GetCurrentMethod().Name));

                return _targetBusinessEntity;
            }
            set
            {
                Logger.LogTrace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.EnteredMethod, UnderlyingSystemTypeName, MethodBase.GetCurrentMethod().Name));

                _targetBusinessEntity = value;

                Logger.LogTrace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.ExitingMethod, UnderlyingSystemTypeName, MethodBase.GetCurrentMethod().Name));
            }
        }

        public T PreBusinessEntity
        {
            get
            {
                Logger.LogTrace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.EnteredMethod, UnderlyingSystemTypeName, MethodBase.GetCurrentMethod().Name));

                _preBusinessEntity ??= PreEntity?.ToEntity<T>();

                Logger.LogTrace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.ExitingMethod, UnderlyingSystemTypeName, MethodBase.GetCurrentMethod().Name));

                return _preBusinessEntity;
            }
            set
            {
                Logger.LogTrace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.EnteredMethod, UnderlyingSystemTypeName, MethodBase.GetCurrentMethod().Name));

                _preBusinessEntity = value;

                Logger.LogTrace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.ExitingMethod, UnderlyingSystemTypeName, MethodBase.GetCurrentMethod().Name));
            }
        }

        public T PostBusinessEntity
        {
            get
            {
                Logger.LogTrace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.EnteredMethod, UnderlyingSystemTypeName, MethodBase.GetCurrentMethod().Name));

                _postBusinessEntity ??= PostEntity?.ToEntity<T>();

                Logger.LogTrace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.ExitingMethod, UnderlyingSystemTypeName, MethodBase.GetCurrentMethod().Name));

                return _postBusinessEntity;
            }
            set
            {
                Logger.LogTrace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.EnteredMethod, UnderlyingSystemTypeName, MethodBase.GetCurrentMethod().Name));

                _postBusinessEntity = value;

                Logger.LogTrace(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.ExitingMethod, UnderlyingSystemTypeName, MethodBase.GetCurrentMethod().Name));
            }
        }

        #endregion
    }
}
