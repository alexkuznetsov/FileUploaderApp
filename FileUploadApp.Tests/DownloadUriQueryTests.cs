﻿using FileUploadApp.Domain.Dirty;
using FileUploadApp.Interfaces;
using FileUploadApp.Requests;
using FileUploadApp.Services;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FileUploadApp.Tests
{
    [TestClass]
    public class DownloadUriQueryTests
    {
        public TestContext TestContext { get; set; }

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
            });
        }

        private IContentDownloaderFactory<DownloadUriResponse> CreateFakeContentDownloaderFactory()
        {
            var fakeDownloader = CreateFakeContentDownloader();
            var mock = new Mock<IContentDownloaderFactory<DownloadUriResponse>>();

            mock.Setup(x => x.Create(It.IsAny<Uri>()))
                .Returns(fakeDownloader);

            return mock.Object;
        }

        private IContentDownloader<DownloadUriResponse> CreateFakeContentDownloader()
        {
            var mock = new Mock<IContentDownloader<DownloadUriResponse>>();

            mock.Setup(x => x.DownloadAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(FakeDownloadUriResponse));

            return mock.Object;
        }

        private DownloadUriResponse FakeDownloadUriResponse
            => new DownloadUriResponse(RequestUri, MimeConstants.BitmapMime, ImageArray);

        [TestCleanup]
        public void Cleanup()
        {
            if (serviceProvider is IDisposable d)
            {
                d.Dispose();
            }
        }

        private byte[] ImageArray { get; }
            = Convert.FromBase64String(
                       "Qk2mFQAAAAAAADYAAAAoAAAAJQAAADEAAAABABgAAAAAAHAVAAAAAAAAAAAAAAAAAAAAAAAA////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////AP///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////wD///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////8A////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////AP///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////wD///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////8A////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////AP///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////wD///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////8A////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////AP///////////////////////wAAAP///////////////////////////////////////////////////////////////////wAAAAAAAP///////////////////////////////////////////wD///////////////////////8AAAD///////////////////////////////////////////////////////////////8AAAD///////////////////////////////////////////////////8A////////////////////////AAAA////////////////////////////////////////////////////////////AAAAAAAA////////////////////////////////////////////////////AP///////////////////////wAAAP///////////////////////////////////////////////////////////wAAAP///////////////////////////////////////////////////////wD///////////////////////////8AAAD///////////////////////////////////////////////////////8AAAD///////////////////////////////////////////////////////8A////////////////////////////AAAA////////////////////////////////////////////////AAAAAAAA////////////////////////////////////////////////////////////AP///////////////////////////////wAAAP///////////////////////////////////////////wAAAP///////////////////////////////////////////////////////////////wD///////////////////////////////////8AAAD///////////////////////////////////////8AAAD///////////////////////////////////////////////////////////////8A////////////////////////////////////////AAAA////////////////////////////////AAAAAAAA////////////////////////////////////////////////////////////////AP///////////////////////////////////////////wAAAP///////////////////////////wAAAP///////////////////////////////////////////////////////////////////wD///////////////////////////////////////////8AAAD///////////////////////8AAAD///////////////////////////////////////////////////////////////////////8A////////////////////////////////////////////////AAAA////////////////AAAA////////////////////////////////////////////////////////////////////////////AP///////////////////////////////////////////////wAAAP///////////wAAAP///////////////////////////////////////////////////////////////////////////////wD///////////////////////////////////////////////////8AAAD///////8AAAD///////////////////////////////////////////////////////////////////////////////8A////////////////////////////////////////////////////AAAAAAAAAAAA////////////////////////////////////////////////////////////////////////////////////AP///////////////////////////////////////////////////wAAAP///////////////////////////////////////////////////////////////////////////////////////////wD///////////////////////////////////////////////////8AAAAAAAAAAAAAAAD///////////////////////////////////////////////////////////////////////////////8A////////////////////////////////////////////////////AAAA////////////AAAA////////////////////////////////////////////////////////////////////////////AP///////////////////////////////////////////////wAAAP///////////////wAAAP///////////////////////////////////////////////////////////////////////////wD///////////////////////////////////////////8AAAD///////////////////8AAAD///////////////////////////////////////////////////////////////////////////8A////////////////////////////////////////////AAAA////////////////////////AAAAAAAA////////////////////////////////////////////////////////////////////AP///////////////////////////////////////////wAAAP///////////////////////////////wAAAP///////////////////////////////////////////////////////////////wD///////////////////////////////////////8AAAD///////////////////////////////////8AAAD///////////////////////////////////////////////////////////////8A////////////////////////////////////////AAAA////////////////////////////////////////AAAA////////////////////////////////////////////////////////////AP///////////////////////////////////wAAAP///////////////////////////////////////////wAAAAAAAP///////////////////////////////////////////////////////wD///////////////////////////////8AAAAAAAD///////////////////////////////////////////////8AAAD///////////////////////////////////////////////////////8A////////////////////////////AAAA////////////////////////////////////////////////////////AAAAAAAA////////////////////////////////////////////////////AP///////////////////////////wAAAP///////////////////////////////////////////////////////////wAAAP///////////////////////////////////////////////////wD///////////////////////////8AAAD///////////////////////////////////////////////////////////////////////////////////////////////////////////////////8A////////////////////////AAAA////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////AP///////////////////////wAAAP///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////wD///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////8A////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////AP///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////wD///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////8A////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////AP///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////wD///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////8A////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////AA==");

        private Uri RequestUri => new Uri("http://localhot/1.bmp");

        [TestMethod]
        public async Task Test_QueryShouldReturnValidEntity()
        {
            var req = new DownloadUriQuery(RequestUri);

            using (var scope = serviceProvider.CreateScope())
            {
                var mediator = serviceProvider.GetRequiredService<IMediator>();
                var response = await mediator.Send(req);

                Assert.IsNotNull(response);
                Assert.AreEqual(response.Uri, RequestUri);
                Assert.AreEqual(response.ContentType, MimeConstants.BitmapMime);
                Assert.IsTrue(ImageArray.AsSpan().SequenceEqual(response.Bytea.Span));
            }
        }
    }
}
