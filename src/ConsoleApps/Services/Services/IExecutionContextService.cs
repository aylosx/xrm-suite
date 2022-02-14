namespace Aylos.Xrm.Sdk.ConsoleApps.Services
{
    using System;

    public interface IExecutionContextService : IDisposable
    {
        void ProcessExecutionContexts();
    }
}