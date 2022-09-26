using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Extensions.DependencyInjection;
using FileUploadApp.Domain;
using FileUploadApp.Features.Services;

namespace FileUploadApp.Tests
{
    [TestClass]
    public class AppConfigurationTests
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
        public void Test_AppConfiguration_ShouldBeNotNull()
        {
            var appConf = serviceProvider.GetRequiredService<AppConfiguration>();

            Assert.IsNotNull(appConf);
        }

        [TestMethod]
        public void Test_AppConfiguration_DefaultUA_ShouldBeNotNull()
        {
            var appConf = serviceProvider.GetRequiredService<AppConfiguration>();

            Assert.IsFalse(string.IsNullOrWhiteSpace(appConf.DefaultUserAgent));
            Assert.IsNotNull(appConf.PreviewSize);
            Assert.IsTrue(appConf.PreviewSize.Width == 100);
            Assert.IsTrue(appConf.PreviewSize.Height == 100);
        }


        [TestMethod]
        public void Test_AppConfiguration_PreviewSize_ShouldBeNotNull()
        {
            var appConf = serviceProvider.GetRequiredService<AppConfiguration>();

            Assert.IsTrue(appConf.PreviewSize != default);
            Assert.IsTrue(appConf.PreviewSize.Width == 100);
            Assert.IsTrue(appConf.PreviewSize.Height == 100);
        }

        [TestMethod]
        public void Test_AppConfiguration_AllowedContentTypes_ShouldBeNotNullOrEmpty()
        {
            var appConf = serviceProvider.GetRequiredService<AppConfiguration>();

            Assert.IsNotNull(appConf.AllowedContentTypes);
            Assert.IsTrue(appConf.AllowedContentTypes.Length > 0);
        }

        [TestMethod]
        public void Test_AppConfiguration_AllowedContentTypes_ShouldContainsDefaultMimes()
        {
            var appConf = serviceProvider.GetRequiredService<AppConfiguration>();

            Assert.IsTrue(Array.IndexOf(appConf.AllowedContentTypes, MimeConstants.PngMime) >= 0);
            Assert.IsTrue(Array.IndexOf(appConf.AllowedContentTypes, MimeConstants.BitmapMime) >= 0);
            Assert.IsTrue(Array.IndexOf(appConf.AllowedContentTypes, MimeConstants.GifMime) >= 0);
            Assert.IsTrue(Array.IndexOf(appConf.AllowedContentTypes, MimeConstants.JpgMime) >= 0);
            Assert.IsTrue(Array.IndexOf(appConf.AllowedContentTypes, MimeConstants.TiffMime) >= 0);
        }
    }
}
