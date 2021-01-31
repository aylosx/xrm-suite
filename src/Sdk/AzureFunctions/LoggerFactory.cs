namespace Aylos.Xrm.Sdk.AzureFunctions
{
    using Microsoft.Azure.WebJobs.Host;

    public class LoggerFactory : ILoggerFactory
    {
#pragma warning disable CS0618 // Type or member is obsolete
        public ILogger GetLogger(TraceWriter logger)
#pragma warning restore CS0618 // Type or member is obsolete
        {
            return new TraceWriteLogger(logger);
        }

        public ILogger GetLogger(Microsoft.Extensions.Logging.ILogger logger)
        {
            return new ApplicationInsightsLogger(logger);
        }
    }
}
