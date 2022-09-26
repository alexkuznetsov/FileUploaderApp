using FileUploadApp.Authentication;
using FileUploadApp.Authentication.Queries;
using FileUploadApp.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;

namespace FileUploadApp.Tests
{
    [TestClass]
    public class CheckUserServiceTests
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
        public async Task Test_CheckServiceShouldReturnCorrectUser()
        {
            var svc = serviceProvider.GetRequiredService<ICheckUserService<User>>();
            var user = await svc.FindByNameAsync("rex");

            Assert.IsNotNull(user);
            Assert.AreEqual(user.Username, "rex");
            Assert.IsTrue(user.Id > 0);
            Assert.IsTrue(user.CreatedAt > DateTime.MinValue);
            Assert.IsTrue(user.UpdatedAt == null);
            Assert.IsFalse(string.IsNullOrEmpty(user.Passwhash));
        }

        [TestMethod]
        public async Task Test_CheckServiceShouldReturnNullOnNonExistsUser()
        {
            var svc = serviceProvider.GetRequiredService<ICheckUserService<User>>();
            var user = await svc.FindByNameAsync("hex");

            Assert.IsNull(user);
        }

        [TestMethod]
        public async Task Test_CheckServiceShouldAuthenticateCorrectUser()
        {
            var svc = serviceProvider.GetRequiredService<ICheckUserService<User>>();
            var user = await svc.FindByNameAsync("rex");
            var status = svc.Authenticate(user, "1qaz!QAZ");

            Assert.IsNotNull(user);
            Assert.AreEqual(user.Username, "rex");
            Assert.IsTrue(status);
        }

        [TestMethod]
        public async Task Test_CheckHandlerShouldAuthenticateCorrectUser()
        {
            var mediator = serviceProvider.GetRequiredService<IMediator>();
            var result = await mediator.Send(new CheckUser.Query("rex", "1qaz!QAZ"));

            Assert.IsNotNull(result);
            Assert.AreEqual(result.User.Username, "rex");
        }

        [TestMethod]
        public async Task Test_CheckServiceShouldNotAuthenticateCorrectUserWithWrongPassword()
        {
            var svc = serviceProvider.GetRequiredService<ICheckUserService<User>>();
            var user = await svc.FindByNameAsync("rex");
            var status = svc.Authenticate(user, "1qazQAZ");

            Assert.IsNotNull(user);
            Assert.AreEqual(user.Username, "rex");
            Assert.IsFalse(status);
        }
    }
}
