﻿using FileUploadApp.Domain;
using FileUploadApp.Domain.Raw;
using FileUploadApp.Features.Commands;
using FileUploadApp.Features.Services;
using FileUploadApp.Interfaces;
using FileUploadApp.Storage;
using FileUploadApp.StreamAdapters;
using MediatR;
using Moq;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace FileUploadApp.Tests
{
    public abstract class TestData
    {
        protected static Guid RequestId { get; } = Guid.Parse("22b07b19-3083-4e78-a3da-1af171def587");

        private static Guid RequestPreviewId { get; } = Guid.Parse("953a0102-39dd-4164-89cb-92d3867c0739");

        private static Stream ImageArray { get; } = new MemoryStream(Convert.FromBase64String(
            "Qk2mFQAAAAAAADYAAAAoAAAAJQAAADEAAAABABgAAAAAAHAVAAAAAAAAAAAAAAAAAAAAAAAA////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////AP///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////wD///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////8A////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////AP///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////wD///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////8A////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////AP///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////wD///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////8A////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////AP///////////////////////wAAAP///////////////////////////////////////////////////////////////////wAAAAAAAP///////////////////////////////////////////wD///////////////////////8AAAD///////////////////////////////////////////////////////////////8AAAD///////////////////////////////////////////////////8A////////////////////////AAAA////////////////////////////////////////////////////////////AAAAAAAA////////////////////////////////////////////////////AP///////////////////////wAAAP///////////////////////////////////////////////////////////wAAAP///////////////////////////////////////////////////////wD///////////////////////////8AAAD///////////////////////////////////////////////////////8AAAD///////////////////////////////////////////////////////8A////////////////////////////AAAA////////////////////////////////////////////////AAAAAAAA////////////////////////////////////////////////////////////AP///////////////////////////////wAAAP///////////////////////////////////////////wAAAP///////////////////////////////////////////////////////////////wD///////////////////////////////////8AAAD///////////////////////////////////////8AAAD///////////////////////////////////////////////////////////////8A////////////////////////////////////////AAAA////////////////////////////////AAAAAAAA////////////////////////////////////////////////////////////////AP///////////////////////////////////////////wAAAP///////////////////////////wAAAP///////////////////////////////////////////////////////////////////wD///////////////////////////////////////////8AAAD///////////////////////8AAAD///////////////////////////////////////////////////////////////////////8A////////////////////////////////////////////////AAAA////////////////AAAA////////////////////////////////////////////////////////////////////////////AP///////////////////////////////////////////////wAAAP///////////wAAAP///////////////////////////////////////////////////////////////////////////////wD///////////////////////////////////////////////////8AAAD///////8AAAD///////////////////////////////////////////////////////////////////////////////8A////////////////////////////////////////////////////AAAAAAAAAAAA////////////////////////////////////////////////////////////////////////////////////AP///////////////////////////////////////////////////wAAAP///////////////////////////////////////////////////////////////////////////////////////////wD///////////////////////////////////////////////////8AAAAAAAAAAAAAAAD///////////////////////////////////////////////////////////////////////////////8A////////////////////////////////////////////////////AAAA////////////AAAA////////////////////////////////////////////////////////////////////////////AP///////////////////////////////////////////////wAAAP///////////////wAAAP///////////////////////////////////////////////////////////////////////////wD///////////////////////////////////////////8AAAD///////////////////8AAAD///////////////////////////////////////////////////////////////////////////8A////////////////////////////////////////////AAAA////////////////////////AAAAAAAA////////////////////////////////////////////////////////////////////AP///////////////////////////////////////////wAAAP///////////////////////////////wAAAP///////////////////////////////////////////////////////////////wD///////////////////////////////////////8AAAD///////////////////////////////////8AAAD///////////////////////////////////////////////////////////////8A////////////////////////////////////////AAAA////////////////////////////////////////AAAA////////////////////////////////////////////////////////////AP///////////////////////////////////wAAAP///////////////////////////////////////////wAAAAAAAP///////////////////////////////////////////////////////wD///////////////////////////////8AAAAAAAD///////////////////////////////////////////////8AAAD///////////////////////////////////////////////////////8A////////////////////////////AAAA////////////////////////////////////////////////////////AAAAAAAA////////////////////////////////////////////////////AP///////////////////////////wAAAP///////////////////////////////////////////////////////////wAAAP///////////////////////////////////////////////////wD///////////////////////////8AAAD///////////////////////////////////////////////////////////////////////////////////////////////////////////////////8A////////////////////////AAAA////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////AP///////////////////////wAAAP///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////wD///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////8A////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////AP///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////wD///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////8A////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////AP///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////wD///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////8A////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////AA=="));

        protected static Uri RequestUri { get; } = new Uri("http://localhot/1.bmp");

        private static DateTime FakeMetadataDate { get; } = DateTime.Parse("2018-05-31 10:00");

        private static DownloadUriResponse FakeDownloadUriResponse { get; } =
            new DownloadUriResponse(RequestUri, MimeConstants.BitmapMime
                , DefaultStreamAdapter);

        private static StreamAdapter DefaultStreamAdapter { get; } 
            = new CommonStreamStreamAdapter(ImageArray);

        protected static Upload FakeUpload { get; } = new Upload(
              RequestId
            , RequestPreviewId
            , 0U
            , Path.GetFileName(FakeDownloadUriResponse.Uri.LocalPath)
            , FakeDownloadUriResponse.ContentType
            , DefaultStreamAdapter);

        private static Metadata DefaultMetadata { get; } = new Metadata(RequestId
            , Path.GetFileName(FakeDownloadUriResponse.Uri.LocalPath)
            , MimeConstants.BitmapMime
            , FakeMetadataDate);


        #region Mock Storage

        protected static IFileStreamProvider<Guid, StreamAdapter> CreateFakeStreamAdapter()
        {
            var mock = new Mock<IFileStreamProvider<Guid, StreamAdapter>>();
            mock.Setup(x => x.GetStreamAdapter(It.Is<Guid>(g => g.Equals(RequestId))))
                .Returns(DefaultStreamAdapter);

            return mock.Object;
        }

        protected static IStoreBackend<Guid, Metadata, Upload> CreateFakeUploadStore()
        {
            var mock = new Mock<IStoreBackend<Guid, Metadata, Upload>>();
            mock.Setup(x => x.FindAsync(It.Is<Guid>(y => y == RequestId), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(FakeUpload));

            return mock.Object;
        }

        protected static IStoreBackend<Guid, Metadata, Metadata> CreateFakeMetadataStore()
        {
            var mock = new Mock<IStoreBackend<Guid, Metadata, Metadata>>();

            mock.Setup(x => x.FindAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(DefaultMetadata));

            return mock.Object;
        }

        #endregion

        #region Mock download

        protected static IRequestHandler<DownloadUri.Command, Upload> CreateFakeRequestHandlerForDownloadUriQuery()
        {
            var mock = new Mock<IRequestHandler<DownloadUri.Command, Upload>>();
            mock.Setup(x => x.Handle(It.IsAny<DownloadUri.Command>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(FakeUpload));

            return mock.Object;
        }

        protected static IContentDownloaderFactory<DownloadUriResponse> CreateFakeContentDownloaderFactory()
        {
            var fakeDownloader = CreateFakeContentDownloader();
            var mock = new Mock<IContentDownloaderFactory<DownloadUriResponse>>();

            mock.Setup(x => x.Create(It.IsAny<Uri>()))
                .Returns(fakeDownloader);

            return mock.Object;
        }

        private static IContentDownloader<DownloadUriResponse> CreateFakeContentDownloader()
        {
            var mock = new Mock<IContentDownloader<DownloadUriResponse>>();

            mock.Setup(x => x.DownloadAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(FakeDownloadUriResponse));

            return mock.Object;
        }

        #endregion
    }
}