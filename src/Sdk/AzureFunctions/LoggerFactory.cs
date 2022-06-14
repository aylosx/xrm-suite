namespace Aylos.Xrm.Sdk.AzureFunctions
{
    public class LoggerFactory : ILoggerFactory
    {
        public ILogger GetLogger(Microsoft.Extensions.Logging.ILogger logger)
        {
            return new ApplicationInsightsLogger(logger);
        }
    }
}
