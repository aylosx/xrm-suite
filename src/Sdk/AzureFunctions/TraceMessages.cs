namespace Aylos.Xrm.Sdk.AzureFunctions
{
    public static class TraceMessages
    {
        public const string AbortingMethod = "{0} : Aborting method : {1}.";
        public const string ArgumentException = "{0} : Please check the input arguments syntax and retry.";
        public const string CodeActivityContextIsRequired = "{0} : Code activity context is required.";
        public const string DisposingMessage = "{0} : Disposing class.";
        public const string EnteredMethod = "{0} : Entered method : {1}.";
        public const string ExceptionDetailKey = "Detail Key: {0}, Value: {1}";
        public const string ExceptionErrorCode = "ErrorCode: {0}";
        public const string ExceptionMessage = "Message: {0}";
        public const string ExceptionTrace = "Trace: {0}";
        public const string ExecutionContextTracingCompleted = "{0} : Tracing execution context completed at {1}.";
        public const string ExecutionTime = "{0} : The module execution required {1} milliseconds.";
        public const string ExitingMethod = "{0} : Exiting method : {1}.";
        public const string IncorrectMessageName = "{0} : The module is not running under the correct message type. Message type {1} expected but currently is {2}.";
        public const string IncorrectPipelineStage = "{0} : The module is not running in the expected pipeline stage. Stage {1} expected but currently is {2}.";
        public const string InvalidExecutionContext = "{0} : Execution context is not valid. Check the module registration details.";
        public const string InvalidFilterConfiguration = "{0} : The module filtering attributes of the target entity have not been configured properly.";
        public const string InvalidPluginExecution = "{0} : An invalid plug-in exception occurred. Error: {1} - Stack: {2}.";
        public const string MaxDepthViolation = "{0} : The module is running in depth that is greater than expected. Maximum depth expected {1} but currently is {2}.";
        public const string MissingPostImage = "{0} : Incorrect plugin registration against {1} message - post image {2} expected but not found.";
        public const string MissingPreImage = "{0} : Incorrect plugin registration against {1} message - pre image {2} expected but not found.";
        public const string NoActionsHaveBeenProvided = "{0} : No actions have been provided.";
        public const string OrganizationServiceFault = "{0} : An organization service fault occurred. Error: {1} - Stack: {2}.";
        public const string ServiceProviderIsRequired = "{0} : Service provider is required.";
        public const string StartTracing = "Starting {0} ... time elapsed: {1}.";
        public const string StopTracing = "Stopping {0}.";
        public const string UndefinedFault = "{0} : An exception occurred. Error: {1} - Stack: {2}.";
        public const string WrongRegisteredEntity = "{0} : Module does not support entities of type {1}. Check the module registration details.";
    }
}
