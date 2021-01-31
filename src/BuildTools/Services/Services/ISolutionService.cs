namespace Aylos.Xrm.Sdk.BuildTools.Services
{
    using System;

    public interface ISolutionService : IDisposable
    {
        void ExportSolutions(string solutionName, int solutionType, string outputPath, bool includeVersion);

        void ImportSolution(string solutionName, string inputFile, bool holdingSolution, bool overwriteUnmanagedCustomizations, bool publishWorkflows, bool skipProductUpdateDependencies, int pollingInterval, int pollingTimeout);

        void RemoveSolutionComponent(string solutionName, string componentId, int componentType);

        void UpdateSolutionVersion(string solutionName, string timeZone, string version);
    }
}