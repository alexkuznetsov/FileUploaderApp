using FileUploadApp.Domain;
using FileUploadApp.Domain.Dirty;
using FileUploadApp.Interfaces;
using FileUploadApp.Requests;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;

namespace FileUploadApp.Tests
{
    [TestClass]
    public class DownloadUriQueryTests : TestData
    {
        private IServiceProvider serviceProvider;

        [TestInitialize]
        public void Initialize()
        {
            var builder = new ContainerBuilder();

            serviceProvider = builder.Create((s) =>
            {
                var fakeDownldFactory = CreateFakeContentDownloaderFactory();
                var sd = new ServiceDescriptor(
                      typeof(IContentDownloaderFactory<DownloadUriResponse>)
                    , (_) => fakeDownldFactory
                    , ServiceLifetime.Scoped);

                s.Replace(sd);

                var fakeHandler = CreateFakeRequestHandlerForDownloadUriQuery();
                sd = new ServiceDescriptor(
                      typeof(IRequestHandler<DownloadUriQuery, Upload>)
                    , (_) => CreateFakeRequestHandlerForDownloadUriQuery()
                    , ServiceLifetime.Scoped);

                s.Replace(sd);
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
        public async Task Test_QueryShouldReturnValidEntity()
        {
            var req = new DownloadUriQuery(0U, RequestUri);

            using (var scope = serviceProvider.CreateScope())
            {
                var mediator = serviceProvider.GetRequiredService<IMediator>();
                var response = await mediator.Send(req);

                Assert.IsNotNull(response);
                Assert.AreEqual(response.ContentType, FakeUpload.ContentType);
                Assert.AreEqual(response.Id, FakeUpload.Id);
                Assert.AreEqual(response.Name, FakeUpload.Name);
                Assert.AreEqual(response.Number, FakeUpload.Number);
                Assert.AreEqual(response.PreviewId, FakeUpload.PreviewId);
            }
        }
    }
}
