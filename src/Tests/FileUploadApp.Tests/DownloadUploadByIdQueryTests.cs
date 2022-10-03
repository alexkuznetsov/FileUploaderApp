using FileUploadApp.Domain;
using FileUploadApp.Features.Queries;
using FileUploadApp.Storage;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;

namespace FileUploadApp.Tests
{
    [TestClass]
    public class DownloadUploadByIdQueryTests : TestData
    {
        public TestContext TestContext { get; set; }

        private IServiceProvider serviceProvider;

        [TestInitialize]
        public void Initialize()
        {
            serviceProvider = ContainerBuilder.Create((s) =>
            {
                #region Replace by mocked

                var fakeMetaStore = CreateFakeMetadataStore();
                var sd = new ServiceDescriptor(
                    typeof(IStoreBackend<Guid, Metadata, Metadata>)
                    , (_) => fakeMetaStore
                    , ServiceLifetime.Scoped);

                s.Replace(sd);

                var fakeUploadsStore = CreateFakeUploadStore();

                sd = new ServiceDescriptor(
                    typeof(IStoreBackend<Guid, Metadata, Upload>)
                    , (_) => fakeUploadsStore
                    , ServiceLifetime.Scoped);

                s.Replace(sd);

                var fakeFileStmAdapter = CreateFakeStreamAdapter();

                sd = new ServiceDescriptor(
                    typeof(IFileStreamProvider<Guid, StreamAdapter>)
                    , (_) => fakeFileStmAdapter
                    , ServiceLifetime.Scoped);

                s.Replace(sd);

                #endregion
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
            var req = new DownloadUploadById.Query(RequestId);

            using var scope = serviceProvider.CreateScope();

            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
            var response = await mediator.Send(req);

            Assert.IsNotNull(response);
            Assert.AreEqual(response.Id, RequestId);
            Assert.AreEqual(response.Name, FakeUpload.Name);
            Assert.AreEqual(response.ContentType, FakeUpload.ContentType);
        }
    }
}