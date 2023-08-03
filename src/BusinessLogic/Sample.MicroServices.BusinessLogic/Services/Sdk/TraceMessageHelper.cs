namespace Sample.MicroServices.BusinessLogic.Services.Sdk
{
    public static class TraceMessageHelper
    {
        public const string AbortingMethod = "{0} | Aborting | {1}.";
        public const string ArgumentException = "{0} | Please check the input arguments syntax and retry.";
        public const string DisposingMessage = "{0} | Disposing class.";
        public const string EnteredMethod = "{0} | Entered | {1}.";
        public const string TracingCompleted = "{0} | Tracing of input context completed at {1}.";
        public const string ExitingMethod = "{0} | Exiting | {1}.";
        public const string HttpRequestExceptionMessage = "{0} | A HTTP request exception occurred. Error: {1} - Stack: {2}.";
        public const string StopTracing = "Stopping {0}.";
    }
}
