namespace Aylos.Xrm.Sdk.Common
{
    public enum PluginAssemblyIsolationMode
    {
        Unknown = 0,
        None = 1,
        Sandbox = 2,
    }

    public enum SdkMessageProcessingStepStage
    {
        Unknown = 0,
        PreValidation = 10,
        PreOperation = 20,
        MainOperation = 30,
        PostOperation = 40,
    }
}
