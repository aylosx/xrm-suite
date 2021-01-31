namespace Aylos.Xrm.Sdk.BuildTools.Services
{
    using System;

    public interface ICompressionService : IDisposable
    {
        void Compress(string inputPath, string outputFile);

        void Decompress(string inputFile, string outputPath);
    }
}