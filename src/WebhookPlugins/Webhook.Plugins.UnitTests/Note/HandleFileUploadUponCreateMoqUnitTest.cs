namespace Webhook.Plugins.Note.UnitTests
{
    using Aylos.Xrm.Sdk.Common;
    using Aylos.Xrm.Sdk.Core.Common;
    using Aylos.Xrm.Sdk.Core.WebhookPlugins.MoqTests;

    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Microsoft.Xrm.Sdk;

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

        #endregion

        #region Setup Mocks & Stubs

        /// <summary>
        /// Sets up the mock objects of the derived unit test class
        /// </summary>
        protected override void SetupMockObjectsForPlugin()
        {
            var cscMock = new Mock<CrmServiceContext>(DataverseWebhookPlugin.ServiceClient).SetupAllProperties();

            var csMock = new Mock<CrmService>(DataverseWebhookPlugin.ServiceClient, cscMock.Object, DataverseWebhookPlugin.LoggerFactory).SetupAllProperties();
            DataverseWebhookPlugin.CrmService = csMock.Object;

            var httpMock = new Mock<HttpClient>().SetupAllProperties();
            DataverseWebhookPlugin.HttpClient = httpMock.Object;

            var iesMock = new Mock<FileHandlingService>(DataverseWebhookPlugin.HttpClient, DataverseWebhookPlugin.CrmService, DataverseWebhookPlugin.ServiceClient, DataverseWebhookPlugin.RemoteExecutionContext, DataverseWebhookPlugin.LoggerFactory).SetupAllProperties();
            DataverseWebhookPlugin.FileHandlingService = iesMock.Object;
        }

        #endregion

        #region Setup Mock Responses

        #endregion

        #region Helper Methods

        private static Note CreateNote(string base64body)
        {
            var annotation = new Note
            {
                Id = Guid.NewGuid(),
                CreatedOn = FileHandlingService.DateTimeNow
            };
            if (!string.IsNullOrWhiteSpace(base64body)) 
            {
                annotation.Document = base64body;
            }
            return annotation;
        }

        private static Entity CreateNoteEntity()
        {
            return new Entity
            {
                LogicalName = Note.EntityLogicalName
            };
        }

        private static Entity CreateNoteEntity(Note annotation)
        {
            return annotation.ToEntity<Entity>();
        }

        #endregion

        #region Unit Tests

        /// <summary>
        /// Tests the exception thrown when a target annotation is not available in the plugin execution context.
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

            // Setup the plugin mock objects
            SetupMockObjectsForPlugin();

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
            Entity targetEntity = new Entity { LogicalName = _dummyText };

            // Setup the unit test mock context
            RemoteExecutionContext context = CreateRemoteExecutionContext(targetEntity);

            // Setup the HTTP request
            var headers = new Dictionary<string, string>
            {
                { HttpRequestHeaders.DynamicsEntityName, targetEntity.LogicalName }
            };

            // Initialize the unit test
            InitializeUnitTest(headers, context);

            // Setup the plugin mock objects
            SetupMockObjectsForPlugin();

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
        /// Tests the exception thrown when the plugin message is not the expected.
        /// </summary>
        [TestMethod]
        public async System.Threading.Tasks.Task HandleFileUploadUponCreate_ExceptionHandling_IncorrectMessageNameAsync()
        {
            #region ARRANGE

            // Prepare the unit test mock target annotation
            Entity targetEntity = CreateNoteEntity();

            // Setup the unit test mock context
            RemoteExecutionContext context = CreateRemoteExecutionContext(_dummyText, _dummyNumber, 
                targetEntity.LogicalName, _dummyNumber, targetEntity);

            // Setup the HTTP request
            var headers = new Dictionary<string, string>
            {
                { HttpRequestHeaders.DynamicsEntityName, targetEntity.LogicalName }
            };

            // Initialize the unit test
            InitializeUnitTest(headers, context);

            // Setup the plugin mock objects
            SetupMockObjectsForPlugin();

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
        /// Tests the exception thrown when the plugin pipeline stage is not the expected.
        /// </summary>
        [TestMethod]
        public async System.Threading.Tasks.Task HandleFileUploadUponCreate_ExceptionHandling_IncorrectPipelineStageAsync()
        {
            #region ARRANGE

            // Prepare the unit test mock target annotation
            Entity targetEntity = CreateNoteEntity();

            // Setup the unit test mock context
            RemoteExecutionContext context = CreateRemoteExecutionContext(PlatformMessageHelper.Create, _dummyNumber, 
                targetEntity.LogicalName, _dummyNumber, targetEntity);

            // Setup the HTTP request
            var headers = new Dictionary<string, string>
            {
                { HttpRequestHeaders.DynamicsEntityName, targetEntity.LogicalName }
            };

            // Initialize the unit test
            InitializeUnitTest(headers, context);

            // Setup the plugin mock objects
            SetupMockObjectsForPlugin();

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
        /// Tests the exception thrown when the plugin pipeline stage is not the expected.
        /// </summary>
        [TestMethod]
        public async System.Threading.Tasks.Task HandleFileUploadUponCreate_ExceptionHandling_MaxDepthViolationAsync()
        {
            #region ARRANGE

            // Prepare the unit test mock target annotation
            Entity targetEntity = CreateNoteEntity();

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

            // Setup the plugin mock objects
            SetupMockObjectsForPlugin();

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
        /// Tests the behavior of the plugin when the current annotation document body contains data.
        /// </summary>
        [TestMethod]
        public async System.Threading.Tasks.Task HandleFileUploadUponCreate_FileHandlingService_InitializeEntity_CurrentNoteNumberContainsDataAsync()
        {
            #region ARRANGE

            // Prepare the unit test mock target annotation
            Note annotation = CreateNote(_dummyText);
            Entity targetEntity = CreateNoteEntity(annotation);

            // Setup the unit test mock context
            RemoteExecutionContext context = CreateRemoteExecutionContext(PlatformMessageHelper.Create,
                HandleFileUploadUponCreate.MaximumAllowedExecutionDepth, targetEntity.LogicalName,
                (int)SdkMessageProcessingStepStage.PreOperation, targetEntity);

            // Setup the HTTP request
            var headers = new Dictionary<string, string>
            {
                { HttpRequestHeaders.DynamicsEntityName, targetEntity.LogicalName }
            };

            // Initialize the unit test
            InitializeUnitTest(headers, context);

            // Setup the plugin mock objects
            SetupMockObjectsForPlugin();

            #endregion
            #region ACT

            // Execute the webhook plugin step
            HttpResponseMessage res = await DataverseWebhookPlugin.Execute(DataverseWebhookPlugin.HttpRequestMessage);

            #endregion
            #region ASSERT

            // Load the modified plugin execution context
            IPluginExecutionContext rec = DataverseWebhookPlugin.RemoteExecutionContext;
            Entity modifiedTargetEntity = (Entity)rec.InputParameters[PlatformConstants.TargetText];
            Note modifiedTarget = modifiedTargetEntity.ToEntity<Note>();

            // Assertions
            Assert.AreEqual(modifiedTarget.Document, _dummyText);

            #endregion
        }

        /// <summary>
        /// Tests the behavior of the plugin when the current annotation document body is empty.
        /// </summary>
        [TestMethod]
        public async System.Threading.Tasks.Task HandleFileUploadUponCreate_FileHandlingService_InitializeEntity_CurrentNoteNumberIsEmptyAsync()
        {
            #region ARRANGE

            // Prepare the unit test mock target annotation
            Note annotation = CreateNote(string.Empty);
            Entity targetEntity = CreateNoteEntity(annotation);

            // Setup the unit test mock context
            RemoteExecutionContext context = CreateRemoteExecutionContext(PlatformMessageHelper.Create,
                HandleFileUploadUponCreate.MaximumAllowedExecutionDepth, targetEntity.LogicalName,
                (int)SdkMessageProcessingStepStage.PreOperation, targetEntity);

            // Setup the HTTP request
            var headers = new Dictionary<string, string>
            {
                { HttpRequestHeaders.DynamicsEntityName, targetEntity.LogicalName }
            };

            // Initialize the unit test
            InitializeUnitTest(headers, context);

            // Setup the plugin mock objects
            SetupMockObjectsForPlugin();

            #endregion
            #region ACT

            // Execute the webhook plugin step
            HttpResponseMessage res = await DataverseWebhookPlugin.Execute(DataverseWebhookPlugin.HttpRequestMessage);

            #endregion
            #region ASSERT

            // Load the modified plugin execution context
            IPluginExecutionContext rec = DataverseWebhookPlugin.RemoteExecutionContext;
            Entity modifiedTargetEntity = (Entity)rec.InputParameters[PlatformConstants.TargetText];
            Note modifiedTarget = modifiedTargetEntity.ToEntity<Note>();

            // Assertions
            Assert.AreNotEqual(modifiedTarget.Document, string.Empty);
            /*
            Assert.AreEqual(modifiedTarget.Document, string.Format(CultureInfo.InvariantCulture, 
                FileHandlingService.TextFormat, modifiedTarget.CreatedOn.Value.ToString(FileHandlingService.DateTimeFormat, 
                CultureInfo.InvariantCulture)));
            */
            #endregion
        }

        #endregion
    }
}
