namespace Aylos.Xrm.Sdk.AzureFunctions
{
    public interface ILoggerFactory
    {
        ILogger GetLogger(Microsoft.Extensions.Logging.ILogger logger);
    }
}
