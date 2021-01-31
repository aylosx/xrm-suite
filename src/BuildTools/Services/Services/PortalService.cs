namespace Aylos.Xrm.Sdk.BuildTools.Services
{
    using Aylos.Xrm.Sdk.Common;
    using Aylos.Xrm.Sdk.ConsoleApps;
    using Aylos.Xrm.Sdk.BuildTools.Models.Domain;
    using Aylos.Xrm.Sdk.Exceptions;
    using Microsoft.Xrm.Sdk;
    using Microsoft.Xrm.Sdk.Client;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Threading;
    using System.Web;

    public sealed class PortalService : ConsoleService<CrmServiceContext>, IPortalService
    {
        ICrmService CrmService { get; set; }

        bool disposedValue;

        string SystemTypeName { get { return GetType().UnderlyingSystemType.Name; } }

        public PortalService(CrmServiceContext organizationServiceContext, ICrmService crmService) : base(organizationServiceContext)
        {
            Logger.Trace(CultureInfo.InvariantCulture, TraceMessageHelper.EnteredMethod, SystemTypeName, MethodBase.GetCurrentMethod().Name);

            CrmService = crmService ?? throw new ArgumentNullException(nameof(crmService));

            Logger.Trace(CultureInfo.InvariantCulture, TraceMessageHelper.ExitingMethod, SystemTypeName, MethodBase.GetCurrentMethod().Name);
        }

        public void ExportWebFiles(string outputPath, string websitePrimaryKey)
        {
            Logger.Trace(CultureInfo.InvariantCulture, TraceMessageHelper.EnteredMethod, SystemTypeName, MethodBase.GetCurrentMethod().Name);

            if (string.IsNullOrWhiteSpace(outputPath)) throw new ArgumentNullException(nameof(outputPath));
            Logger.Info(CultureInfo.InvariantCulture, "Output path: {0}.", outputPath);

            if (!Directory.Exists(Path.GetDirectoryName(outputPath))) throw new ArgumentException("The output file path does not exist.");

            if (string.IsNullOrWhiteSpace(websitePrimaryKey)) throw new ArgumentNullException(nameof(websitePrimaryKey));
            Logger.Info(CultureInfo.InvariantCulture, "Web site primary key: {0}.", websitePrimaryKey);

            bool isValid = Guid.TryParse(websitePrimaryKey, out Guid websiteId);
            if (!isValid) throw new ArgumentException("The web site primary key is not valid");

            IList<FileInfo> files = new List<FileInfo>();
            EnumerateFiles(outputPath, files);

            IList<WebFile> webFiles = CrmService.GetWebsiteFiles(websiteId);
            foreach (WebFile webFile in webFiles.OrderBy(x => x.Name))
            {
                Logger.Info(CultureInfo.InvariantCulture, "Processing web file {0}.", webFile.Name);

                Note annotation = CrmService.GetAnnotations(webFile.Id).OrderByDescending(x => x.CreatedOn).FirstOrDefault();
                if (annotation == null)
                {
                    Logger.Warn(CultureInfo.InvariantCulture, "No annotation exists for {0} web file.", webFile.Name);
                }
                else
                {
                    string path = Path.Combine(outputPath, annotation.FileName);
                    byte[] bytes = Convert.FromBase64String(annotation.Document);
                    File.WriteAllBytes(path, bytes);

                    Logger.Info(CultureInfo.InvariantCulture, "File {0} written.", path);
                }
            }

            Logger.Trace(CultureInfo.InvariantCulture, TraceMessageHelper.ExitingMethod, SystemTypeName, MethodBase.GetCurrentMethod().Name);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling")]
        public void UpdateWebFiles(string inputPath, string websitePrimaryKey, string blockedAttachments)
        {
            Logger.Trace(CultureInfo.InvariantCulture, TraceMessageHelper.EnteredMethod, SystemTypeName, MethodBase.GetCurrentMethod().Name);

            if (string.IsNullOrWhiteSpace(inputPath)) throw new ArgumentNullException(nameof(inputPath));
            Logger.Info(CultureInfo.InvariantCulture, "Input path: {0}.", inputPath);

            if (!Directory.Exists(Path.GetDirectoryName(inputPath))) throw new FileNotFoundException("The input file path does not exist.");

            if (string.IsNullOrWhiteSpace(websitePrimaryKey)) throw new ArgumentNullException(nameof(websitePrimaryKey));
            Logger.Info(CultureInfo.InvariantCulture, "Web site primary key: {0}.", websitePrimaryKey);

            bool isValid = Guid.TryParse(websitePrimaryKey, out Guid websiteId);
            if (!isValid) throw new ArgumentException("The web site primary key is not valid");

            IList<FileInfo> files = new List<FileInfo>();

            EnumerateFiles(inputPath, files);

            UpdateOrganizationSettings("js;", blockedAttachments);

            IList<WebFile> webFiles = CrmService.GetWebsiteFiles(websiteId);

            OrganizationServiceContext.ClearChanges();

            foreach (FileInfo file in files)
            {
                Logger.Info(CultureInfo.InvariantCulture, "Processing file {0}.", file.FullName);
                WebFile webFile = webFiles.SingleOrDefault(x => x.Name.ToUpperInvariant().Equals(file.Name.ToUpperInvariant()));
                if (webFile == null) {
                    Logger.Warn(CultureInfo.InvariantCulture, "There is no portal web file entity for the file {0}.", file.Name);
                    continue;
                }

                Note annotation = CrmService.GetAnnotations(webFile.Id).OrderByDescending(x => x.CreatedOn).FirstOrDefault();

                if (annotation == null)
                {
                    Logger.Info(CultureInfo.InvariantCulture, "A new annotation {0} will be created.", file.Name);

                    annotation = new Note {
                        Document = Convert.ToBase64String(File.ReadAllBytes(file.FullName)),
                        FileName = file.Name, 
                        IsDocument = true, 
                        MimeType = MimeMapping.GetMimeMapping(file.Name),
                        ObjectType = webFile.LogicalName, 
                        Regarding = new EntityReference(webFile.LogicalName, webFile.Id),
                    };

                    OrganizationServiceContext.AddObject(annotation);
                }
                else if (!annotation.Document.Equals(Convert.ToBase64String(File.ReadAllBytes(file.FullName))))
                {
                    Logger.Info(CultureInfo.InvariantCulture, "The annotation {0} will be updated.", file.Name);

                    annotation = new Note
                    {
                        AnnotationId = annotation.Id,
                        Document = Convert.ToBase64String(File.ReadAllBytes(file.FullName))
                    };

                    OrganizationServiceContext.Attach(annotation);
                    OrganizationServiceContext.UpdateObject(annotation);
                }
            }

            CrmService.SaveChanges(OrganizationServiceContext, SaveChangesOptions.None);

            UpdateOrganizationSettings("nothing really", blockedAttachments);

            Logger.Trace(CultureInfo.InvariantCulture, TraceMessageHelper.ExitingMethod, SystemTypeName, MethodBase.GetCurrentMethod().Name);
        }

        public void UpdateWebTemplates(string inputPath, string websitePrimaryKey)
        {
            Logger.Trace(CultureInfo.InvariantCulture, TraceMessageHelper.EnteredMethod, SystemTypeName, MethodBase.GetCurrentMethod().Name);

            if (string.IsNullOrWhiteSpace(inputPath)) throw new ArgumentNullException(nameof(inputPath));
            Logger.Info(CultureInfo.InvariantCulture, "Input path: {0}.", inputPath);

            if (!Directory.Exists(Path.GetDirectoryName(inputPath))) throw new FileNotFoundException("The input file path does not exist.");

            if (string.IsNullOrWhiteSpace(websitePrimaryKey)) throw new ArgumentNullException(nameof(websitePrimaryKey));
            Logger.Info(CultureInfo.InvariantCulture, "Web site primary key: {0}.", websitePrimaryKey);

            bool isValid = Guid.TryParse(websitePrimaryKey, out Guid websiteId);
            if (!isValid) throw new ArgumentException("The web site primary key is not valid");

            IList<FileInfo> files = new List<FileInfo>();
            EnumerateFiles(inputPath, files);

            IList<WebTemplate> webTemplates = CrmService.GetWebTemplates(websiteId);
            OrganizationServiceContext.ClearChanges();

            foreach (FileInfo file in files)
            {
                Logger.Info(CultureInfo.InvariantCulture, "Processing file {0}.", file.FullName);

                string fileName = file.Name.ToUpperInvariant().Replace(".HTML", string.Empty);

                WebTemplate webTemplate = webTemplates.SingleOrDefault(x => x.Name.ToUpperInvariant().Equals(fileName));

                if (webTemplate == null)
                {
                    Logger.Warn(CultureInfo.InvariantCulture, "There is no portal web template entity for the file {0}.", file.Name);
                    continue;
                }

                string source = File.ReadAllText(file.FullName);
                if (source.Equals(webTemplate.Source)) continue;

                var updatedWebTemplate = new WebTemplate
                {
                    Id = webTemplate.Id,
                    Source = source
                };

                OrganizationServiceContext.Attach(updatedWebTemplate);
                OrganizationServiceContext.UpdateObject(updatedWebTemplate);
            }

            CrmService.SaveChanges(OrganizationServiceContext, SaveChangesOptions.None);

            Logger.Trace(CultureInfo.InvariantCulture, TraceMessageHelper.ExitingMethod, SystemTypeName, MethodBase.GetCurrentMethod().Name);
        }

        private void EnumerateFiles(string inputPath, IList<FileInfo> files)
        {
            Logger.Trace(CultureInfo.InvariantCulture, TraceMessageHelper.EnteredMethod, SystemTypeName, MethodBase.GetCurrentMethod().Name);

            if (files == null) throw new ArgumentNullException(nameof(files));
            DirectoryInfo dir = new DirectoryInfo(inputPath);
            foreach (FileInfo fi in dir.GetFiles()) files.Add(fi);
            foreach (DirectoryInfo subDir in dir.GetDirectories()) EnumerateFiles(subDir.FullName, files);

            Logger.Trace(CultureInfo.InvariantCulture, TraceMessageHelper.ExitingMethod, SystemTypeName, MethodBase.GetCurrentMethod().Name);
        }

        private void UpdateOrganizationSettings(string replaceString, string blockedAttachments)
        {
            Logger.Trace(CultureInfo.InvariantCulture, TraceMessageHelper.EnteredMethod, SystemTypeName, MethodBase.GetCurrentMethod().Name);

            if (string.IsNullOrWhiteSpace(blockedAttachments)) throw new ArgumentNullException(nameof(blockedAttachments));

            Logger.Info(CultureInfo.InvariantCulture, "Updating blocked attachments.");

            Organization organizationSettings = CrmService.GetOrganizationSettings();
            if (organizationSettings == null) throw new EntityNotFoundException("No organization record can be found in the system.");

            OrganizationServiceContext.ClearChanges();

            Organization updatedOrganizationSettings = new Organization
            {
                Id = organizationSettings.Id,
                BlockAttachments = blockedAttachments.Replace(replaceString, string.Empty),
            };

            OrganizationServiceContext.Attach(updatedOrganizationSettings);

            OrganizationServiceContext.UpdateObject(updatedOrganizationSettings);

            CrmService.SaveChanges(OrganizationServiceContext, SaveChangesOptions.None);

            Thread.Sleep(60000);

            Logger.Info(CultureInfo.InvariantCulture, "Blocked attachments updated to: {0}.", updatedOrganizationSettings.BlockAttachments);

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
