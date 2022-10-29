namespace Plugins.Note.UnitTest
{
    using System;
    using System.Globalization;

    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Microsoft.Xrm.Sdk;

    using Moq;

    using Aylos.Xrm.Sdk.Common;
    using Aylos.Xrm.Sdk.Plugins;
    using Aylos.Xrm.Sdk.Plugins.MoqTests;

    using Note;
    using Shared.Models.Domain;
    using Shared.BusinessLogic.Services.Note;
    using Shared.BusinessLogic.Services.Data;

    [TestClass]
    public class InitializeEntityUponCreateMoqUnitTest : CustomPluginUnitTest<InitializeEntityUponCreate>
    {
        #region Constants and Static Members

        const string _dummyText = "Dummy";
        const int _dummyNumber = -1;

        #endregion

        #region Setup Mocks & Stubs

        /// <summary>
        /// Sets up the mock objects of the derived unit test class
        /// </summary>
        public override void SetupMockObjectsForPlugin()
        {
            var cscMock = new Mock<CrmServiceContext>(Plugin.CurrentUserService).SetupAllProperties();
            Plugin.OrganizationServiceContext = cscMock.Object;

            var csMock = new Mock<CrmService>(Plugin.OrganizationServiceContext, Plugin.TracingService).SetupAllProperties();
            Plugin.CrmService = csMock.Object;

            var iesMock = new Mock<InitializeEntityService>(Plugin.CrmService, Plugin.OrganizationServiceContext, Plugin.PluginExecutionContext, Plugin.TracingService).SetupAllProperties();
            Plugin.InitializeEntityService = iesMock.Object;
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
                CreatedOn = InitializeEntityService.DateTimeNow
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
        public void InitializeEntityUponCreate_ExceptionHandling_TargetEntityIsRequired()
        {
            #region ARRANGE

            // Setup the unit test mock context
            PluginExecutionContext context = CreatePluginExecutionContext();

            // Initialize the unit test
            InitializeUnitTest(context);

            // Setup the plugin mock objects
            SetupMockObjectsForPlugin();

            #endregion
            #region ACT

            try
            {
                // Execute the plugin step
                Plugin.Execute(Plugin.ServiceProvider);
            }
            catch (InvalidPluginExecutionException ex)
            {
                ActualException = ex;
            }

            #endregion
            #region ASSERT

            Assert.IsNotNull(ActualException);
            Assert.AreEqual(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.TargetEntityIsRequired, Plugin.GetType().Name), ActualException.Message);

            #endregion
        }

        /// <summary>
        /// Tests the exception thrown when the target annotation is not the expected.
        /// </summary>
        [TestMethod]
        public void InitializeEntityUponCreate_ExceptionHandling_WrongRegisteredEntity()
        {
            #region ARRANGE

            // Prepare the unit test mock target annotation
            Entity targetEntity = new Entity { LogicalName = _dummyText };

            // Setup the unit test mock context
            PluginExecutionContext context = CreatePluginExecutionContext(targetEntity);

            // Initialize the unit test
            InitializeUnitTest(context);

            // Setup the plugin mock objects
            SetupMockObjectsForPlugin();

            #endregion
            #region ACT

            try
            {
                // Execute the plugin step
                Plugin.Execute(Plugin.ServiceProvider);
            }
            catch (InvalidPluginExecutionException ex)
            {
                ActualException = ex;
            }

            #endregion
            #region ASSERT

            Assert.IsNotNull(ActualException);
            Assert.AreEqual(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.WrongRegisteredEntity, Plugin.GetType().Name, _dummyText), ActualException.Message);

            #endregion
        }

        /// <summary>
        /// Tests the exception thrown when the plugin message is not the expected.
        /// </summary>
        [TestMethod]
        public void InitializeEntityUponCreate_ExceptionHandling_IncorrectMessageName()
        {
            #region ARRANGE

            // Prepare the unit test mock target annotation
            Entity targetEntity = CreateNoteEntity();

            // Setup the unit test mock context
            PluginExecutionContext context = CreatePluginExecutionContext(_dummyText, 
                _dummyNumber, targetEntity.LogicalName, _dummyNumber, targetEntity);

            // Initialize the unit test
            InitializeUnitTest(context);

            // Setup the plugin mock objects
            SetupMockObjectsForPlugin();

            #endregion
            #region ACT

            try
            {
                // Execute the plugin step
                Plugin.Execute(Plugin.ServiceProvider);
            }
            catch (InvalidPluginExecutionException ex)
            {
                ActualException = ex;
            }

            #endregion
            #region ASSERT

            Assert.IsNotNull(ActualException);
            Assert.AreEqual(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.IncorrectMessageName, Plugin.GetType().Name, PlatformMessageHelper.Create, _dummyText), ActualException.Message);

            #endregion
        }

        /// <summary>
        /// Tests the exception thrown when the plugin pipeline stage is not the expected.
        /// </summary>
        [TestMethod]
        public void InitializeEntityUponCreate_ExceptionHandling_IncorrectPipelineStage()
        {
            #region ARRANGE

            // Prepare the unit test mock target annotation
            Entity targetEntity = CreateNoteEntity();

            // Setup the unit test mock context
            PluginExecutionContext context = CreatePluginExecutionContext(PlatformMessageHelper.Create, 
                _dummyNumber, targetEntity.LogicalName, _dummyNumber, targetEntity);

            // Initialize the unit test
            InitializeUnitTest(context);

            // Setup the plugin mock objects
            SetupMockObjectsForPlugin();

            #endregion
            #region ACT

            try
            {
                // Execute the plugin step
                Plugin.Execute(Plugin.ServiceProvider);
            }
            catch (InvalidPluginExecutionException ex)
            {
                ActualException = ex;
            }

            #endregion
            #region ASSERT

            Assert.IsNotNull(ActualException);
            Assert.AreEqual(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.IncorrectPipelineStage, Plugin.GetType().Name, (int)SdkMessageProcessingStepStage.PreOperation, -1), ActualException.Message);

            #endregion
        }

        /// <summary>
        /// Tests the exception thrown when the plugin pipeline stage is not the expected.
        /// </summary>
        [TestMethod]
        public void InitializeEntityUponCreate_ExceptionHandling_MaxDepthViolation()
        {
            #region ARRANGE

            // Prepare the unit test mock target annotation
            Entity targetEntity = CreateNoteEntity();

            // Setup the unit test mock context
            PluginExecutionContext context = CreatePluginExecutionContext(PlatformMessageHelper.Create,
                CustomPlugin.MaximumAllowedExecutionDepth + 1, targetEntity.LogicalName, 
                (int)SdkMessageProcessingStepStage.PreOperation, targetEntity);

            // Initialize the unit test
            InitializeUnitTest(context);

            // Setup the plugin mock objects
            SetupMockObjectsForPlugin();

            #endregion
            #region ACT

            try
            {
                // Execute the plugin step
                Plugin.Execute(Plugin.ServiceProvider);
            }
            catch (InvalidPluginExecutionException ex)
            {
                ActualException = ex;
            }

            #endregion
            #region ASSERT

            Assert.IsNotNull(ActualException);
            Assert.AreEqual(string.Format(CultureInfo.InvariantCulture, 
                TraceMessageHelper.MaxDepthViolation, Plugin.GetType().Name, CustomPlugin.MaximumAllowedExecutionDepth,
                CustomPlugin.MaximumAllowedExecutionDepth + 1), ActualException.Message);

            #endregion
        }

        /// <summary>
        /// Tests the behavior of the plugin when the current annotation document body contains data.
        /// </summary>
        [TestMethod]
        public void InitializeEntityUponCreate_InitializeEntityService_InitializeEntity_CurrentNoteNumberContainsData()
        {
            #region ARRANGE

            // Prepare the unit test mock target annotation
            Note annotation = CreateNote(_dummyText);
            Entity targetEntity = CreateNoteEntity(annotation);

            // Setup the unit test mock context
            PluginExecutionContext context = CreatePluginExecutionContext(PlatformMessageHelper.Create,
                CustomPlugin.MaximumAllowedExecutionDepth, targetEntity.LogicalName,
                (int)SdkMessageProcessingStepStage.PreOperation, targetEntity);

            // Initialize the unit test
            InitializeUnitTest(context);

            // Setup the plugin mock objects
            SetupMockObjectsForPlugin();

            #endregion
            #region ACT

            // Execute the plugin step
            Plugin.Execute(Plugin.ServiceProvider);

            #endregion
            #region ASSERT

            // Load the modified plugin execution context
            IPluginExecutionContext pex = Plugin.PluginExecutionContext;
            Entity modifiedTargetEntity = (Entity)pex.InputParameters[PlatformConstants.TargetText];
            Note modifiedTarget = modifiedTargetEntity.ToEntity<Note>();

            // Assertions
            Assert.AreEqual(modifiedTarget.Document, _dummyText);

            #endregion
        }

        /// <summary>
        /// Tests the behavior of the plugin when the current annotation document body is empty.
        /// </summary>
        [TestMethod]
        public void InitializeEntityUponCreate_InitializeEntityService_InitializeEntity_CurrentNoteNumberIsEmpty()
        {
            #region ARRANGE

            // Prepare the unit test mock target annotation
            Note annotation = CreateNote(string.Empty);
            Entity targetEntity = CreateNoteEntity(annotation);

            // Setup the unit test mock context
            PluginExecutionContext context = CreatePluginExecutionContext(PlatformMessageHelper.Create,
                CustomPlugin.MaximumAllowedExecutionDepth, targetEntity.LogicalName,
                (int)SdkMessageProcessingStepStage.PreOperation, targetEntity);

            // Initialize the unit test
            InitializeUnitTest(context);

            // Setup the plugin mock objects
            SetupMockObjectsForPlugin();

            #endregion
            #region ACT

            // Execute the plugin step
            Plugin.Execute(Plugin.ServiceProvider);

            #endregion
            #region ASSERT

            // Load the modified plugin execution context
            IPluginExecutionContext pex = Plugin.PluginExecutionContext;
            Entity modifiedTargetEntity = (Entity)pex.InputParameters[PlatformConstants.TargetText];
            Note modifiedTarget = modifiedTargetEntity.ToEntity<Note>();

            // Assertions
            Assert.AreNotEqual(modifiedTarget.Document, string.Empty);
            /*
            Assert.AreEqual(modifiedTarget.Document, string.Format(CultureInfo.InvariantCulture, 
                InitializeEntityService.TextFormat, modifiedTarget.CreatedOn.Value.ToString(InitializeEntityService.DateTimeFormat, 
                CultureInfo.InvariantCulture)));
            */
            #endregion
        }

        #endregion
    }
}
