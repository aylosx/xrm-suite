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
    public class ManageEntityDeletionUponDeleteRMocksUnitTest : CustomPluginUnitTest<ManageEntityDeletionUponDelete>
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
            Plugin.ManageDeleteEntityService = MockRepository.GenerateMock<ManageDeleteEntityService>(Plugin.CrmService, Plugin.OrganizationServiceContext, Plugin.PluginExecutionContext, Plugin.TracingService);
        }

        #endregion

        #region Setup Mock Responses

        #endregion

        #region Helper Methods

        //private static Account CreateAccount(string accountNumber)
        //{
        //    var entity = new Account
        //    {
        //        Id = Guid.NewGuid(),
        //        AccountNumber = accountNumber,
        //        CreatedOn = ManageDeleteEntityService.DateTimeNow
        //    };
        //    return entity;
        //}

        private static Entity CreateAccountEntity()
        {
            return new Entity
            {
                LogicalName = Account.EntityLogicalName
            };
        }

        //private static Entity CreateAccountEntity(Account account)
        //{
        //    return account.ToEntity<Entity>();
        //}

        #endregion

        #region Unit Tests

        /// <summary>
        /// Tests the exception thrown when a target entity reference is missing.
        /// </summary>
        [TestMethod]
        public void ManageEntityDeletionUponDelete_ExceptionHandling_TargetEntityReferenceIsMissing()
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
            Assert.AreEqual(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.TargetEntityReferenceIsRequired, Plugin.GetType().Name), ActualException.Message);

            #endregion
        }

        /// <summary>
        /// Tests the exception thrown when the pre entity is missing.
        /// </summary>
        [TestMethod]
        public void ManageEntityDeletionUponDelete_ExceptionHandling_PreEntityIsMissing()
        {
            #region ARRANGE

            // Prepare the unit test mock target entity reference
            EntityReference targetEntityReference = new EntityReference(Account.EntityLogicalName, Guid.NewGuid());

            // Setup the unit test mock context
            PluginExecutionContext context = CreatePluginExecutionContext(targetEntityReference);

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
            Assert.AreEqual(string.Format(CultureInfo.InvariantCulture,TraceMessageHelper.PreEntityIsRequired, Plugin.GetType().Name), ActualException.Message);

            #endregion
        }

        /// <summary>
        /// Tests the exception thrown when the pre entity is not the expected.
        /// </summary>
        [TestMethod]
        public void ManageEntityDeletionUponDelete_ExceptionHandling_WrongRegisteredEntity()
        {
            #region ARRANGE

            // Prepare the unit test mock target entity reference
            EntityReference targetEntityReference = new EntityReference(Account.EntityLogicalName, Guid.NewGuid());

            // Prepare the unit test mock target entity
            Entity preEntity = new Entity { LogicalName = _dummyText };

            // Setup the unit test mock context
            PluginExecutionContext context = CreatePluginExecutionContext(targetEntityReference, preEntity, null);

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
            Assert.AreEqual(ActualException.Message, string.Format(CultureInfo.InvariantCulture,
                TraceMessageHelper.WrongRegisteredEntity, Plugin.GetType().Name, _dummyText));

            #endregion
        }

        /// <summary>
        /// Tests the exception thrown when the plugin message is not the expected.
        /// </summary>
        [TestMethod]
        public void ManageEntityDeletionUponDelete_ExceptionHandling_IncorrectMessageName()
        {
            #region ARRANGE

            // Prepare the unit test mock target entity reference
            EntityReference targetEntityReference = new EntityReference(Account.EntityLogicalName, Guid.NewGuid());

            // Prepare the unit test mock target entity
            Entity preEntity = CreateAccountEntity();

            // Setup the unit test mock context
            PluginExecutionContext context = CreatePluginExecutionContext(_dummyText,
                _dummyNumber, preEntity.LogicalName, _dummyNumber, targetEntityReference, preEntity, null);

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
            Assert.AreEqual(ActualException.Message, string.Format(CultureInfo.InvariantCulture,
                TraceMessageHelper.IncorrectMessageName, Plugin.GetType().Name, PlatformMessageHelper.Delete, _dummyText));

            #endregion
        }

        /// <summary>
        /// Tests the exception thrown when the plugin pipeline stage is not the expected.
        /// </summary>
        [TestMethod]
        public void ManageEntityDeletionUponDelete_ExceptionHandling_IncorrectPipelineStage()
        {
            #region ARRANGE

            // Prepare the unit test mock target entity reference
            EntityReference targetEntityReference = new EntityReference(Account.EntityLogicalName, Guid.NewGuid());

            // Prepare the unit test mock target entity
            Entity preEntity = CreateAccountEntity();

            // Setup the unit test mock context
            PluginExecutionContext context = CreatePluginExecutionContext(PlatformMessageHelper.Delete,
                _dummyNumber, preEntity.LogicalName, _dummyNumber, targetEntityReference, preEntity, null);

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
            Assert.AreEqual(ActualException.Message, string.Format(CultureInfo.InvariantCulture,
                TraceMessageHelper.IncorrectPipelineStage, Plugin.GetType().Name, (int)SdkMessageProcessingStepStage.PreOperation, -1));

            #endregion
        }

        /// <summary>
        /// Tests the exception thrown when the plugin pipeline stage is not the expected.
        /// </summary>
        [TestMethod]
        public void ManageEntityDeletionUponDelete_ExceptionHandling_MaxDepthViolation()
        {
            #region ARRANGE

            // Prepare the unit test mock target entity reference
            EntityReference targetEntityReference = new EntityReference(Account.EntityLogicalName, Guid.NewGuid());

            // Prepare the unit test mock target entity
            Entity preEntity = CreateAccountEntity();

            // Setup the unit test mock context
            PluginExecutionContext context = CreatePluginExecutionContext(PlatformMessageHelper.Delete,
                CustomPlugin.MaximumAllowedExecutionDepth + 1, preEntity.LogicalName,
                (int)SdkMessageProcessingStepStage.PreOperation, targetEntityReference, preEntity, null);

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
            Assert.AreEqual(ActualException.Message, string.Format(CultureInfo.InvariantCulture,
                TraceMessageHelper.MaxDepthViolation, Plugin.GetType().Name, CustomPlugin.MaximumAllowedExecutionDepth,
                CustomPlugin.MaximumAllowedExecutionDepth + 1));

            #endregion
        }

        #endregion
    }
}
