namespace Aylos.Xrm.Sdk.BuildTools.Services
{
    using Aylos.Xrm.Sdk.BuildTools.Models.Domain;
    using Aylos.Xrm.Sdk.Common;
    using Aylos.Xrm.Sdk.ConsoleApps;
    using Aylos.Xrm.Sdk.Exceptions;

    using Microsoft.Crm.Sdk.Messages;
    using Microsoft.Xrm.Sdk;
    using Microsoft.Xrm.Sdk.Client;
    using Microsoft.Xrm.Sdk.Messages;

    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.ServiceModel;
    using System.ServiceModel.Security;
    using System.Threading;
    using System.Xml.Linq;

    public sealed class SolutionService : ConsoleService<CrmServiceContext>, ISolutionService
    {
        // Warning!!! The following message might be changed by Microsoft in the future.
        const string SolutionVersionMustBeHigherMessage = "The import solution must have a higher version than the existing solution it is upgrading.";

        bool disposedValue;

        string SystemTypeName { get { return GetType().UnderlyingSystemType.Name; } }

        readonly CancellationTokenSource cts = new CancellationTokenSource();

        public SolutionService(CrmServiceContext organizationServiceContext, Connection connection, ICrmService crmService) : base(organizationServiceContext, connection)
        {
            Logger.Trace(CultureInfo.InvariantCulture, TraceMessageHelper.EnteredMethod, SystemTypeName, MethodBase.GetCurrentMethod().Name);

            CrmService = crmService ?? throw new ArgumentNullException(nameof(crmService));

            Logger.Trace(CultureInfo.InvariantCulture, TraceMessageHelper.ExitingMethod, SystemTypeName, MethodBase.GetCurrentMethod().Name);
        }

        ICrmService CrmService { get; set; }

        public void DeleteSolution(string uniqueName)
        {
            Logger.Trace(CultureInfo.InvariantCulture, TraceMessageHelper.EnteredMethod, SystemTypeName, MethodBase.GetCurrentMethod().Name);

            if (string.IsNullOrWhiteSpace(uniqueName)) throw new ArgumentNullException(nameof(uniqueName));

            Solution solution = CrmService.GetSolutions(uniqueName).SingleOrDefault();

            if (solution != null)
            {
                Logger.Info(CultureInfo.InvariantCulture, "Deleting solution {0}.", uniqueName);
                CrmService.DeleteSolution(solution.Id);
            }

            Logger.Trace(CultureInfo.InvariantCulture, TraceMessageHelper.ExitingMethod, SystemTypeName, MethodBase.GetCurrentMethod().Name);
        }

        public void ExportSolutions(string solutionName, int solutionType, string outputPath, bool includeVersion)
        {
            Logger.Trace(CultureInfo.InvariantCulture, TraceMessageHelper.EnteredMethod, SystemTypeName, MethodBase.GetCurrentMethod().Name);

            if (string.IsNullOrWhiteSpace(solutionName)) throw new ArgumentNullException(nameof(solutionName));
            Logger.Info(CultureInfo.InvariantCulture, "Solution(s) pattern: {0}.", solutionName);

            if (solutionName.Length < 3) throw new ArgumentException("Solution name length should be at least three characters.");

            Logger.Info(CultureInfo.InvariantCulture, "Solution(s) type: {0}.", solutionType);

            if (solutionType < 0 || solutionType > 2) throw new ArgumentException("Solution type can be '0' for unmanaged, '1' for managed or '2' for both.");

            if (string.IsNullOrWhiteSpace(outputPath)) throw new ArgumentNullException(nameof(outputPath));
            outputPath = Path.GetFullPath(outputPath) + @"\";

            Logger.Info(CultureInfo.InvariantCulture, "Output file path: {0}.", outputPath);
            if (!Directory.Exists(outputPath)) throw new ArgumentException("File path does not exist.");

            IList<Solution> solutions;

            if (solutionName.StartsWith("*"))
            {
                solutions = CrmService.GetSolutionsEndsWith(solutionName.Substring(1));
            }
            else if (solutionName.EndsWith("*"))
            {
                solutions = CrmService.GetSolutionsStartsWith(solutionName.Substring(0, solutionName.Length - 1));
            }
            else
            {
                solutions = CrmService.GetSolutions(solutionName);
            }

            foreach (Solution solution in solutions)
            {
                Logger.Info(CultureInfo.InvariantCulture, "Processing solution '{0} {1}'.", solution.DisplayName, solution.Version);

                if (solution.PackageType == true)
                {
                    Logger.Warn("The solution is managed therefore cannot be exported.");
                }
                else
                {
                    string absoluteFilePath;
                    string versionPart = includeVersion ? string.Format(CultureInfo.InvariantCulture, "_{0}", solution.Version.Replace(".", "_")) : string.Empty;
                    string managedSuffix = includeVersion ? "_managed" : string.Empty;
                    switch (solutionType)
                    {
                        case 0:
                            // unmanaged
                            absoluteFilePath = string.Format(CultureInfo.InvariantCulture, "{0}{1}{2}.zip", outputPath, solution.Name, versionPart);
                            ExportSolution(solution.Name, false, absoluteFilePath);
                            break;
                        case 1:
                            // managed
                            absoluteFilePath = string.Format(CultureInfo.InvariantCulture, "{0}{1}{2}{3}.zip", outputPath, solution.Name, versionPart, managedSuffix);
                            ExportSolution(solution.Name, true, absoluteFilePath);
                            break;
                        case 2:
                            // unmanaged
                            absoluteFilePath = string.Format(CultureInfo.InvariantCulture, "{0}{1}{2}.zip", outputPath, solution.Name, versionPart);
                            ExportSolution(solution.Name, false, absoluteFilePath);
                            // managed
                            absoluteFilePath = string.Format(CultureInfo.InvariantCulture, "{0}{1}{2}{3}.zip", outputPath, solution.Name, versionPart, managedSuffix);
                            ExportSolution(solution.Name, true, absoluteFilePath);
                            break;
                    }
                }
            }

            Logger.Trace(CultureInfo.InvariantCulture, TraceMessageHelper.ExitingMethod, SystemTypeName, MethodBase.GetCurrentMethod().Name);
        }

        public void ImportSolution(string solutionName, string inputFile, bool holdingSolution, bool overwriteUnmanagedCustomizations, bool publishWorkflows, bool skipProductUpdateDependencies, int pollingInterval, int pollingTimeout)
        {
            Logger.Trace(CultureInfo.InvariantCulture, TraceMessageHelper.EnteredMethod, SystemTypeName, MethodBase.GetCurrentMethod().Name);

            if (string.IsNullOrWhiteSpace(solutionName)) throw new ArgumentNullException(nameof(solutionName));
            if (string.IsNullOrWhiteSpace(inputFile)) throw new ArgumentNullException(nameof(inputFile));

            inputFile = Path.GetFullPath(inputFile);
            Logger.Info(CultureInfo.InvariantCulture, "Input file: {0}.", inputFile);

            if (!File.Exists(inputFile)) throw new FileNotFoundException("Input file does not exist.");

            Logger.Info(CultureInfo.InvariantCulture, "Holding solution: {0}.", holdingSolution);
            Logger.Info(CultureInfo.InvariantCulture, "Overwrite unmanaged customizations: {0}.", overwriteUnmanagedCustomizations);
            Logger.Info(CultureInfo.InvariantCulture, "Publish workflows: {0}.", publishWorkflows);
            Logger.Info(CultureInfo.InvariantCulture, "Skip product update dependencies: {0}.", skipProductUpdateDependencies);

            Guid importJobId = Guid.NewGuid();
            ExecuteAsyncResponse res;

            byte[] bytes = File.ReadAllBytes(inputFile);

            switch (holdingSolution)
            {
                case false:
                    Logger.Info(CultureInfo.InvariantCulture, "Solution import starting.");
                    res = CrmService.ImportSolutionAsync(importJobId, bytes, false, overwriteUnmanagedCustomizations, publishWorkflows, skipProductUpdateDependencies);
                    Logger.Info(CultureInfo.InvariantCulture, "Solution import started.");
                    WaitForImportJob(importJobId, res.AsyncJobId, pollingInterval, pollingTimeout, true); //values in seconds
                    Logger.Info(CultureInfo.InvariantCulture, "Solution import completed successfully.");
                    break;

                case true:
                    Solution solution = CrmService.GetSolutions(solutionName).SingleOrDefault();
                    if (solution == null)
                    {
                        Logger.Info(CultureInfo.InvariantCulture, "Solution import starting.");
                        res = CrmService.ImportSolutionAsync(importJobId, bytes, false, overwriteUnmanagedCustomizations, publishWorkflows, skipProductUpdateDependencies);
                        Logger.Info(CultureInfo.InvariantCulture, "Solution import started.");
                        WaitForImportJob(importJobId, res.AsyncJobId, pollingInterval, pollingTimeout, true); //values in seconds
                        Logger.Info(CultureInfo.InvariantCulture, "Solution import completed successfully.");
                    }
                    else
                    {
                        try
                        {
                            Logger.Info(CultureInfo.InvariantCulture, "Solution import starting.");
                            res = CrmService.ImportSolutionAsync(importJobId, bytes, true, overwriteUnmanagedCustomizations, publishWorkflows, skipProductUpdateDependencies);
                            Logger.Info(CultureInfo.InvariantCulture, "Solution import started.");
                            WaitForImportJob(importJobId, res.AsyncJobId, pollingInterval, pollingTimeout, true); //values in seconds
                            Logger.Info(CultureInfo.InvariantCulture, "Solution import completed successfully.");
                            res = CrmService.DeleteAndPromoteSolutionAsync(solution.Name);
                            Logger.Info(CultureInfo.InvariantCulture, "Solution upgrade started.");
                            WaitForSystemJob(res.AsyncJobId, pollingInterval, pollingTimeout, false); //values in seconds
                            Logger.Info(CultureInfo.InvariantCulture, "Solution upgrade completed successfully.");
                        }
                        catch (PlatformException ex)
                        {
                            if (!ex.Message.Contains(SolutionVersionMustBeHigherMessage)) throw;
                            Logger.Warn(SolutionVersionMustBeHigherMessage); importJobId = Guid.NewGuid();
                            Logger.Info(CultureInfo.InvariantCulture, "Restarting solution import process with no upgrade option.");
                            res = CrmService.ImportSolutionAsync(importJobId, bytes, false, overwriteUnmanagedCustomizations, publishWorkflows, skipProductUpdateDependencies);
                            Logger.Info(CultureInfo.InvariantCulture, "Solution import process restarted.");
                            WaitForImportJob(importJobId, res.AsyncJobId, pollingInterval, pollingTimeout, true); //values in seconds
                            Logger.Info(CultureInfo.InvariantCulture, "Solution import completed successfully.");
                        }
                    }
                    break;
            }

            Logger.Trace(CultureInfo.InvariantCulture, TraceMessageHelper.ExitingMethod, SystemTypeName, MethodBase.GetCurrentMethod().Name);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling")]
        private void WaitForImportJob(Guid importJobId, Guid systemJobId, int pollingInterval, int pollingTimeout, bool throwsException, int waitedFor = 0)
        {
            Logger.Trace(CultureInfo.InvariantCulture, TraceMessageHelper.EnteredMethod, SystemTypeName, MethodBase.GetCurrentMethod().Name);

            if (Guid.Empty.Equals(systemJobId)) throw new ArgumentNullException(nameof(systemJobId));
            if (pollingInterval < 1) throw new PlatformException("Interval must be 1 second or more.");
            pollingInterval *= 1000; pollingTimeout *= 1000; bool inProgress = true;

            while (inProgress)
            {
                try
                {
                    OrganizationServiceContext.ClearChanges();

                    SystemJob systemJob = CrmService.GetSystemJob(systemJobId);

                    if (systemJob == null) throw new PlatformException("The requested job doesn't exist.");

                    switch (systemJob.StatusReason)
                    {
                        case SystemJobStatusReason.Succeeded:
                            inProgress = false;
                            break;

                        case SystemJobStatusReason.Canceling:
                        case SystemJobStatusReason.Canceled:
                        case SystemJobStatusReason.Failed:
                        case SystemJobStatusReason.Pausing:
                            if (throwsException)
                            {
                                throw new PlatformException(string.Format(CultureInfo.InvariantCulture, "The requested job failed. {0} {1}", systemJob.StatusReason, systemJob.Message));
                            }
                            inProgress = false;
                            break;
                    }

                    ImportJob importJob = CrmService.GetImportJob(importJobId);

                    if (importJob != null && importJob.Progress != null)
                    {
                        int progress = Convert.ToInt32(importJob.Progress.GetValueOrDefault(0));
                        Logger.Info(CultureInfo.InvariantCulture, "Importing solution ... progress: {0}%", progress);
                        if (progress >= 100)
                        {
                            if (!string.IsNullOrWhiteSpace(importJob.Data))
                            {
                                IEnumerable<ImportJobResult> importJobResults = ParseImportJobXmlData(importJob.Data);

                                int warningCount = 0; int errorCount = 0; int fatalErrorCount = 0;

                                foreach (ImportJobResult importJobResult in importJobResults)
                                {
                                    string name = GetNameText(importJobResult);
                                    string processed = GetProcessedText(importJobResult);
                                    string result = GetResultText(importJobResult);

                                    switch (importJobResult.Result)
                                    {
                                        case ImportJobStatus.Warning:
                                            Logger.Warn(CultureInfo.InvariantCulture, "{0}{1}{2}{3}", importJobResult.ElementName, name, processed, result);
                                            warningCount++;
                                            break;

                                        case ImportJobStatus.Error:
                                            Logger.Error(CultureInfo.InvariantCulture, "{0}{1}{2}{3}", importJobResult.ElementName, name, processed, result);
                                            if (importJobResult.ErrorCode != "0") errorCount++;
                                            break;

                                        case ImportJobStatus.Failure:
                                            Logger.Fatal(CultureInfo.InvariantCulture, "{0}{1}{2}{3}", importJobResult.ElementName, name, processed, result);
                                            if (importJobResult.ErrorCode != "0") fatalErrorCount++;
                                            break;

                                        default:
                                            Logger.Info(CultureInfo.InvariantCulture, "{0}{1}{2}{3}", importJobResult.ElementName, name, processed, result);
                                            break;
                                    }
                                }

                                if (errorCount > 0 || fatalErrorCount > 0)
                                {
                                    throw new PlatformException("One or more error(s) occurred whilst importing the solution.");
                                }
                            }

                            inProgress = false;
                        }
                    }
                }
                catch (MessageSecurityException ex)
                {
                    Logger.Warn(CultureInfo.InvariantCulture, "Oups ... a security exception occurred, we will try to reconnect.");
                    Logger.Warn(ex.Message);

                    Connection.SetOrganizationService();

                    OrganizationServiceContext.Dispose();
                    OrganizationServiceContext = new CrmServiceContext(Connection.OrganizationService);

                    CrmService.Dispose();
                    CrmService = new CrmService(OrganizationServiceContext, Connection);

                    WaitForImportJob(importJobId, systemJobId, pollingInterval / 1000, pollingTimeout / 1000, throwsException, waitedFor);
                    break;
                }
                catch (FaultException<OrganizationServiceFault> ex)
                {
                    if (ex.Detail != null && ex.Detail.ErrorCode == -2147020463)
                    {
                        Logger.Warn(CultureInfo.InvariantCulture, "Oups ... another solution is currently installing, we will retry in a minute.");
                        Logger.Warn(ex.Detail.Message);

                        Thread.Sleep(60000); WaitForImportJob(importJobId, systemJobId, pollingInterval, pollingTimeout, throwsException);
                    }
                    else
                    {
                        throw;
                    }
                }

                if (waitedFor > pollingTimeout)
                {
                    if (throwsException)
                    {
                        throw new TimeoutException(string.Format(CultureInfo.InvariantCulture, "The system job {0} timed out.", systemJobId));
                    }
                    inProgress = false;
                }

                Thread.Sleep(pollingInterval); waitedFor += pollingInterval;
            }

            Logger.Trace(CultureInfo.InvariantCulture, TraceMessageHelper.ExitingMethod, SystemTypeName, MethodBase.GetCurrentMethod().Name);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling")]
        private void WaitForSystemJob(Guid systemJobId, int pollingInterval, int pollingTimeout, bool throwsException, int waitedFor = 0)
        {
            Logger.Trace(CultureInfo.InvariantCulture, TraceMessageHelper.EnteredMethod, SystemTypeName, MethodBase.GetCurrentMethod().Name);

            if (Guid.Empty.Equals(systemJobId)) throw new ArgumentNullException(nameof(systemJobId));
            if (pollingInterval < 1) throw new PlatformException("Interval must be 1 second or more.");
            pollingInterval *= 1000; pollingTimeout *= 1000; bool inProgress = true;

            while (inProgress)
            {
                try
                {
                    OrganizationServiceContext.ClearChanges();

                    SystemJob systemJob = CrmService.GetSystemJob(systemJobId);

                    if (systemJob == null) throw new PlatformException("The requested job doesn't exist.");

                    switch (systemJob.StatusReason)
                    {
                        case SystemJobStatusReason.Succeeded:
                            inProgress = false;
                            break;

                        case SystemJobStatusReason.Canceling:
                        case SystemJobStatusReason.Canceled:
                        case SystemJobStatusReason.Failed:
                        case SystemJobStatusReason.Pausing:
                            if (throwsException)
                            {
                                throw new PlatformException(string.Format(CultureInfo.InvariantCulture, "The requested job failed. {0} {1}", systemJob.StatusReason, systemJob.Message));
                            }
                            inProgress = false;
                            break;
                    }
                }
                catch (MessageSecurityException ex)
                {
                    Logger.Warn(CultureInfo.InvariantCulture, "Oups ... a security exception occurred, we will try to reconnect.");
                    Logger.Warn(ex.Message);

                    Connection.SetOrganizationService();

                    OrganizationServiceContext.Dispose();
                    OrganizationServiceContext = new CrmServiceContext(Connection.OrganizationService);

                    CrmService.Dispose();
                    CrmService = new CrmService(OrganizationServiceContext, Connection);

                    WaitForSystemJob(systemJobId, pollingInterval / 1000, pollingTimeout / 1000, throwsException, waitedFor);
                    break;
                }
                catch (FaultException<OrganizationServiceFault> ex)
                {
                    if (ex.Detail != null && ex.Detail.ErrorCode == -2147020463)
                    {
                        Logger.Warn(CultureInfo.InvariantCulture, "Oups ... another solution is currently installing, we will retry in a minute.");
                        Logger.Warn(ex.Detail.Message);

                        Thread.Sleep(60000); WaitForSystemJob(systemJobId, pollingInterval, pollingTimeout, throwsException);
                    }
                    else
                    {
                        throw;
                    }
                }

                if (waitedFor > pollingTimeout)
                {
                    if (throwsException)
                    {
                        throw new TimeoutException(string.Format(CultureInfo.InvariantCulture, "The system job {0} timed out.", systemJobId));
                    }
                    inProgress = false;
                }

                Logger.Info(CultureInfo.InvariantCulture, "The system job is running for {0} seconds.", waitedFor/1000);
                Thread.Sleep(pollingInterval); waitedFor += pollingInterval;
            }

            Logger.Trace(CultureInfo.InvariantCulture, TraceMessageHelper.ExitingMethod, SystemTypeName, MethodBase.GetCurrentMethod().Name);
        }

        public void PublishCustomizations()
        {
            Logger.Trace(CultureInfo.InvariantCulture, TraceMessageHelper.EnteredMethod, SystemTypeName, MethodBase.GetCurrentMethod().Name);

            int errorCount = 0;

            PublishAllXmlResponse response = CrmService.PublishAllCustomizations();

            foreach (KeyValuePair<string, object> result in response.Results)
            {
                if (result.Value is Exception exception)
                {
                    errorCount++;
                    Logger.Error(exception);
                }
            }

            if (errorCount > 0) throw new PlatformException("Error(s) occurred whilst publishing the customizations.");

            Logger.Trace(CultureInfo.InvariantCulture, TraceMessageHelper.ExitingMethod, SystemTypeName, MethodBase.GetCurrentMethod().Name);
        }

        public void RemoveSolutionComponent(string solutionName, string componentId, int componentType)
        {
            Logger.Trace(CultureInfo.InvariantCulture, TraceMessageHelper.EnteredMethod, SystemTypeName, MethodBase.GetCurrentMethod().Name);

            if (string.IsNullOrWhiteSpace(solutionName)) throw new ArgumentNullException(nameof(solutionName));
            if (string.IsNullOrWhiteSpace(componentId)) throw new ArgumentNullException(nameof(componentId));

            if (!Guid.TryParse(componentId, out Guid componentGuid)) throw new ArgumentException("componentId is not a valid Guid");

            CrmService.RemoveSolutionComponent(solutionName, componentGuid, componentType);

            Logger.Trace(CultureInfo.InvariantCulture, TraceMessageHelper.ExitingMethod, SystemTypeName, MethodBase.GetCurrentMethod().Name);
        }

        public void UpdateSolutionVersion(string solutionName, string timeZone, string version)
        {
            Logger.Trace(CultureInfo.InvariantCulture, TraceMessageHelper.EnteredMethod, SystemTypeName, MethodBase.GetCurrentMethod().Name);

            if (string.IsNullOrWhiteSpace(solutionName)) throw new ArgumentNullException(nameof(solutionName));
            Logger.Info(CultureInfo.InvariantCulture, "Solution name: {0}.", solutionName);

            if (string.IsNullOrWhiteSpace(timeZone)) throw new ArgumentNullException(nameof(timeZone));
            Logger.Info(CultureInfo.InvariantCulture, "Time zone: {0}.", timeZone);

            if (solutionName.ToUpperInvariant().Equals("DEFAULT"))
            {
                Logger.Warn(CultureInfo.InvariantCulture, "Version update of the default solution is not allowed.");
                Logger.Trace(CultureInfo.InvariantCulture, TraceMessageHelper.EnteredMethod, SystemTypeName, MethodBase.GetCurrentMethod().Name);
                return;
            }

            IList<Solution> solutions = CrmService.GetSolutions(solutionName);

            OrganizationServiceContext.ClearChanges();

            foreach (Solution solution in solutions)
            {
                Logger.Info(CultureInfo.InvariantCulture, "Processing solution '{0} {1}'.", solution.DisplayName, solution.Version);

                if (solution.PackageType.GetValueOrDefault()) throw new PlatformException("Version update of managed solutions is not allowed.");

                Solution updatedSolution = new Solution
                {
                    SolutionId = solution.Id,
                    Version = string.IsNullOrWhiteSpace(version) ? CalculateVersion(timeZone) : version
                };

                OrganizationServiceContext.Attach(updatedSolution);
                OrganizationServiceContext.UpdateObject(updatedSolution);

                Logger.Info(CultureInfo.InvariantCulture, "Solution {0} version set to {1}.", solution.DisplayName, updatedSolution.Version);
            }

            if (OrganizationServiceContext.GetAttachedEntities().OfType<Solution>().Any())
            {
                CrmService.SaveChanges(OrganizationServiceContext, SaveChangesOptions.None);
            }

            Logger.Trace(CultureInfo.InvariantCulture, TraceMessageHelper.ExitingMethod, SystemTypeName, MethodBase.GetCurrentMethod().Name);
        }

        private string CalculateVersion(string timeZone)
        {
            Logger.Trace(CultureInfo.InvariantCulture, TraceMessageHelper.EnteredMethod, SystemTypeName, MethodBase.GetCurrentMethod().Name);

            if (string.IsNullOrWhiteSpace(timeZone)) throw new ArgumentNullException(nameof(timeZone));

            TimeZoneInfo timeZoneInfo;
            DateTime dateTime;

            try
            {
                timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(timeZone);
                dateTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZoneInfo);
            }
            catch
            {
                dateTime = DateTime.UtcNow;
            }

            string major = string.Format(CultureInfo.InvariantCulture, "{0}", dateTime.ToString("yy", CultureInfo.InvariantCulture));
            string minor = string.Format(CultureInfo.InvariantCulture, "{0}", dateTime.Month.ToString("00", CultureInfo.InvariantCulture));
            string revision = string.Format(CultureInfo.InvariantCulture, "{0}", dateTime.Day.ToString("00", CultureInfo.InvariantCulture));
            string build = string.Format(CultureInfo.InvariantCulture, "{0}{1}", dateTime.Hour.ToString("00", CultureInfo.InvariantCulture), dateTime.Minute.ToString("00", CultureInfo.InvariantCulture));

            Logger.Trace(CultureInfo.InvariantCulture, TraceMessageHelper.ExitingMethod, SystemTypeName, MethodBase.GetCurrentMethod().Name);

            return string.Format(CultureInfo.InvariantCulture, "{0}.{1}.{2}.{3}", major, minor, revision, build);
        }

        private void ExportSolution(string uniqueName, bool packageType, string absoluteFilePath)
        {
            Logger.Trace(CultureInfo.InvariantCulture, TraceMessageHelper.EnteredMethod, SystemTypeName, MethodBase.GetCurrentMethod().Name);

            if (string.IsNullOrWhiteSpace(uniqueName)) throw new ArgumentNullException(nameof(uniqueName));

            Logger.Info(CultureInfo.InvariantCulture, "Downloading {0} solution {1}", packageType ? "managed" : "unmanaged", uniqueName);

            ExportSolutionResponse res = CrmService.ExportSolution(uniqueName, packageType);

            Logger.Info(CultureInfo.InvariantCulture, "Saving {0}.", absoluteFilePath);

            File.WriteAllBytes(absoluteFilePath, res.ExportSolutionFile);

            Logger.Trace(CultureInfo.InvariantCulture, TraceMessageHelper.ExitingMethod, SystemTypeName, MethodBase.GetCurrentMethod().Name);
        }

        private static string GetNameText(ImportJobResult importJobResult)
        {
            return string.IsNullOrWhiteSpace(importJobResult.Name) ? " " : string.Format(CultureInfo.InvariantCulture, " {0}", importJobResult.Name);
        }

        private static string GetProcessedText(ImportJobResult importJobResult)
        {
            return string.IsNullOrWhiteSpace(importJobResult.Processed) ? " " : bool.Parse(importJobResult.Processed) ? " processed" : " not processed";
        }

        private static string GetResultText(ImportJobResult importJobResult)
        {
            return string.IsNullOrWhiteSpace(importJobResult.Result) ? "." :
                importJobResult.Result == ImportJobStatus.Success ? " successfully." :
                importJobResult.Result == ImportJobStatus.Warning ? string.Format(CultureInfo.InvariantCulture, " with WARNING {0}.", importJobResult.ErrorText) :
                importJobResult.Result == ImportJobStatus.Error ? string.Format(CultureInfo.InvariantCulture, " with ERROR {0}.", importJobResult.ErrorText) : ".";
        }

        private IEnumerable<ImportJobResult> ParseImportJobXmlData(string importJobData)
        {
            Logger.Trace(CultureInfo.InvariantCulture, TraceMessageHelper.EnteredMethod, SystemTypeName, MethodBase.GetCurrentMethod().Name);

            if (string.IsNullOrWhiteSpace(importJobData)) throw new ArgumentNullException(nameof(importJobData));
            
            XElement elements = XElement.Parse(importJobData);

            IEnumerable<ImportJobResult> importResults = elements.Descendants("result").ToList().Select(x => {
                XElement y = x.Ancestors().First();
                return new ImportJobResult
                {
                    ElementName = y.Name.ToString(),
                    Name =
                        y.Attribute(ImportJobComponent.LocalizedName) != null ? y.Attribute(ImportJobComponent.LocalizedName).Value :
                        y.Attribute(ImportJobComponent.Name) != null ? y.Attribute(ImportJobComponent.Name).Value :
                        y.Attribute(ImportJobComponent.Id) != null ? y.Attribute(ImportJobComponent.Id).Value :
                        string.Empty,
                    Processed = y.Attribute(ImportJobComponent.Processed)?.Value,
                    Result = x.Attribute(ImportJobComponent.Result)?.Value,
                    ErrorCode = x.Attribute(ImportJobComponent.ErrorCode)?.Value,
                    ErrorText = x.Attribute(ImportJobComponent.ErrorText)?.Value,
                    DateTimeTicks = new DateTime(Convert.ToInt64(x.Attribute(ImportJobComponent.DateTimeTicks)?.Value, CultureInfo.InvariantCulture), DateTimeKind.Utc).ToLocalTime().ToString(CultureInfo.InvariantCulture)
                };
            }).OrderBy(x => x.DateTimeTicks);

            Logger.Trace(CultureInfo.InvariantCulture, TraceMessageHelper.ExitingMethod, SystemTypeName, MethodBase.GetCurrentMethod().Name);

            return importResults;
        }

        void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (cts != null) cts.Dispose();
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