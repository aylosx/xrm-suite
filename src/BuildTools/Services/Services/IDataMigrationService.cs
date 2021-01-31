namespace Aylos.Xrm.Sdk.BuildTools.Services
{
    using System;

    public interface IDataMigrationService : IDisposable
    {
        bool ExportData(string schemaFile, string outputFile);

        void HideData(string inputFile, string outputFile, string primaryKey, string attributeName, string attributeValue);

        bool ImportData(string inputFile);

        void RemoveData(string inputFile, string outputFile, string attributeName, string attributeValue);

        void ReplaceData(string inputFile, string outputFile, string findText, string replaceWithText);

        void UpdateData(string entityName, string primaryKey, string attributeName, string attributeValue);
    }
}