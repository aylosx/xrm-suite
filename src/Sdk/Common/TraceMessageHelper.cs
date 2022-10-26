namespace Aylos.Xrm.Sdk.Common
{
    public static class TraceMessageHelper
    {
        public const string AbortingMethod = "{0} | Aborting | {1}.";
        public const string ArgumentException = "{0} | Please check the input arguments syntax and retry.";
        public const string BusinessEntityCollectionIsEmpty = "{0} | The BusinessEntityCollection in the context is empty.";
        public const string BusinessEntityCollectionNotFound = "{0} | BusinessEntityCollection not found in the context.";
        public const string BusinessEntityIsEmpty = "{0} | the BusinessEntity in the context is empty.";
        public const string BusinessEntityNotFound = "{0} | BusinessEntity not found in the context.";
        public const string CodeActivityContextIsRequired = "{0} | Code activity context is required.";
        public const string ColumnSetIsEmpty = "{0} | the ColumnSet in the context is empty.";
        public const string ColumnSetNotFound = "{0} | ColumnSet not found in the context.";
        public const string DisposingMessage = "{0} | Disposing class.";
        public const string EnteredMethod = "{0} | Entered | {1}.";
        public const string ExceptionDetailKey = "Detail Key: {0}, Value: {1}";
        public const string ExceptionErrorCode = "ErrorCode: {0}";
        public const string ExceptionMessage = "Message: {0}";
        public const string ExceptionTrace = "Trace: {0}";
        public const string ExecutionContextTracingCompleted = "{0} | Tracing execution context completed at {1}.";
        public const string ExecutionTime = "{0} | The module execution required {1} milliseconds.";
        public const string ExitingMethod = "{0} | Exiting | {1}.";
        public const string HttpRequestExceptionMessage = "{0} | A HTTP request exception occurred. Error: {1} - Stack: {2}.";
        public const string IncorrectMessageName = "{0} | The module is not running under the correct message type. Message type {1} expected but currently is {2}.";
        public const string IncorrectPipelineStage = "{0} | The module is not running in the expected pipeline stage. Stage {1} expected but currently is {2}.";
        public const string InvalidExecutionContext = "{0} | Execution context is not valid. Check the module registration details.";
        public const string InvalidFilterConfiguration = "{0} | The module filtering attributes of the target entity have not been configured properly.";
        public const string InvalidPluginExecution = "{0} | An invalid plug-in exception occurred. Error: {1} - Stack: {2}.";
        public const string LeadEntityReferenceIsRequired = "{0} | Execution context is not valid, lead reference is required. Check the module registration details.";
        public const string MaxDepthViolation = "{0} | The module is running in depth that is greater than expected. Maximum depth expected {1} but currently is {2}.";
        public const string MissingPostImage = "{0} | Incorrect plugin registration against {1} message - post image {2} expected but not found.";
        public const string MissingPreImage = "{0} | Incorrect plugin registration against {1} message - pre image {2} expected but not found.";
        public const string NoActionsHaveBeenProvided = "{0} | No actions have been provided.";
        public const string OrganizationServiceFault = "{0} | An organization service fault occurred. Error: {1} - Stack: {2}.";
        public const string PostEntityIsRequired = "{0} | Execution context is not valid, post entity is required. Check the module registration details.";
        public const string PreEntityIsRequired = "{0} | Execution context is not valid, pre entity is required. Check the module registration details.";
        public const string QueryIsEmpty = "{0} | the Query in the context is empty.";
        public const string QueryNotFound = "{0} | Query not found in the context.";
        public const string RegisteredAttributesMissing = "{0} | One or more registered attributes are missing. Check the module registration details.";
        public const string ServiceProviderIsRequired = "{0} | Service provider is required.";
        public const string StartTracing = "Starting {0} ... time elapsed: {1}.";
        public const string StopTracing = "Stopping {0}.";
        public const string TargetEntityIsRequired = "{0} | Execution context is not valid, target entity is required. Check the module registration details.";
        public const string TargetEntityReferenceIsRequired = "{0} | Execution context is not valid, target entity reference is required. Check the module registration details.";
        public const string UndefinedFault = "{0} | An exception occurred. Error: {1} - Stack: {2}.";
        public const string WrongRegisteredEntity = "{0} | Module does not support entities of type {1}. Check the module registration details.";
    }
}
