using FileUploadApp.Domain;
using FileUploadApp.Domain.Dirty;
using FileUploadApp.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace FileUploadApp.Tests
{
    [TestClass]
    public class StorageTests
    {
        public TestContext TestContext { get; set; }

        private IServiceProvider serviceProvider;

        [TestInitialize]
        public void Initialize()
        {
            var builder = new ContainerBuilder();
            serviceProvider = builder.Create();
        }

        [TestCleanup]
        public void Cleanup()
        {
            if (serviceProvider is IDisposable d)
            {
                d.Dispose();
            }
        }

        [TestMethod]
        public void Test_UploadRequest_ShouldBeDeserilizedWithoutErrors_1()
        {
            //TODO Implement storage tests
        }
    }
}
