namespace Webhook.Plugins.Note.UnitTests
{
    using Aylos.Xrm.Sdk.Common;
    using Aylos.Xrm.Sdk.Core.Common;
    using Aylos.Xrm.Sdk.Core.WebhookPlugins.MoqTests;

    using Microsoft.Extensions.Logging;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Microsoft.Xrm.Sdk;
    using Microsoft.Xrm.Sdk.Query;

    using Moq;

    using Shared.Models.Domain;

    using System;
    using System.Globalization;
    using System.Net;

    using Webhook.Plugins.BusinessLogic.Services.Data;
    using Webhook.Plugins.BusinessLogic.Services.Note;

    using RemoteExecutionContext = Aylos.Xrm.Sdk.Core.WebhookPlugins.MoqTests.RemoteExecutionContext;

    [TestClass]
    public class HandleFileUploadUponCreateMoqUnitTest : DataverseWebhookPluginUnitTest<HandleFileUploadUponCreate>
    {
        #region Constants and Static Members

        const string _dummyText = "Dummy";
        const int _dummyNumber = -1;

        readonly Guid _regardingEntityId = Guid.NewGuid();
        const string _regardingEntityName = "entityname";
        
        Mock<CrmServiceContext>? _cscMock;
        Mock<CrmService>? _csMock;
        Mock<HttpClient>? _httpMock;
        Mock<FileHandlingService>? _iesMock;


        #endregion

        #region Setup Mocks & Stubs

        /// <summary>
        /// Sets up the mock objects of the derived unit test class
        /// </summary>
        protected override void SetupMockObjectsForService()
        {
            _cscMock = new Mock<CrmServiceContext>(DataverseWebhookPlugin.ServiceClient).SetupAllProperties();
            _csMock = new Mock<CrmService>(DataverseWebhookPlugin.ServiceClient, _cscMock.Object, DataverseWebhookPlugin.LoggerFactory).SetupAllProperties();
            DataverseWebhookPlugin.CrmService = _csMock.Object;

            _httpMock = new Mock<HttpClient>().SetupAllProperties();
            DataverseWebhookPlugin.HttpClient = _httpMock.Object;

            _iesMock = new Mock<FileHandlingService>(DataverseWebhookPlugin.HttpClient, DataverseWebhookPlugin.CrmService, DataverseWebhookPlugin.ServiceClient, DataverseWebhookPlugin.RemoteExecutionContext, DataverseWebhookPlugin.LoggerFactory).SetupAllProperties();
            _iesMock.Object.Logger = Mock.Of<ILogger>();
            DataverseWebhookPlugin.FileHandlingService = _iesMock.Object;
        }

        #endregion

        #region Setup Mock Responses

        #endregion

        #region Helper Methods

        private static Entity CreateNoteEntity(Guid id)
        {
            return CreateEntity(Note.EntityLogicalName, id);
        }

        private static Entity CreateEntity(string logicalName)
        {
            if (string.IsNullOrWhiteSpace(logicalName)) throw new ArgumentNullException(nameof(logicalName));

            return new Entity { LogicalName = logicalName };
        }

        private static Entity CreateEntity(string logicalName, Guid id)
        {
            if (Guid.Empty.Equals(id)) throw new ArgumentException(nameof(id));

            Entity entity = CreateEntity(logicalName);
            entity.Id = id;

            return entity;
        }

        #endregion

        #region Unit Tests

        /// <summary>
        /// Tests the exception thrown when a target annotation is not available in the webhook plugin execution context.
        /// </summary>
        [TestMethod]
        public async System.Threading.Tasks.Task HandleFileUploadUponCreate_ExceptionHandling_TargetEntityIsRequiredAsync()
        {
            #region ARRANGE

            // Setup the unit test mock context
            RemoteExecutionContext context = CreateRemoteExecutionContext();

            // Setup the HTTP request
            var headers = new Dictionary<string, string> { };

            // Initialize the unit test
            InitializeUnitTest(headers, context);

            // Setup the local mock objects
            SetupMockObjectsForService();

            #endregion
            #region ACT

            // Execute the webhook plugin step
            HttpResponseMessage res = await DataverseWebhookPlugin.Execute(DataverseWebhookPlugin.HttpRequestMessage);

            #endregion
            #region ASSERT

            Assert.IsNotNull(res);
            Assert.AreEqual(HttpStatusCode.InternalServerError, res.StatusCode);
            Assert.AreEqual(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.TargetEntityIsRequired, DataverseWebhookPlugin.GetType().Name), res.Content.ReadAsStringAsync().Result);

            #endregion
        }

        /// <summary>
        /// Tests the exception thrown when the target annotation is not the expected.
        /// </summary>
        [TestMethod]
        public async System.Threading.Tasks.Task HandleFileUploadUponCreate_ExceptionHandling_WrongRegisteredEntityAsync()
        {
            #region ARRANGE

            // Prepare the unit test mock target annotation
            Entity targetEntity = CreateEntity(_dummyText);

            // Setup the unit test mock context
            RemoteExecutionContext context = CreateRemoteExecutionContext(targetEntity);

            // Setup the HTTP request
            var headers = new Dictionary<string, string>
            {
                { HttpRequestHeaders.DynamicsEntityName, targetEntity.LogicalName }
            };

            // Initialize the unit test
            InitializeUnitTest(headers, context);

            // Setup the local mock objects
            SetupMockObjectsForService();

            #endregion
            #region ACT

            // Execute the webhook plugin step
            HttpResponseMessage res = await DataverseWebhookPlugin.Execute(DataverseWebhookPlugin.HttpRequestMessage);

            #endregion
            #region ASSERT

            Assert.IsNotNull(res);
            Assert.AreEqual(HttpStatusCode.InternalServerError, res.StatusCode);
            Assert.AreEqual(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.WrongRegisteredEntity, DataverseWebhookPlugin.GetType().Name, _dummyText), res.Content.ReadAsStringAsync().Result);

            #endregion
        }

        /// <summary>
        /// Tests the exception thrown when the webhook plugin message is not the expected.
        /// </summary>
        [TestMethod]
        public async System.Threading.Tasks.Task HandleFileUploadUponCreate_ExceptionHandling_IncorrectMessageNameAsync()
        {
            #region ARRANGE

            // Prepare the unit test mock target annotation
            Entity targetEntity = CreateNoteEntity(Guid.NewGuid());

            // Setup the unit test mock context
            RemoteExecutionContext context = CreateRemoteExecutionContext(_dummyText, _dummyNumber, targetEntity.LogicalName, _dummyNumber, targetEntity);

            // Setup the HTTP request
            var headers = new Dictionary<string, string>
            {
                { HttpRequestHeaders.DynamicsEntityName, targetEntity.LogicalName }
            };

            // Initialize the unit test
            InitializeUnitTest(headers, context);

            // Setup the local mock objects
            SetupMockObjectsForService();

            #endregion
            #region ACT

            // Execute the webhook plugin step
            HttpResponseMessage res = await DataverseWebhookPlugin.Execute(DataverseWebhookPlugin.HttpRequestMessage);

            #endregion
            #region ASSERT

            Assert.IsNotNull(res);
            Assert.AreEqual(HttpStatusCode.InternalServerError, res.StatusCode);
            Assert.AreEqual(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.IncorrectMessageName, DataverseWebhookPlugin.GetType().Name, PlatformMessageHelper.Create, _dummyText), res.Content.ReadAsStringAsync().Result);

            #endregion
        }

        /// <summary>
        /// Tests the exception thrown when the webhook plugin pipeline stage is not the expected.
        /// </summary>
        [TestMethod]
        public async System.Threading.Tasks.Task HandleFileUploadUponCreate_ExceptionHandling_IncorrectPipelineStageAsync()
        {
            #region ARRANGE

            // Prepare the unit test mock target annotation
            Entity targetEntity = CreateNoteEntity(Guid.NewGuid());

            // Setup the unit test mock context
            RemoteExecutionContext context = CreateRemoteExecutionContext(PlatformMessageHelper.Create, _dummyNumber, targetEntity.LogicalName, _dummyNumber, targetEntity);

            // Setup the HTTP request
            var headers = new Dictionary<string, string>
            {
                { HttpRequestHeaders.DynamicsEntityName, targetEntity.LogicalName }
            };

            // Initialize the unit test
            InitializeUnitTest(headers, context);

            // Setup the local mock objects
            SetupMockObjectsForService();

            #endregion
            #region ACT

            // Execute the webhook plugin step
            HttpResponseMessage res = await DataverseWebhookPlugin.Execute(DataverseWebhookPlugin.HttpRequestMessage);

            #endregion
            #region ASSERT

            Assert.IsNotNull(res);
            Assert.AreEqual(HttpStatusCode.InternalServerError, res.StatusCode);
            Assert.AreEqual(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.IncorrectPipelineStage, DataverseWebhookPlugin.GetType().Name, (int)SdkMessageProcessingStepStage.PreOperation, -1), res.Content.ReadAsStringAsync().Result);

            #endregion
        }

        /// <summary>
        /// Tests the exception thrown when the webhook plugin pipeline stage is not the expected.
        /// </summary>
        [TestMethod]
        public async System.Threading.Tasks.Task HandleFileUploadUponCreate_ExceptionHandling_MaxDepthViolationAsync()
        {
            #region ARRANGE

            // Prepare the unit test mock target annotation
            Entity targetEntity = CreateNoteEntity(Guid.NewGuid());

            // Setup the unit test mock context
            RemoteExecutionContext context = CreateRemoteExecutionContext(PlatformMessageHelper.Create,
                HandleFileUploadUponCreate.MaximumAllowedExecutionDepth + 1, targetEntity.LogicalName, 
                (int)SdkMessageProcessingStepStage.PreOperation, targetEntity);

            // Setup the HTTP request
            var headers = new Dictionary<string, string>
            {
                { HttpRequestHeaders.DynamicsEntityName, targetEntity.LogicalName }
            };

            // Initialize the unit test
            InitializeUnitTest(headers, context);

            // Setup the local mock objects
            SetupMockObjectsForService();

            #endregion
            #region ACT

            // Execute the webhook plugin step
            HttpResponseMessage res = await DataverseWebhookPlugin.Execute(DataverseWebhookPlugin.HttpRequestMessage);

            #endregion
            #region ASSERT

            Assert.IsNotNull(res);
            Assert.AreEqual(HttpStatusCode.InternalServerError, res.StatusCode);
            Assert.AreEqual(string.Format(CultureInfo.InvariantCulture,
                TraceMessageHelper.MaxDepthViolation, DataverseWebhookPlugin.GetType().Name, HandleFileUploadUponCreate.MaximumAllowedExecutionDepth,
                HandleFileUploadUponCreate.MaximumAllowedExecutionDepth + 1), res.Content.ReadAsStringAsync().Result);

            #endregion
        }

        /// <summary>
        /// Tests the behavior of the webhook plugin when the current annotation document body contains data.
        /// </summary>
        [TestMethod]
        public async System.Threading.Tasks.Task HandleFileUploadUponCreate_FileHandlingService_HandleFileUpload_CurrentNoteContainsDataAsync()
        {
            #region ARRANGE

            // Prepare the unit test mock target
            var annotation = new Note
            {
                Id = Guid.NewGuid(),
                CreatedOn = FileHandlingService.DateTimeNow,
                Document = _dummyText,
                FileName = _dummyText,
                IsDocument = true,
                MimeType = _dummyText,
                Regarding = new EntityReference(_regardingEntityName, _regardingEntityId),
            };

            // Setup the unit test mock context
            RemoteExecutionContext context = CreateRemoteExecutionContext(PlatformMessageHelper.Create, HandleFileUploadUponCreate.MaximumAllowedExecutionDepth, 
                annotation.LogicalName, (int)SdkMessageProcessingStepStage.PreOperation, annotation.ToEntity<Entity>());

            // Setup the HTTP request
            var headers = new Dictionary<string, string>
            {
                { HttpRequestHeaders.DynamicsEntityName, annotation.LogicalName }
            };

            // Initialize the unit test
            InitializeUnitTest(headers, context);

            // Setup the local mock objects
            SetupMockObjectsForService();

            // Stub the local mock objects
            _csMock?.Setup(x => x
                .GetEntityByKey(_regardingEntityId, _regardingEntityName, It.IsAny<ColumnSet>(), DataverseWebhookPlugin.RemoteExecutionContext.InitiatingUserId))
                .Returns(CreateEntity(_regardingEntityName, _regardingEntityId)).Verifiable();
            
            #endregion
            #region ACT

            // Execute the webhook plugin step
            HttpResponseMessage res = await DataverseWebhookPlugin.Execute(DataverseWebhookPlugin.HttpRequestMessage);

            #endregion
            #region ASSERT

            // Verifications
            _csMock?.Verify(x => x.GetEntityByKey(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<ColumnSet>(), It.IsAny<Guid>()), Times.Once());
            _iesMock?.Verify(x => x.SubmitFileToAV(It.IsAny<Note>()), Times.Once);
            _iesMock?.Verify(x => x.UploadFileToStorage(It.IsAny<Note>()), Times.Once);

            // Assertions
            Assert.IsNotNull(res);
            Assert.AreEqual(HttpStatusCode.OK, res.StatusCode);

            // Load the modified plugin execution context
            IPluginExecutionContext rec = DataverseWebhookPlugin.RemoteExecutionContext;
            Entity modifiedTargetEntity = (Entity)rec.InputParameters[PlatformConstants.TargetText];
            Note modifiedTarget = modifiedTargetEntity.ToEntity<Note>();

            // Assertions
            Assert.AreEqual(modifiedTarget.Document, _dummyText);

            #endregion
        }

        /// <summary>
        /// Tests the behavior of the webhook plugin when the current annotation document body is empty.
        /// </summary>
        [TestMethod]
        public async System.Threading.Tasks.Task HandleFileUploadUponCreate_FileHandlingService_HandleFileUpload_CurrentNoteIsEmptyAsync()
        {
            #region ARRANGE

            // Prepare the unit test mock target
            var annotation = new Note
            {
                Id = Guid.NewGuid(),
                CreatedOn = FileHandlingService.DateTimeNow,
                //Document = _dummyText,
                IsDocument = true,
            };

            // Setup the unit test mock context
            RemoteExecutionContext context = CreateRemoteExecutionContext(PlatformMessageHelper.Create, HandleFileUploadUponCreate.MaximumAllowedExecutionDepth,
                annotation.LogicalName, (int)SdkMessageProcessingStepStage.PreOperation, annotation.ToEntity<Entity>());

            // Setup the HTTP request
            var headers = new Dictionary<string, string>
            {
                { HttpRequestHeaders.DynamicsEntityName, annotation.LogicalName }
            };

            // Initialize the unit test
            InitializeUnitTest(headers, context);

            // Setup the local mock objects
            SetupMockObjectsForService();

            #endregion
            #region ACT

            // Execute the webhook plugin step
            HttpResponseMessage res = await DataverseWebhookPlugin.Execute(DataverseWebhookPlugin.HttpRequestMessage);

            #endregion
            #region ASSERT

            // Verifications
            _csMock?.Verify(x => x.GetEntityByKey(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<ColumnSet>(), It.IsAny<Guid>()), Times.Never);
            _iesMock?.Verify(x => x.SubmitFileToAV(It.IsAny<Note>()), Times.Never);
            _iesMock?.Verify(x => x.UploadFileToStorage(It.IsAny<Note>()), Times.Never);

            // Assertions
            Assert.IsNotNull(res);
            Assert.AreEqual(HttpStatusCode.InternalServerError, res.StatusCode);
            Assert.AreEqual(FileHandlingService.AnnotationContentIsEmpty, res.Content.ReadAsStringAsync().Result);

            #endregion
        }

        /// <summary>
        /// Tests the behavior of the webhook plugin when the current annotation is not a document.
        /// </summary>
        [TestMethod]
        public async System.Threading.Tasks.Task HandleFileUploadUponCreate_FileHandlingService_HandleFileUpload_CurrentNoteIsNotDocumentAsync()
        {
            #region ARRANGE

            // Prepare the unit test mock target
            var annotation = new Note
            {
                Id = Guid.NewGuid(),
                CreatedOn = FileHandlingService.DateTimeNow,
                IsDocument = false,
            };

            // Setup the unit test mock context
            RemoteExecutionContext context = CreateRemoteExecutionContext(PlatformMessageHelper.Create, HandleFileUploadUponCreate.MaximumAllowedExecutionDepth,
                annotation.LogicalName, (int)SdkMessageProcessingStepStage.PreOperation, annotation.ToEntity<Entity>());

            // Setup the HTTP request
            var headers = new Dictionary<string, string>
            {
                { HttpRequestHeaders.DynamicsEntityName, annotation.LogicalName }
            };

            // Initialize the unit test
            InitializeUnitTest(headers, context);

            // Setup the local mock objects
            SetupMockObjectsForService();

            #endregion
            #region ACT

            // Execute the webhook plugin step
            HttpResponseMessage res = await DataverseWebhookPlugin.Execute(DataverseWebhookPlugin.HttpRequestMessage);

            #endregion
            #region ASSERT

            // Verifications
            _csMock?.Verify(x => x.GetEntityByKey(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<ColumnSet>(), It.IsAny<Guid>()), Times.Never);
            _iesMock?.Verify(x => x.SubmitFileToAV(It.IsAny<Note>()), Times.Never);
            _iesMock?.Verify(x => x.UploadFileToStorage(It.IsAny<Note>()), Times.Never);

            // Assertions
            Assert.IsNotNull(res);
            Assert.AreEqual(HttpStatusCode.InternalServerError, res.StatusCode);
            Assert.AreEqual(FileHandlingService.AnnotationIsNotDocument, res.Content.ReadAsStringAsync().Result);

            #endregion
        }

        #endregion
    }
}
