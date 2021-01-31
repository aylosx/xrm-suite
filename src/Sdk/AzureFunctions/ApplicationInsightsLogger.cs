namespace Aylos.Xrm.Sdk.AzureFunctions
{
    using Microsoft.Extensions.Logging;

    public class ApplicationInsightsLogger : ILogger
    {
        private readonly Microsoft.Extensions.Logging.ILogger _logger;

        public ApplicationInsightsLogger(Microsoft.Extensions.Logging.ILogger logger)
        {
            _logger = logger ?? throw new System.ArgumentNullException(nameof(logger));
        }

        public void Info(string message)
        {
            _logger.LogInformation(message);
        }

        public void Warning(string message)
        {
            _logger.LogWarning(message);
        }

        public void Error(string message)
        {
            _logger.LogError(message);
        }

        public void Debug(string message)
        {
            _logger.LogDebug(message);
        }
    }
}
