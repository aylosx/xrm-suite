namespace Aylos.Xrm.Sdk.BuildTools.Services
{
    using Aylos.Xrm.Sdk.Common;
    using Aylos.Xrm.Sdk.ConsoleApps;
    using Aylos.Xrm.Sdk.BuildTools.Models.Domain;
    using Aylos.Xrm.Sdk.Exceptions;
    using Microsoft.Xrm.Sdk;
    using Microsoft.Xrm.Tooling.Connector;
    using Microsoft.Xrm.Tooling.Dmt.DataMigCommon.Utility;
    using Microsoft.Xrm.Tooling.Dmt.ExportProcessor.DataInteraction;
    using Microsoft.Xrm.Tooling.Dmt.ImportProcessor.DataInteraction;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Xml.Linq;

    public sealed class DataMigrationService : ConsoleService<CrmServiceContext>, IDataMigrationService
    {
        ICrmService CrmService { get; set; }

        bool disposedValue;

        string SystemTypeName { get { return GetType().UnderlyingSystemType.Name; } }

        public DataMigrationService() : base()
        {
            Logger.Trace(CultureInfo.InvariantCulture, TraceMessageHelper.EnteredMethod, SystemTypeName, MethodBase.GetCurrentMethod().Name);

            Logger.Trace(CultureInfo.InvariantCulture, TraceMessageHelper.ExitingMethod, SystemTypeName, MethodBase.GetCurrentMethod().Name);
        }

        public DataMigrationService(CrmServiceClient crmServiceClient) : base(crmServiceClient)
        {
            Logger.Trace(CultureInfo.InvariantCulture, TraceMessageHelper.EnteredMethod, SystemTypeName, MethodBase.GetCurrentMethod().Name);

            Logger.Trace(CultureInfo.InvariantCulture, TraceMessageHelper.ExitingMethod, SystemTypeName, MethodBase.GetCurrentMethod().Name);
        }

        public DataMigrationService(CrmServiceContext organizationServiceContext, ICrmService crmService) : base(organizationServiceContext)
        {
            Logger.Trace(CultureInfo.InvariantCulture, TraceMessageHelper.EnteredMethod, SystemTypeName, MethodBase.GetCurrentMethod().Name);

            CrmService = crmService ?? throw new ArgumentNullException(nameof(crmService));

            Logger.Trace(CultureInfo.InvariantCulture, TraceMessageHelper.ExitingMethod, SystemTypeName, MethodBase.GetCurrentMethod().Name);
        }

        public bool ExportData(string schemaFile, string outputFile)
        {
            Logger.Trace(CultureInfo.InvariantCulture, TraceMessageHelper.EnteredMethod, SystemTypeName, MethodBase.GetCurrentMethod().Name);

            if (string.IsNullOrWhiteSpace(schemaFile)) throw new ArgumentNullException(nameof(schemaFile));
            Logger.Info(CultureInfo.InvariantCulture, "Schema file: {0}.", schemaFile);

            if (!File.Exists(schemaFile)) throw new FileNotFoundException("Schema file does not exist.");

            if (string.IsNullOrWhiteSpace(outputFile)) throw new ArgumentNullException(nameof(outputFile));
            Logger.Info(CultureInfo.InvariantCulture, "Output file: {0}.", outputFile);

            if (!Directory.Exists(Path.GetDirectoryName(outputFile))) throw new DirectoryNotFoundException("Path to save the output file does not exist.");

            Logger.Info("Exporting data started.");
            Logger.Warn("This feature is experimental.");

            ExportDataHandler edh = new ExportDataHandler(CrmServiceClient);
            edh.AddNewProgressItem += new EventHandler<ProgressItemEventArgs>(LogMigrationProgress);
            edh.UpdateProgressItem += new EventHandler<ProgressItemEventArgs>(LogMigrationProgress);
            bool result = edh.ExportData(schemaFile, outputFile);

            Logger.Info("Data exported successfully.");

            Logger.Trace(CultureInfo.InvariantCulture, TraceMessageHelper.ExitingMethod, SystemTypeName, MethodBase.GetCurrentMethod().Name);

            return result;
        }

        public void HideData(string inputFile, string outputFile, string primaryKey, string attributeName, string attributeValue)
        {
            Logger.Trace(CultureInfo.InvariantCulture, TraceMessageHelper.EnteredMethod, SystemTypeName, MethodBase.GetCurrentMethod().Name);

            if (string.IsNullOrWhiteSpace(inputFile)) throw new ArgumentNullException(nameof(inputFile));
            Logger.Info(CultureInfo.InvariantCulture, "Input file: {0}.", inputFile);

            if (!File.Exists(inputFile)) throw new FileNotFoundException("Input file does not exist.");

            if (string.IsNullOrWhiteSpace(outputFile)) throw new ArgumentNullException(nameof(outputFile));
            Logger.Info(CultureInfo.InvariantCulture, "Output file: {0}.", outputFile);

            if (!Directory.Exists(Path.GetDirectoryName(outputFile))) throw new DirectoryNotFoundException("Path to save the output file does not exist.");

            if (string.IsNullOrWhiteSpace(primaryKey)) throw new ArgumentNullException(nameof(primaryKey));
            Logger.Info(CultureInfo.InvariantCulture, "Primary key: {0}.", primaryKey);

            if (string.IsNullOrWhiteSpace(attributeName)) throw new ArgumentNullException(nameof(attributeName));
            Logger.Info(CultureInfo.InvariantCulture, "Field attribute name: {0}.", attributeName);

            if (string.IsNullOrWhiteSpace(attributeValue)) throw new ArgumentNullException(nameof(attributeValue));
            Logger.Info(CultureInfo.InvariantCulture, "Field attribute value: {0}.", attributeValue);

            Logger.Info(CultureInfo.InvariantCulture, "Input file: {0}.", Path.GetDirectoryName(inputFile));

            Logger.Info("Hiding data started.");

            XDocument xd = XDocument.Load(inputFile);

            XElement record = (
                from x in xd.Root.Descendants("record")
                where x.Attribute("id").Value.ToUpperInvariant() == primaryKey.ToUpperInvariant()
                select x).SingleOrDefault();

            if (record == null)
            {
                Logger.Warn("Record element not found in the XML document.");
                Logger.Trace(CultureInfo.InvariantCulture, TraceMessageHelper.AbortingMethod, SystemTypeName, MethodBase.GetCurrentMethod().Name);
                return;
            }

            XElement field = (
                from x in record.Descendants("field")
                where x.Attribute("name").Value.ToUpperInvariant() == attributeName.ToUpperInvariant()
                select x).SingleOrDefault();

            if (field == null)
            {
                Logger.Warn("Field element not found in the XML document.");
                Logger.Trace(CultureInfo.InvariantCulture, TraceMessageHelper.AbortingMethod, SystemTypeName, MethodBase.GetCurrentMethod().Name);
                return;
            }

            field.SetAttributeValue("value", attributeValue);

            xd.Save(outputFile, SaveOptions.None);

            Logger.Info("Data hidden successfully.");

            Logger.Trace(CultureInfo.InvariantCulture, TraceMessageHelper.ExitingMethod, SystemTypeName, MethodBase.GetCurrentMethod().Name);
        }

        public bool ImportData(string inputFile)
        {
            Logger.Trace(CultureInfo.InvariantCulture, TraceMessageHelper.EnteredMethod, SystemTypeName, MethodBase.GetCurrentMethod().Name);

            if (string.IsNullOrWhiteSpace(inputFile)) throw new ArgumentNullException(nameof(inputFile));

            inputFile = Path.GetFullPath(inputFile);
            Logger.Info(CultureInfo.InvariantCulture, "Input file: {0}.", inputFile);

            if (!File.Exists(inputFile)) throw new FileNotFoundException("Input file does not exist.");

            Logger.Info("Importing data started.");

            string workingFolder = string.Format(CultureInfo.InvariantCulture, @"{0}\{1}", Path.GetDirectoryName(inputFile), Path.GetFileNameWithoutExtension(inputFile));

            ImportCrmDataHandler.CrackZipFileAndCheckContents(inputFile, workingFolder, out workingFolder);

            var idh = new ImportCrmDataHandler
            {
                CrmConnection = CrmServiceClient,
                ImportConnections = new Dictionary<int, CrmServiceClient>
                {
                    { 1, CrmServiceClient }
                }
            };
            idh.AddNewProgressItem += new EventHandler<ProgressItemEventArgs>(LogMigrationProgress);
            idh.UpdateProgressItem += new EventHandler<ProgressItemEventArgs>(LogMigrationProgress);
            idh.ValidateSchemaFile(workingFolder);

            bool result = idh.ImportDataToCrm(workingFolder, false);

            var di = new DirectoryInfo(workingFolder);
            di.Delete(true);

            Logger.Info("Data imported successfully.");

            Logger.Trace(CultureInfo.InvariantCulture, TraceMessageHelper.ExitingMethod, SystemTypeName, MethodBase.GetCurrentMethod().Name);

            return result;
        }

        public void RemoveData(string inputFile, string outputFile, string attributeName, string attributeValue)
        {
            Logger.Trace(CultureInfo.InvariantCulture, TraceMessageHelper.EnteredMethod, SystemTypeName, MethodBase.GetCurrentMethod().Name);

            if (string.IsNullOrWhiteSpace(inputFile)) throw new ArgumentNullException(nameof(inputFile));
            Logger.Info(CultureInfo.InvariantCulture, "Input file: {0}.", inputFile);
            if (!File.Exists(inputFile)) throw new FileNotFoundException("Input file does not exist.");

            if (string.IsNullOrWhiteSpace(outputFile)) throw new ArgumentNullException(nameof(outputFile));
            Logger.Info(CultureInfo.InvariantCulture, "Output file: {0}.", outputFile);
            if (!Directory.Exists(Path.GetDirectoryName(outputFile))) throw new DirectoryNotFoundException("Path to save the output file does not exist.");

            if (string.IsNullOrWhiteSpace(attributeName)) throw new ArgumentNullException(nameof(attributeName));
            Logger.Info(CultureInfo.InvariantCulture, "Field attribute name: {0}.", attributeName);

            if (string.IsNullOrWhiteSpace(attributeValue)) throw new ArgumentNullException(nameof(attributeValue));
            Logger.Info(CultureInfo.InvariantCulture, "Field attribute value: {0}.", attributeValue);

            Logger.Info(CultureInfo.InvariantCulture, "Input: {0}.", Path.GetDirectoryName(inputFile));

            Logger.Info("Removing data started.");

            XDocument xd = XDocument.Load(inputFile);
            RemoveRecordNodes(xd);
            RemoveRecordNodes(xd, attributeName, attributeValue);
            RemoveEntityNodes(xd);
            xd.Save(outputFile, SaveOptions.None);

            Logger.Info("Data removed successfully.");

            Logger.Trace(CultureInfo.InvariantCulture, TraceMessageHelper.ExitingMethod, SystemTypeName, MethodBase.GetCurrentMethod().Name);
        }

        public void ReplaceData(string inputFile, string outputFile, string findText, string replaceWithText)
        {
            Logger.Trace(CultureInfo.InvariantCulture, TraceMessageHelper.EnteredMethod, SystemTypeName, MethodBase.GetCurrentMethod().Name);

            if (string.IsNullOrWhiteSpace(inputFile)) throw new ArgumentNullException(nameof(inputFile));
            Logger.Info(CultureInfo.InvariantCulture, "Input file: {0}.", inputFile);
            if (!File.Exists(inputFile)) throw new FileNotFoundException("Input file does not exist.");

            if (string.IsNullOrWhiteSpace(outputFile)) throw new ArgumentNullException(nameof(outputFile));
            Logger.Info(CultureInfo.InvariantCulture, "Output file: {0}.", outputFile);
            if (!Directory.Exists(Path.GetDirectoryName(outputFile))) throw new DirectoryNotFoundException("Path to save the output file does not exist.");

            if (string.IsNullOrWhiteSpace(findText)) throw new ArgumentNullException(nameof(findText));
            Logger.Info(CultureInfo.InvariantCulture, "Find text: {0}.", findText);

            Logger.Info(CultureInfo.InvariantCulture, "Input: {0}.", Path.GetDirectoryName(inputFile));

            Logger.Info("Replacing data started.");

            StreamReader sr = null;
            StreamWriter sw = null;
            try
            {
                sr = new StreamReader(inputFile);
                string fileContents = sr.ReadToEnd();
                sr.Close();
                sr = null;

                sw = new StreamWriter(inputFile, false);
                sw.Write(fileContents.Replace(findText, replaceWithText));
                sw.Close();
                sw = null;
            } 
            finally 
            {
                if (sr != null) sr.Dispose();
                if (sw != null) sw.Dispose();
            }

            Logger.Info("Data replaced successfully.");

            Logger.Trace(CultureInfo.InvariantCulture, TraceMessageHelper.ExitingMethod, SystemTypeName, MethodBase.GetCurrentMethod().Name);
        }

        public void UpdateData(string entityName, string primaryKey, string attributeName, string attributeValue)
        {
            Logger.Trace(CultureInfo.InvariantCulture, TraceMessageHelper.EnteredMethod, SystemTypeName, MethodBase.GetCurrentMethod().Name);

            if (string.IsNullOrWhiteSpace(entityName)) throw new ArgumentNullException(nameof(entityName));
            entityName = entityName.ToLowerInvariant();
            Logger.Info(CultureInfo.InvariantCulture, "Entity name: {0}.", entityName);

            if (string.IsNullOrWhiteSpace(primaryKey)) throw new ArgumentNullException(nameof(primaryKey));
            Logger.Info(CultureInfo.InvariantCulture, "Primary key: {0}.", primaryKey);

            if (string.IsNullOrWhiteSpace(attributeName)) throw new ArgumentNullException(nameof(attributeName));
            attributeName = attributeName.ToLowerInvariant();
            Logger.Info(CultureInfo.InvariantCulture, "Attribute name: {0}.", attributeName);

            if (string.IsNullOrWhiteSpace(attributeValue)) throw new ArgumentNullException(nameof(attributeValue));

            Logger.Info("Updating entity started.");

            Entity entity = new Entity(entityName);
            entity[string.Format(CultureInfo.InvariantCulture, "{0}id", entityName)] = new Guid(primaryKey);
            entity[attributeName] = attributeValue;

            CrmService.UpdateEntity(entity);

            Logger.Info("Entity updated successfully.");

            Logger.Trace(CultureInfo.InvariantCulture, TraceMessageHelper.ExitingMethod, SystemTypeName, MethodBase.GetCurrentMethod().Name);
        }

        private void LogMigrationProgress(object sender, ProgressItemEventArgs e)
        {
            switch (e.progressItem.ItemStatus)
            {
                case ProgressItemStatus.Complete:
                case ProgressItemStatus.Working:
                    Logger.Info(e.progressItem.ItemText);
                    break;
                case ProgressItemStatus.Failed:
                case ProgressItemStatus.Unknown:
                    Logger.Error(e.progressItem.ItemText);
                    throw new PlatformException(e.progressItem.ItemText);
                case ProgressItemStatus.Warning:
                    Logger.Warn(e.progressItem.ItemText);
                    break;
            }
        }

        private void RemoveRecordNodes(XDocument xd)
        {
            Logger.Trace(CultureInfo.InvariantCulture, TraceMessageHelper.EnteredMethod, SystemTypeName, MethodBase.GetCurrentMethod().Name);

            if (xd == null) throw new ArgumentNullException(nameof(xd));

            IList<XElement> elements = (
                from x in xd.Root.Descendants("field")
                where
                    x.Attribute("name").Value.ToUpperInvariant() == "adx_name".ToUpperInvariant() &&
                    x.Attribute("value").Value.ToUpperInvariant() == "PackageImportComplete".ToUpperInvariant()
                select x.Parent).ToList();

            Logger.Trace(CultureInfo.InvariantCulture, "Found {0} records.", elements.Count);
            if (elements.Count > 0)
            {
                for (int i = elements.Count - 1; i >= 0; i--)
                {
                    RemoveRecordNodes(xd, elements[i], "adx_name");
                }
            }

            Logger.Trace(CultureInfo.InvariantCulture, TraceMessageHelper.ExitingMethod, SystemTypeName, MethodBase.GetCurrentMethod().Name);
        }

        private void RemoveRecordNodes(XDocument xd, string attributeName, string attributeValue)
        {
            Logger.Trace(CultureInfo.InvariantCulture, TraceMessageHelper.EnteredMethod, SystemTypeName, MethodBase.GetCurrentMethod().Name);

            if (xd == null) throw new ArgumentNullException(nameof(xd));

            IList<XElement> elements = (
                from x in xd.Root.Descendants("field")
                where
                    x.Attribute("name").Value.ToUpperInvariant() == attributeName.ToUpperInvariant() &&
                    x.Attribute("value").Value.ToUpperInvariant() == attributeValue.ToUpperInvariant()
                select x.Parent).ToList();

            Logger.Trace(CultureInfo.InvariantCulture, "Found {0} records.", elements.Count);
            if (elements.Count > 0)
            {
                for (int i = elements.Count - 1; i >= 0; i--)
                {
                    RemoveRecordNodes(xd, elements[i], attributeName);
                }
            }

            Logger.Trace(CultureInfo.InvariantCulture, TraceMessageHelper.ExitingMethod, SystemTypeName, MethodBase.GetCurrentMethod().Name);
        }

        private void RemoveEntityNodes(XDocument xd)
        {
            Logger.Trace(CultureInfo.InvariantCulture, TraceMessageHelper.EnteredMethod, SystemTypeName, MethodBase.GetCurrentMethod().Name);

            if (xd == null) throw new ArgumentNullException(nameof(xd));

            IList<XElement> entities = (
                from x in xd.Root.Descendants("entity")
                select x).ToList();

            Logger.Trace(CultureInfo.InvariantCulture, "Found {0} entities.", entities.Count);

            if (entities.Count > 0)
            {
                XAttribute timestamp = entities[0].Parent.Attribute("timestamp");
                if (timestamp != null) timestamp.Remove();
                for (int i = entities.Count - 1; i >= 0; i--)
                {
                    IList<XElement> records = (
                        from x in entities[i].Descendants("record")
                        select x).ToList();

                    if (records.Count == 0) entities[i].Remove();
                }
            }

            Logger.Trace(CultureInfo.InvariantCulture, TraceMessageHelper.ExitingMethod, SystemTypeName, MethodBase.GetCurrentMethod().Name);
        }

        private void RemoveRecordNodes(XDocument xd, XElement element, string attributeName)
        {
            Logger.Trace(CultureInfo.InvariantCulture, TraceMessageHelper.EnteredMethod, SystemTypeName, MethodBase.GetCurrentMethod().Name);

            if (xd == null) throw new ArgumentNullException(nameof(xd));
            if (element == null) throw new ArgumentNullException(nameof(element));

            RemoveRelationshipNodes(xd, element);

            RemoveLookupNodes(xd, element, attributeName);

            element.Remove();

            Logger.Trace(CultureInfo.InvariantCulture, TraceMessageHelper.ExitingMethod, SystemTypeName, MethodBase.GetCurrentMethod().Name);
        }

        private void RemoveLookupNodes(XDocument xd, XElement element, string attributeName)
        {
            Logger.Trace(CultureInfo.InvariantCulture, TraceMessageHelper.EnteredMethod, SystemTypeName, MethodBase.GetCurrentMethod().Name);

            if (xd == null) throw new ArgumentNullException(nameof(xd));
            if (element == null) throw new ArgumentNullException(nameof(element));

            Logger.Trace(CultureInfo.InvariantCulture, "Looking for references with name: {0} and value: {1}.",
                element.Parent.Parent.Attribute("name").Value,
                element.Attribute("id").Value
                );

            IList<XElement> elements = (
                from x in xd.Root.Descendants("field")
                where
                    x.Attribute("name").Value.ToUpperInvariant() == element.Parent.Parent.Attribute("name").Value.ToUpperInvariant() &&
                    x.Attribute("lookupentity").Value.ToUpperInvariant() == element.Parent.Parent.Attribute("name").Value.ToUpperInvariant() &&
                    x.Attribute("value").Value.ToUpperInvariant() == element.Attribute("id").Value.ToUpperInvariant()
                select x.Parent).ToList();

            Logger.Trace(CultureInfo.InvariantCulture, "Found {0} references.", elements.Count);

            if (elements.Count > 0)
            {
                for (int i = elements.Count - 1; i >= 0; i--)
                {
                    IList<XElement> excludedElements = (
                        from x in elements[i].Descendants("field")
                        where x.Attribute("name").Value.ToUpperInvariant() == attributeName.ToUpperInvariant()
                        select x.Parent).ToList();

                    if (excludedElements.Count == 0)
                    {
                        RemoveRecordNodes(xd, elements[i], attributeName);
                    }
                }
            }

            Logger.Trace(CultureInfo.InvariantCulture, TraceMessageHelper.ExitingMethod, SystemTypeName, MethodBase.GetCurrentMethod().Name);
        }

        private void RemoveRelationshipNodes(XDocument xd, XElement element)
        {
            Logger.Trace(CultureInfo.InvariantCulture, TraceMessageHelper.EnteredMethod, SystemTypeName, MethodBase.GetCurrentMethod().Name);

            if (xd == null) throw new ArgumentNullException(nameof(xd));
            if (element == null) throw new ArgumentNullException(nameof(element));

            Logger.Trace(CultureInfo.InvariantCulture, "Searching for relationships with source id: {0}.", element.Attribute("id").Value);

            IList<XElement> elements = (
                from x in xd.Root.Descendants("m2mrelationship")
                where x.Attribute("sourceid").Value.ToUpperInvariant() == element.Attribute("id").Value.ToUpperInvariant()
                select x).ToList();

            Logger.Trace(CultureInfo.InvariantCulture, "Found {0} relationships.", elements.Count);

            if (elements.Count > 0)
            {
                for (int i = elements.Count - 1; i >= 0; i--)
                {
                    elements[i].Remove();
                }
            }

            Logger.Trace(CultureInfo.InvariantCulture, TraceMessageHelper.ExitingMethod, SystemTypeName, MethodBase.GetCurrentMethod().Name);
        }

        void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (CrmService != null) CrmService.Dispose();
                    if (OrganizationServiceContext != null) OrganizationServiceContext.Dispose();
                }
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}