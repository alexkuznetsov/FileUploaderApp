using FileUploadApp.Domain;
using FileUploadApp.Features.Commands;
using FileUploadApp.Interfaces;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;

namespace FileUploadApp.Tests
{
    [TestClass]
    public class StorageTests : TestData
    {
        private IServiceProvider serviceProvider;

        [TestInitialize]
        public void Initialize()
        {
            serviceProvider = ContainerBuilder.Create();
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
        public async Task Test_Store_ShouldUploadWithoutError()
        {
            //TODO Implement storage tests
            var storage = serviceProvider.GetRequiredService<IStore<Guid, Upload, UploadResultRow>>();
            var result = await storage.StoreAsync(FakeUpload);

            Assert.IsTrue(result != null);
            Assert.AreEqual(result.ContentType, FakeUpload.ContentType);
            Assert.AreEqual(result.Id, FakeUpload.Id);
            Assert.AreEqual(result.Name, FakeUpload.Name);
            Assert.AreEqual(result.Number, FakeUpload.Number);
            Assert.IsTrue(result.IsImage());
        }

        [TestMethod]
        public async Task Test_HandlerAndStore_ShouldUploadImageAndMakePreview()
        {
            var uploadEvent = new UploadFiles.Event(new[] { FakeUpload });
            var mediator = serviceProvider.GetRequiredService<IMediator>();
            var appConfig = serviceProvider.GetRequiredService<AppConfiguration>();

            await mediator.Publish(uploadEvent);

            var storage = serviceProvider.GetRequiredService<IStore<Guid, Upload, UploadResultRow>>();

            var storedOrigin = await storage.ReceiveAsync(FakeUpload.Id);
            var storedPReview = await storage.ReceiveAsync(FakeUpload.PreviewId);

            Assert.IsTrue(storedOrigin != null);
            Assert.AreEqual(storedOrigin.ContentType, FakeUpload.ContentType);
            Assert.AreEqual(storedOrigin.Id, FakeUpload.Id);
            Assert.AreEqual(storedOrigin.Name, FakeUpload.Name);
            Assert.IsTrue(storedOrigin.IsImage());

            Assert.IsTrue(storedPReview != null);
            Assert.AreEqual(storedPReview.ContentType, appConfig.PreviewContentType);
            Assert.AreEqual(storedPReview.Id, FakeUpload.PreviewId);
            Assert.AreEqual(storedPReview.Name, Upload.PreviewPrefix + FakeUpload.Name);
            Assert.IsTrue(storedPReview.IsImage());
        }
    }
}
