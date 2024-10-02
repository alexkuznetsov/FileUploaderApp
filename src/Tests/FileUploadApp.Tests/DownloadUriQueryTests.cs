using System;
using System.Threading.Tasks;

using FileUploadApp.Application.Uploading.Commands;
using FileUploadApp.Domain;
using FileUploadApp.Domain.Raw;
using FileUploadApp.Interfaces;

using MediatR;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FileUploadApp.Tests;

[TestClass]
public class DownloadUriQueryTests : TestData
{
    private IServiceProvider _serviceProvider = null!;

    [TestInitialize]
    public void Initialize()
    {
        _serviceProvider = ContainerBuilder.Create((s) =>
        {
            var fakeContentDownloader = CreateFakeContentDownloader();
            var sd = new ServiceDescriptor(
                  typeof(IContentDownloader<DownloadUriResponse>)
                , (_) => fakeContentDownloader
                , ServiceLifetime.Scoped);

            s.Replace(sd);

            var fakeHandler = CreateFakeRequestHandlerForDownloadUriQuery();
            sd = new ServiceDescriptor(
                  typeof(IRequestHandler<DownloadUri.Command, DownloadUri.Result>)
                , (_) => fakeHandler
                , ServiceLifetime.Scoped);

            s.Replace(sd);
        });
    }

    [TestCleanup]
    public void Cleanup()
    {
        if (_serviceProvider is IDisposable d)
        {
            d.Dispose();
        }
    }

    [TestMethod]
    public async Task Test_QueryShouldReturnValidEntity()
    {
        var req = new DownloadUri.Command(0U, RequestUri);

        using var scope = _serviceProvider.CreateScope();

        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
        var result = await mediator.Send(req);
        var response = result.Result;

        Assert.IsNotNull(response);
        Assert.AreEqual(response.ContentType, FakeUpload.ContentType);
        Assert.AreEqual(response.Id, FakeUpload.Id);
        Assert.AreEqual(response.Name, FakeUpload.Name);
        Assert.AreEqual(response.Number, FakeUpload.Number);
        Assert.AreEqual(response.PreviewId, FakeUpload.PreviewId);
    }
}
