namespace CodeActivities.BusinessEntity.UnitTest
{
    using System;
    using System.Activities;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;

    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Microsoft.Xrm.Sdk;
    using Microsoft.Xrm.Sdk.Client;

    using Rhino.Mocks;

    using Aylos.Xrm.Sdk.CodeActivities;
    using Aylos.Xrm.Sdk.CodeActivities.RhinoMocks;
    using Aylos.Xrm.Sdk.Common;

    using BusinessEntity;
    using Shared.Models.Domain;
    using Shared.BusinessLogic.Services.None;
    using Shared.BusinessLogic.Services.Data;
    using Shared.Models.Responses.Account;

    [TestClass]
    public class InitializeEntityUnitTest : CustomCodeActivityUnitTest<InitializeEntity>
    {
        #region Constants and Static Members

        private static Guid _unknownAccountKey = Guid.NewGuid();
        private static Guid _accountKey = Guid.NewGuid();
        private const string _accountNumber = "Account Number";

        #endregion

        #region Setup Mocks & Stubs

        /// <summary>
        /// Sets up the mock objects of the derived unit test class. 
        /// </summary>
        public override void SetupMockObjectsForCustomCodeActivity()
        {
            CustomCodeActivity.OrganizationServiceContext = MockRepository.GenerateStub<CrmServiceContext>(CurrentUserService);
            CustomCodeActivity.CrmService = MockRepository.GenerateMock<CrmService>(CustomCodeActivity.OrganizationServiceContext, TracingService);
            CustomCodeActivity.InitializeEntityService = MockRepository.GenerateMock<InitializeEntityService>(CustomCodeActivity.CrmService, CustomCodeActivity.OrganizationServiceContext, WorkflowContext, TracingService);
        }

        #endregion

        #region Setup Mock Responses

        /// <summary>
        /// Sets up the mock response for account
        /// </summary>
        private void SetupMockResponseForAccount(Guid key, string accountNumber)
        {
            Account account = new Account
            {
                Id = key,
                AccountNumber = accountNumber
            };
            // Stub the data layer method
            if (key == _accountKey) {
                CustomCodeActivity.CrmService.Stub(x => x.GetAccountByKey(_accountKey)).Return(account);
            }
            else {
                CustomCodeActivity.CrmService.Stub(x => x.GetAccountByKey(Arg<Guid>.Is.Anything)).IgnoreArguments().Return(null);
            }
        }

        #endregion

        #region Unit Tests

        /// <summary>
        /// Tests the exception thrown when the initialize entity primary key parameter is NULL.
        /// </summary>
        [TestMethod]
        public void InitializeEntity_ExceptionHandling_AccountPrimaryKeyIsNull()
        {
            #region ARRANGE

            // Create the workflow unit test mock context
            WorkflowExecutionContext context = CreateWorkflowExecutionContext();

            // Initialize the unit test
            InitializeUnitTest(context, new InitializeEntity());

            // Setup the mock input
            CodeActivityInput.Add(InitializeEntity.BusinessEntityPrimaryKeyText, null);

            #endregion
            #region ACT

            // Act: Invoke the workflow returns the actual ouput
            CodeActivityOutput = WorkflowInvoker.Invoke(CodeActivityInput);

            #endregion
            #region ASSERT

            Assert.IsNotNull((string)CodeActivityOutput[ExceptionText]);
            Assert.AreEqual(InitializeEntity.BusinessEntityPrimaryKeyIsRequiredMessage, (string)CodeActivityOutput[ExceptionText]);

            #endregion
        }

        /// <summary>
        /// Tests the exception thrown when the initialize entity primary key parameter is empty.
        /// </summary>
        [TestMethod]
        public void InitializeEntity_ExceptionHandling_AccountPrimaryKeyIsEmpty()
        {
            #region ARRANGE

            // Create the workflow unit test mock context
            WorkflowExecutionContext context = CreateWorkflowExecutionContext();

            // Initialize the unit test
            InitializeUnitTest(context, new InitializeEntity());

            // Setup the mock input
            CodeActivityInput.Add(InitializeEntity.BusinessEntityPrimaryKeyText, "   ");

            #endregion
            #region ACT

            // Act: Invoke the workflow returns the actual ouput
            CodeActivityOutput = WorkflowInvoker.Invoke(CodeActivityInput);

            #endregion
            #region ASSERT

            Assert.IsNotNull((string)CodeActivityOutput[ExceptionText]);
            Assert.AreEqual(InitializeEntity.BusinessEntityPrimaryKeyIsRequiredMessage, (string)CodeActivityOutput[ExceptionText]);

            #endregion
        }

        /// <summary>
        /// Tests the exception thrown when the initialize entity primary key parameter is invalid.
        /// </summary>
        [TestMethod]
        public void InitializeEntity_ExceptionHandling_AccountPrimaryKeyIsInvalid()
        {
            #region ARRANGE

            // Create the workflow unit test mock context
            WorkflowExecutionContext context = CreateWorkflowExecutionContext();

            // Initialize the unit test
            InitializeUnitTest(context, new InitializeEntity());

            // Setup the mock input
            CodeActivityInput.Add(InitializeEntity.BusinessEntityPrimaryKeyText, "invalid primary key");

            #endregion
            #region ACT

            // Act: Invoke the workflow returns the actual ouput
            CodeActivityOutput = WorkflowInvoker.Invoke(CodeActivityInput);

            #endregion
            #region ASSERT

            Assert.IsNotNull((string)CodeActivityOutput[ExceptionText]);
            Assert.AreEqual(InitializeEntity.BusinessEntityPrimaryKeyIsInvalidMessage, (string)CodeActivityOutput[ExceptionText]);

            #endregion
        }

        /// <summary>
        /// Tests the exception thrown when the account does not exist.
        /// </summary>
        [TestMethod]
        public void InitializeEntityService_ExceptionHandling_AccountNotExist()
        {
            #region ARRANGE

            // Create the workflow unit test mock context
            WorkflowExecutionContext context = CreateWorkflowExecutionContext();

            // Initialize the unit test
            InitializeUnitTest(context, new InitializeEntity());

            // Setup the mock input
            CodeActivityInput.Add(InitializeEntity.BusinessEntityPrimaryKeyText, _unknownAccountKey.ToString());

            // Setup the mock objects
            SetupMockObjectsForCustomCodeActivity();

            // Setup the mock responses
            SetupMockResponseForAccount(_unknownAccountKey, _accountNumber);

            #endregion
            #region ACT

            // Act: Invoke the workflow returns the actual ouput
            CodeActivityOutput = WorkflowInvoker.Invoke(CodeActivityInput);

            #endregion
            #region ASSERT

            Assert.IsNotNull((string)CodeActivityOutput[ExceptionText]);
            Assert.AreEqual(InitializeEntityService.AccountNotExistMessage, (string)CodeActivityOutput[ExceptionText]);

            #endregion
        }

        /// <summary>
        /// Tests the exception thrown when the account number is already set.
        /// </summary>
        [TestMethod]
        public void InitializeEntityService_ExceptionHandling_AccountNumberAlreadySet()
        {
            #region ARRANGE

            // Create the workflow unit test mock context
            WorkflowExecutionContext context = CreateWorkflowExecutionContext();

            // Initialize the unit test
            InitializeUnitTest(context, new InitializeEntity());

            // Setup the mock input
            CodeActivityInput.Add(InitializeEntity.BusinessEntityPrimaryKeyText, _accountKey.ToString());

            // Setup the mock objects
            SetupMockObjectsForCustomCodeActivity();

            // Setup the mock responses
            SetupMockResponseForAccount(_accountKey, _accountNumber);

            #endregion
            #region ACT

            // Act: Invoke the workflow returns the actual ouput
            CodeActivityOutput = WorkflowInvoker.Invoke(CodeActivityInput);

            #endregion
            #region ASSERT

            Assert.IsNotNull((string)CodeActivityOutput[ExceptionText]);
            Assert.AreEqual(InitializeEntityService.AccountNumberAlreadySetMessage, (string)CodeActivityOutput[ExceptionText]);

            #endregion
        }

        /// <summary>
        /// Tests the action of setting up the account number.
        /// </summary>
        [TestMethod]
        public void InitializeEntityService_InitializeEntityService_SetAccountNumber()
        {
            #region ARRANGE

            // Create the workflow unit test mock context
            WorkflowExecutionContext context = CreateWorkflowExecutionContext();

            // Initialize the unit test
            InitializeUnitTest(context, new InitializeEntity());

            // Setup the mock input
            CodeActivityInput.Add(InitializeEntity.BusinessEntityPrimaryKeyText, _accountKey.ToString());

            // Setup the mock objects
            SetupMockObjectsForCustomCodeActivity();

            // Setup the mock responses
            SetupMockResponseForAccount(_accountKey, null);

            #endregion
            #region ACT

            // Act: Invoke the workflow returns the actual ouput
            CodeActivityOutput = WorkflowInvoker.Invoke(CodeActivityInput);

            #endregion
            #region ASSERT

            // Setup the expected result
            string accountNumber = string.Format(CultureInfo.InvariantCulture,
                InitializeEntityService.TextFormat, InitializeEntityService.DateTimeNow.
                ToString(InitializeEntityService.DateTimeFormat, CultureInfo.InvariantCulture));

            Assert.IsNull((string)CodeActivityOutput[ExceptionText]);
            InitializedEntity account = SerializationHelper.DeserializeJson<InitializedEntity>(
                (string)CodeActivityOutput.SingleOrDefault(x => x.Key.Equals("InitializedEntity")).Value);
            Assert.AreEqual(accountNumber, account.AccountNumber);

            #endregion
        }

        #endregion
    }
}