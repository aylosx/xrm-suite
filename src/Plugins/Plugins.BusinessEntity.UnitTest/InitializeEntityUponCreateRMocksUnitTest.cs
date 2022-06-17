namespace Plugins.BusinessEntity.UnitTest
{
    using System;
    using System.Globalization;

    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Microsoft.Xrm.Sdk;

    using Rhino.Mocks;

    using Aylos.Xrm.Sdk.Common;
    using Aylos.Xrm.Sdk.Plugins;
    using Aylos.Xrm.Sdk.Plugins.RhinoMocks;

    using BusinessEntity;
    using Shared.Models.Domain;
    using Shared.BusinessLogic.Services.Account;
    using Shared.BusinessLogic.Services.Data;

    [TestClass]
    public class InitializeEntityUponCreateRMocksUnitTest : CustomPluginUnitTest<InitializeEntityUponCreate>
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
            Plugin.InitializeEntityService = MockRepository.GenerateMock<InitializeEntityService>(Plugin.CrmService, Plugin.OrganizationServiceContext, Plugin.PluginExecutionContext, Plugin.TracingService);
        }

        #endregion

        #region Setup Mock Responses

        #endregion

        #region Helper Methods

        private static Account CreateAccount(string accountNumber)
        {
            var entity = new Account
            {
                Id = Guid.NewGuid(),
                AccountNumber = accountNumber,
                CreatedOn = InitializeEntityService.DateTimeNow
            };
            return entity;
        }

        private static Entity CreateAccountEntity()
        {
            return new Entity
            {
                LogicalName = Account.EntityLogicalName
            };
        }

        private static Entity CreateAccountEntity(Account account)
        {
            return account.ToEntity<Entity>();
        }

        #endregion

        #region Unit Tests

        /// <summary>
        /// Tests the exception thrown when a target entity is not available in the plugin execution context.
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
        /// Tests the exception thrown when the target entity is not the expected.
        /// </summary>
        [TestMethod]
        public void InitializeEntityUponCreate_ExceptionHandling_WrongRegisteredEntity()
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
        public void InitializeEntityUponCreate_ExceptionHandling_IncorrectMessageName()
        {
            #region ARRANGE

            // Prepare the unit test mock target entity
            Entity targetEntity = CreateAccountEntity();

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

            // Prepare the unit test mock target entity
            Entity targetEntity = CreateAccountEntity();

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

            // Prepare the unit test mock target entity
            Entity targetEntity = CreateAccountEntity();

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
        /// Tests the behavior of the plugin when the current entity account number contains data.
        /// </summary>
        [TestMethod]
        public void InitializeEntityUponCreate_InitializeEntityService_InitializeEntity_CurrentAccountNumberContainsData()
        {
            #region ARRANGE

            // Prepare the unit test mock target entity
            Account account = CreateAccount(_dummyText);
            Entity targetEntity = CreateAccountEntity(account);

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
            Account modifiedTarget = modifiedTargetEntity.ToEntity<Account>();

            // Assertions
            Assert.AreEqual(modifiedTarget.AccountNumber, _dummyText);

            #endregion
        }

        /// <summary>
        /// Tests the behavior of the plugin when the current entity account number is empty.
        /// </summary>
        [TestMethod]
        public void InitializeEntityUponCreate_InitializeEntityService_InitializeEntity_CurrentAccountNumberIsEmpty()
        {
            #region ARRANGE

            // Prepare the unit test mock target entity
            Account account = CreateAccount(string.Empty);
            Entity targetEntity = CreateAccountEntity(account);

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
            Account modifiedTarget = modifiedTargetEntity.ToEntity<Account>();

            // Assertions
            Assert.AreNotEqual(modifiedTarget.AccountNumber, string.Empty);
            Assert.AreEqual(modifiedTarget.AccountNumber, string.Format(CultureInfo.InvariantCulture, 
                InitializeEntityService.TextFormat, modifiedTarget.CreatedOn.Value.ToString(InitializeEntityService.DateTimeFormat, 
                CultureInfo.InvariantCulture)));

            #endregion
        }

        #endregion
    }
}
