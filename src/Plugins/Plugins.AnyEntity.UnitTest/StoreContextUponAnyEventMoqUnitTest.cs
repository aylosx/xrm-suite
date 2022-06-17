namespace Plugins.AnyEntity.UnitTest
{
    using System;
    using System.Globalization;
    using System.Linq;

    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Microsoft.Xrm.Sdk;

    using Moq;

    using Aylos.Xrm.Sdk.Common;
    using Aylos.Xrm.Sdk.Plugins;
    using Aylos.Xrm.Sdk.Plugins.MoqTests;

    using Shared.Models.Domain;
    using Shared.BusinessLogic.Services.Data;
    using Shared.BusinessLogic.Services.ExecutionContext;

    [TestClass]
    public class StoreContextUponAnyEventMoqUnitTest : CustomPluginUnitTest<StoreContextUponAnyEvent>
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

            var mecMock = new Mock<ManageExecutionContext>(Plugin.CrmService, Plugin.OrganizationServiceContext, Plugin.PluginExecutionContext, Plugin.TracingService).SetupAllProperties();
            Plugin.ManageExecutionContext = mecMock.Object;
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
                CreatedOn = ManageExecutionContext.DateTimeNow
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
        /// Tests the exception thrown when the plugin pipeline stage is not the expected.
        /// </summary>
        [TestMethod]
        public void StoreContextUponAnyEvent_ExceptionHandling_IncorrectPipelineStage()
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
            Assert.AreEqual(string.Format(CultureInfo.InvariantCulture, TraceMessageHelper.IncorrectPipelineStage, Plugin.GetType().Name, (int)SdkMessageProcessingStepStage.PostOperation, -1), ActualException.Message);

            #endregion
        }

        /// <summary>
        /// Tests the exception thrown when the plugin pipeline stage is not the expected.
        /// </summary>
        [TestMethod]
        public void StoreContextUponAnyEvent_ExceptionHandling_MaxDepthViolation()
        {
            #region ARRANGE

            // Prepare the unit test mock target entity
            Entity targetEntity = CreateAccountEntity();

            // Setup the unit test mock context
            PluginExecutionContext context = CreatePluginExecutionContext(PlatformMessageHelper.Create, 8, 
                targetEntity.LogicalName, (int)SdkMessageProcessingStepStage.PostOperation, targetEntity);

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
        public void StoreContextUponAnyEvent_ManageExecutionContext_StoreExecutionContext()
        {
            #region ARRANGE

            // Prepare the unit test mock target entity
            Account account = CreateAccount(_dummyText);
            Entity targetEntity = CreateAccountEntity(account);

            // Setup the unit test mock context
            PluginExecutionContext context = CreatePluginExecutionContext(PlatformMessageHelper.Create, 7,
                targetEntity.LogicalName, (int)SdkMessageProcessingStepStage.PostOperation, targetEntity);

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
            CrmServiceContext csc = Plugin.OrganizationServiceContext;
            ExecutionContext ec = csc.GetAttachedEntities().Single().ToEntity<ExecutionContext>();

            // Assertions
            Assert.IsNotNull(ec);
            Assert.AreEqual(targetEntity.LogicalName, ec.EntityName);
            Assert.AreEqual(targetEntity.Id, new Guid(ec.EntityId));

            #endregion
        }

        #endregion
    }
}
