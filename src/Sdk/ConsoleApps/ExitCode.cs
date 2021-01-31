namespace Aylos.Xrm.Sdk.ConsoleApps
{
    public enum ExitCode
    {
        None = -1,
        Success = 0,
        NotImplemented = 1,
        ArgumentSyntaxError = 2,
        ExecutionError = 3,
        OrganizationServiceFault = 4,
        ConfigurationDataExportFailed = 7,
    }
}
