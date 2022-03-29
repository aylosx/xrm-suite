namespace Plugins.ExecutionContext.UnitTest
{
    using System;
    using System.Globalization;

    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Microsoft.Xrm.Sdk;

    using Rhino.Mocks;

    using Aylos.Xrm.Sdk.Common;
    using Aylos.Xrm.Sdk.Plugins;
    using Aylos.Xrm.Sdk.Plugins.RhinoMocks;

    using ExecutionContext;
    using Shared.Models.Domain;
    using Shared.BusinessLogic.Services.ExecutionContext;
    using Shared.BusinessLogic.Services.Data;

    [TestClass]
    public class PostMessageUponCreateUnitTest : CustomPluginUnitTest<PostMessageUponCreate>
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
            Plugin.OrganizationServiceContext = MockRepository.GenerateStub<CrmServiceContext>(Plugin.CurrentUserService);
            Plugin.CrmService = MockRepository.GenerateMock<CrmService>(Plugin.OrganizationServiceContext, Plugin.TracingService);
            Plugin.PostMessageService = MockRepository.GenerateMock<PostMessageService>(Plugin.CrmService, Plugin.OrganizationServiceContext, Plugin.PluginExecutionContext, Plugin.TracingService);
        }

        #endregion

        #region Setup Mock Responses

        #endregion

        #region Helper Methods

        private static ExecutionContext CreateExecutionContext(string accountNumber)
        {
            var entity = new ExecutionContext
            {
                Id = Guid.NewGuid(),
                CreatedOn = PostMessageService.DateTimeNow
            };
            return entity;
        }

        private static Entity CreateExecutionContextEntity()
        {
            return new Entity
            {
                LogicalName = ExecutionContext.EntityLogicalName
            };
        }

        private static Entity CreateExecutionContextEntity(ExecutionContext account)
        {
            return account.ToEntity<Entity>();
        }

        #endregion

        #region Unit Tests

        /// <summary>
        /// Tests the exception thrown when a target entity is not available in the plugin execution context.
        /// </summary>
        [TestMethod]
        public void PostMessageUponCreate_ExceptionHandling_TargetEntityIsRequired()
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
        /// Tests the exception thrown when the target entity is not the expected.
        /// </summary>
        [TestMethod]
        public void PostMessageUponCreate_ExceptionHandling_WrongRegisteredEntity()
        {
            #region ARRANGE

            // Prepare the unit test mock target entity
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
        public void PostMessageUponCreate_ExceptionHandling_IncorrectMessageName()
        {
            #region ARRANGE

            // Prepare the unit test mock target entity
            Entity targetEntity = CreateExecutionContextEntity();

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
        public void PostMessageUponCreate_ExceptionHandling_IncorrectPipelineStage()
        {
            #region ARRANGE

            // Prepare the unit test mock target entity
            Entity targetEntity = CreateExecutionContextEntity();

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
            Assert.AreEqual(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.IncorrectPipelineStage, 
                Plugin.GetType().Name, (int)SdkMessageProcessingStepStage.PostOperation, -1), ActualException.Message);

            #endregion
        }

        /// <summary>
        /// Tests the exception thrown when the plugin pipeline stage is not the expected.
        /// </summary>
        [TestMethod]
        public void PostMessageUponCreate_ExceptionHandling_MaxDepthViolation()
        {
            #region ARRANGE

            // Prepare the unit test mock target entity
            Entity targetEntity = CreateExecutionContextEntity();

            // Setup the unit test mock context
            PluginExecutionContext context = CreatePluginExecutionContext(PlatformMessageHelper.Create,
                8, targetEntity.LogicalName, (int)SdkMessageProcessingStepStage.PostOperation, targetEntity);

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
        /// Tests the behavior of the plugin when ... .
        /// </summary>
        [TestMethod]
        public void PostMessageUponCreate_PostMessageService_PostMessage_Dosomething()
        {
            #region ARRANGE

            // Prepare the unit test mock target entity
            ExecutionContext account = CreateExecutionContext(_dummyText);
            Entity targetEntity = CreateExecutionContextEntity(account);

            // Setup the unit test mock context
            PluginExecutionContext context = CreatePluginExecutionContext(PlatformMessageHelper.Create,
                7, targetEntity.LogicalName, (int)SdkMessageProcessingStepStage.PostOperation, targetEntity);

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
            ExecutionContext modifiedTarget = modifiedTargetEntity.ToEntity<ExecutionContext>();

            // Assertions


            #endregion
        }

        #endregion
    }
}
