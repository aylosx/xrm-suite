namespace Aylos.Xrm.Sdk.AzureFunctions
{
    using Microsoft.Azure.WebJobs.Host;

    public interface ILoggerFactory
    {
#pragma warning disable CS0618 // Type or member is obsolete
        ILogger GetLogger(TraceWriter logger);
#pragma warning restore CS0618 // Type or member is obsolete

        ILogger GetLogger(Microsoft.Extensions.Logging.ILogger logger);
    }
}
