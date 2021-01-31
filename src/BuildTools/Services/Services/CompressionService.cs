namespace Aylos.Xrm.Sdk.BuildTools.Services
{
    using Aylos.Xrm.Sdk.Common;
    using Aylos.Xrm.Sdk.ConsoleApps;
    using System;
    using System.Globalization;
    using System.IO;
    using System.IO.Compression;
    using System.Reflection;

    public sealed class CompressionService : ConsoleApp, ICompressionService
    {
        bool disposedValue;

        string SystemTypeName { get { return GetType().UnderlyingSystemType.Name; } }

        public CompressionService() : base()
        {
            Logger.Trace(CultureInfo.InvariantCulture, TraceMessageHelper.EnteredMethod, SystemTypeName, MethodBase.GetCurrentMethod().Name);

            Logger.Trace(CultureInfo.InvariantCulture, TraceMessageHelper.ExitingMethod, SystemTypeName, MethodBase.GetCurrentMethod().Name);
        }

        public void Compress(string inputPath, string outputFile)
        {
            Logger.Trace(CultureInfo.InvariantCulture, TraceMessageHelper.EnteredMethod, SystemTypeName, MethodBase.GetCurrentMethod().Name);

            if (string.IsNullOrWhiteSpace(inputPath)) throw new ArgumentNullException(nameof(inputPath));
            Logger.Info(CultureInfo.InvariantCulture, "Input path: {0}.", inputPath);

            if (!Directory.Exists(Path.GetDirectoryName(inputPath))) throw new ArgumentException("Path to extract the files does not exist.");

            if (string.IsNullOrWhiteSpace(outputFile)) throw new ArgumentNullException(nameof(outputFile));
            Logger.Info(CultureInfo.InvariantCulture, "Output file: {0}.", outputFile);

            if (!File.Exists(outputFile)) throw new FileNotFoundException("Output file does not exist.");

            Logger.Info("Compressing directory contents started.");

            if (File.Exists(outputFile))
            {
                Logger.Warn("File exists and will be replaced.");
                File.Delete(outputFile);
            }

            Logger.Warn(CultureInfo.InvariantCulture, "Compressing contents of {0} folder in {1}.", inputPath, outputFile);
            ZipFile.CreateFromDirectory(inputPath, outputFile, CompressionLevel.Optimal, false);

            Logger.Info("Directory contents compressed successfully.");

            Logger.Trace(CultureInfo.InvariantCulture, TraceMessageHelper.ExitingMethod, SystemTypeName, MethodBase.GetCurrentMethod().Name);
        }

        public void Decompress(string inputFile, string outputPath)
        {
            Logger.Trace(CultureInfo.InvariantCulture, TraceMessageHelper.EnteredMethod, SystemTypeName, MethodBase.GetCurrentMethod().Name);

            if (string.IsNullOrWhiteSpace(inputFile)) throw new ArgumentNullException(nameof(inputFile));
            Logger.Info(CultureInfo.InvariantCulture, "Input file: {0}.", inputFile);

            if (!File.Exists(inputFile)) throw new FileNotFoundException("Input file does not exist.");

            if (string.IsNullOrWhiteSpace(outputPath)) throw new ArgumentNullException(nameof(outputPath));
            Logger.Info(CultureInfo.InvariantCulture, "Output path: {0}.", outputPath);

            if (!Directory.Exists(Path.GetDirectoryName(outputPath))) throw new ArgumentException("Path to extract the files does not exist.");

            Logger.Info("Decompressing file started");

            using (ZipArchive archive = ZipFile.OpenRead(inputFile))
            {
                foreach (ZipArchiveEntry entry in archive.Entries)
                {
                    string entryFullName = Path.Combine(outputPath, entry.FullName);

                    string entryPath = Path.GetDirectoryName(entryFullName);

                    if (!Directory.Exists(entryPath))
                    {
                        Logger.Info(CultureInfo.InvariantCulture, "Creating directory: {0}", entryPath);
                        Directory.CreateDirectory(entryPath);
                    }

                    string entryFileName = Path.GetFileName(entryFullName);

                    if (!string.IsNullOrWhiteSpace(entryFileName))
                    {
                        Logger.Info(CultureInfo.InvariantCulture, "Extracting: {0}", entryFullName);
                        entry.ExtractToFile(entryFullName, true);
                    }
                }
            }

            Logger.Info("File decompressed successfully.");

            Logger.Trace(CultureInfo.InvariantCulture, TraceMessageHelper.ExitingMethod, SystemTypeName, MethodBase.GetCurrentMethod().Name);
        }

        void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    
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