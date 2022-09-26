using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using BenchmarkDotNet.Toolchains.CsProj;
using System;

namespace FileUploadApp.Tests.Bench
{
    class Program
    {
        static void Main(string[] args) => BenchmarkRunner.Run<Base64ParserBench>(
            DefaultConfig.Instance
                .AddJob(Job.Default.WithToolchain(CsProjCoreToolchain.NetCoreApp60)));
    }
}
