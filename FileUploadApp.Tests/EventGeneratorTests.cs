using FileUploadApp.Core;
using MediatR;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace FileUploadApp.Tests
{
    [TestClass]
    public class EventGeneratorTests
    {
        public TestContext TestContext { get; set; }

        private IServiceProvider serviceProvider;

        [TestInitialize]
        public void Initialize()
        {
            var builder = new ContainerBuilder();
            serviceProvider = builder.Create((s) =>
            {
               
            });
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
        public void Test_Method1()
        {

        }
    }
}
