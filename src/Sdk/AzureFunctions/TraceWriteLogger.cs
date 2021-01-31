namespace Aylos.Xrm.Sdk.AzureFunctions
{
    using Microsoft.Azure.WebJobs.Host;

    public class TraceWriteLogger : ILogger
    {
#pragma warning disable CS0618 // Type or member is obsolete
        private readonly TraceWriter _traceWriter;
#pragma warning restore CS0618 // Type or member is obsolete

#pragma warning disable CS0618 // Type or member is obsolete
        public TraceWriteLogger(TraceWriter traceWriter)
#pragma warning restore CS0618 // Type or member is obsolete
        {
            _traceWriter = traceWriter;
        }

        public void Info(string message)
        {
            _traceWriter.Info(message);
        }

        public void Warning(string message)
        {
            _traceWriter.Warning(message);
        }

        public void Error(string message)
        {
            _traceWriter.Error(message);
        }

        public void Debug(string message)
        {
            _traceWriter.Verbose(message);
        }
    }
}
