using System;
using System.Threading.Tasks;

using FileUploadApp.Application.Uploading.Commands;
using FileUploadApp.Domain;
using FileUploadApp.Interfaces;

using MediatR;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FileUploadApp.Tests;

[TestClass]
public class StorageTests : TestData
{
    private IServiceProvider _serviceProvider = null!;

    [TestInitialize]
    public void Initialize()
    {
        _serviceProvider = ContainerBuilder.Create();
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
    public async Task Test_Store_ShouldUploadWithoutError()
    {
        //TODO Implement storage tests
        var storage = _serviceProvider.GetRequiredService<IStore<Guid, Upload, UploadResultRow>>();
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
        //TODO Refactor 
        var uploadEvent = new UploadFiles.Command([FakeUpload]);
        var mediator = _serviceProvider.GetRequiredService<IMediator>();
        var appConfig = _serviceProvider.GetRequiredService<AppConfiguration>();

        await mediator.Publish(uploadEvent);

        var storage = _serviceProvider.GetRequiredService<IStore<Guid, Upload, UploadResultRow>>();

        var storedOrigin = await storage.ReceiveAsync(FakeUpload.Id);
        var storedPReview = await storage.ReceiveAsync(FakeUpload.PreviewId!.Value!);

        Assert.IsTrue(storedOrigin != null);
        Assert.AreEqual(storedOrigin.ContentType, FakeUpload.ContentType);
        Assert.AreEqual(storedOrigin.Id, FakeUpload.Id);
        Assert.AreEqual(storedOrigin.Name, FakeUpload.Name);
        Assert.IsTrue(storedOrigin.IsImage());

        Assert.IsTrue(storedPReview != null);
        Assert.AreEqual(storedPReview.ContentType, FakeUpload.ContentType);
        //Assert.AreEqual(storedPReview.Id, FakeUpload.PreviewId);
        //Assert.AreEqual(storedPReview.Name, Upload.PreviewPrefix + FakeUpload.Name);
        Assert.IsTrue(storedPReview.IsImage());
    }
}
