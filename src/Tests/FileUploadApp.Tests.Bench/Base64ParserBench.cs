using BenchmarkDotNet.Attributes;
using FileUploadApp.Core.Encoding;
using System;

namespace FileUploadApp.Tests.Bench
{
    [MemoryDiagnoser]
    public class Base64ParserBench
    {
        private string StringData { get; set; }

        private static string DefaultFileName => "file.bmp";

        [GlobalSetup]
        public void Setup()
        {
            StringData = DataAccessor.ImageBase64();
        }

        [Benchmark]
        [Obsolete("Only for comparing results")]
        public void ParseAsString() => Base64Parser.Parse(StringData, DefaultFileName);

        [Benchmark]
        public void ParseAsSpan() => Base64Parser.Parse(StringData.AsSpan(), DefaultFileName);
    }
}
