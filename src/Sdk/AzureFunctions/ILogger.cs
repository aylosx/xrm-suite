namespace Aylos.Xrm.Sdk.AzureFunctions
{
    public interface ILogger
    {
        void Info(string message);

        void Warning(string message);

        void Error(string message);

        void Debug(string message);
    }
}
