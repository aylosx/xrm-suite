namespace Plugins.BusinessEntity.UnitTest
{
    using System;
    using System.Globalization;

    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Microsoft.Xrm.Sdk;

    using Moq;

    using Aylos.Xrm.Sdk.Common;
    using Aylos.Xrm.Sdk.Plugins;
    using Aylos.Xrm.Sdk.Plugins.MoqTests;

    using BusinessEntity;
    using Shared.Models.Domain;
    using Shared.BusinessLogic.Services.Account;
    using Shared.BusinessLogic.Services.Data;

    [TestClass]
    public class InitializeEntityUponUpdateMoqUnitTest : CustomPluginUnitTest<InitializeEntityUponUpdate>
    {
        #region Constants and Static Members

        const string _dummyString = "Dummy";
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

        private static Account CreateAccount(string name)
        {
            var entity = new Account
            {
                Id = Guid.NewGuid(),
                AccountNumber = name,
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
        public void InitializeEntityUponUpdate_ExceptionHandling_TargetEntityIsRequired()
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
        public void InitializeEntityUponUpdate_ExceptionHandling_WrongRegisteredEntity()
        {
            #region ARRANGE

            // Prepare the unit test mock target entity
            Entity targetEntity = new Entity { LogicalName = _dummyString };

            // Setup the unit test mock context
            PluginExecutionContext context = CreatePluginExecutionContext(targetEntity, null, null);

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
            Assert.AreEqual(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.WrongRegisteredEntity, Plugin.GetType().Name, _dummyString), ActualException.Message);

            #endregion
        }

        /// <summary>
        /// Tests the exception thrown when the plugin message is not the expected.
        /// </summary>
        [TestMethod]
        public void InitializeEntityUponUpdate_ExceptionHandling_IncorrectMessageName()
        {
            #region ARRANGE

            // Prepare the unit test mock target entity
            Entity targetEntity = CreateAccountEntity();

            // Setup the unit test mock context
            PluginExecutionContext context = CreatePluginExecutionContext(_dummyString, 
                _dummyNumber, targetEntity.LogicalName, _dummyNumber, targetEntity, null, null);

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
            Assert.AreEqual(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.IncorrectMessageName, Plugin.GetType().Name, PlatformMessageHelper.Update, _dummyString), ActualException.Message);

            #endregion
        }

        /// <summary>
        /// Tests the exception thrown when the plugin pipeline stage is not the expected.
        /// </summary>
        [TestMethod]
        public void InitializeEntityUponUpdate_ExceptionHandling_IncorrectPipelineStage()
        {
            #region ARRANGE

            // Prepare the unit test mock target entity
            Entity targetEntity = CreateAccountEntity();

            // Setup the unit test mock context
            PluginExecutionContext context = CreatePluginExecutionContext(PlatformMessageHelper.Update, 
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
        public void InitializeEntityUponUpdate_ExceptionHandling_MaxDepthViolation()
        {
            #region ARRANGE

            // Prepare the unit test mock target entity
            Entity targetEntity = CreateAccountEntity();

            // Setup the unit test mock context
            PluginExecutionContext context = CreatePluginExecutionContext(PlatformMessageHelper.Update,
                CustomPlugin.MaximumAllowedExecutionDepth + 1, targetEntity.LogicalName, 
                (int)SdkMessageProcessingStepStage.PreOperation, targetEntity, null, null);

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
        /// Tests the behavior of the plugin when the current name contains data.
        /// </summary>
        [TestMethod]
        public void InitializeEntityUponUpdate_InitializeEntityService_InitializeEntity_CurrentAccountNumberContainsData()
        {
            #region ARRANGE

            // Prepare the unit test mock target entity
            Account account = CreateAccount(_dummyString);
            Entity targetEntity = CreateAccountEntity(account);

            // Setup the unit test mock context
            PluginExecutionContext context = CreatePluginExecutionContext(PlatformMessageHelper.Update,
                CustomPlugin.MaximumAllowedExecutionDepth, targetEntity.LogicalName,
                (int)SdkMessageProcessingStepStage.PreOperation, targetEntity, null, null);

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
            Assert.AreEqual(modifiedTarget.AccountNumber, _dummyString);

            #endregion
        }

        /// <summary>
        /// Tests the behavior of the plugin when the current name is empty.
        /// </summary>
        [TestMethod]
        public void InitializeEntityUponUpdate_InitializeEntityService_InitializeEntity_CurrentAccountNumberIsEmpty()
        {
            #region ARRANGE

            // Prepare the unit test mock target entity
            Account account = CreateAccount(string.Empty);
            Entity targetEntity = CreateAccountEntity(account);

            // Setup the unit test mock context
            PluginExecutionContext context = CreatePluginExecutionContext(PlatformMessageHelper.Update,
                CustomPlugin.MaximumAllowedExecutionDepth, targetEntity.LogicalName,
                (int)SdkMessageProcessingStepStage.PreOperation, targetEntity, null, null);

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
                InitializeEntityService.TextFormat, modifiedTarget.CreatedOn.Value.ToString(InitializeEntityService.DateTimeFormat, CultureInfo.InvariantCulture)));

            #endregion
        }

        #endregion
    }
}
