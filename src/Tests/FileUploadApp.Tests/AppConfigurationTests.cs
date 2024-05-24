using System;

using FileUploadApp.Domain;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FileUploadApp.Tests;

[TestClass]
public class AppConfigurationTests
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
    public void Test_AppConfiguration_ShouldBeNotNull()
    {
        var appConf = _serviceProvider.GetRequiredService<AppConfiguration>();

        Assert.IsNotNull(appConf);
    }

    [TestMethod]
    public void Test_AppConfiguration_DefaultUA_ShouldBeNotNull()
    {
        var appConf = _serviceProvider.GetRequiredService<AppConfiguration>();

        Assert.IsFalse(string.IsNullOrWhiteSpace(appConf.DefaultUserAgent));
        Assert.IsNotNull(appConf.PreviewSize);
        Assert.IsTrue(appConf.PreviewSize.Width == 100);
        Assert.IsTrue(appConf.PreviewSize.Height == 100);
    }


    [TestMethod]
    public void Test_AppConfiguration_PreviewSize_ShouldBeNotNull()
    {
        var appConf = _serviceProvider.GetRequiredService<AppConfiguration>();

        Assert.IsTrue(appConf.PreviewSize != default);
        Assert.IsTrue(appConf.PreviewSize.Width == 100);
        Assert.IsTrue(appConf.PreviewSize.Height == 100);
    }

    [TestMethod]
    public void Test_AppConfiguration_AllowedContentTypes_ShouldBeNotNullOrEmpty()
    {
        var appConf = _serviceProvider.GetRequiredService<AppConfiguration>();

        Assert.IsNotNull(appConf.AllowedContentTypes);
        Assert.IsTrue(appConf.AllowedContentTypes.Length > 0);
    }

    [TestMethod]
    public void Test_AppConfiguration_AllowedContentTypes_ShouldContainsDefaultMimes()
    {
        var appConf = _serviceProvider.GetRequiredService<AppConfiguration>();

        Assert.IsTrue(Array.IndexOf(appConf.AllowedContentTypes, MimeConstants.PngMime) >= 0);
        Assert.IsTrue(Array.IndexOf(appConf.AllowedContentTypes, MimeConstants.BitmapMime) >= 0);
        Assert.IsTrue(Array.IndexOf(appConf.AllowedContentTypes, MimeConstants.GifMime) >= 0);
        Assert.IsTrue(Array.IndexOf(appConf.AllowedContentTypes, MimeConstants.JpgMime) >= 0);
        Assert.IsTrue(Array.IndexOf(appConf.AllowedContentTypes, MimeConstants.TiffMime) >= 0);
    }
}
