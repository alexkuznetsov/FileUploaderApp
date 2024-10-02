using System;
using System.Threading.Tasks;

using FileUploadApp.Application.Authentication.Queries;
using FileUploadApp.Domain;
using FileUploadApp.Interfaces;

using MediatR;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FileUploadApp.Tests;

[TestClass]
public class CheckUserServiceTests
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
    public async Task Test_CheckServiceShouldReturnCorrectUser()
    {
        var svc = _serviceProvider.GetRequiredService<ICheckUserService<User>>();
        var user = await svc.FindByNameAsync("admin");

        Assert.IsNotNull(user);
        Assert.AreEqual(user.Username, "admin");
        Assert.IsTrue(user.Id > 0);
        Assert.IsTrue(user.CreatedAt > DateTime.MinValue);
        Assert.IsTrue(user.UpdatedAt == null);
        Assert.IsFalse(string.IsNullOrEmpty(user.Passwhash));
    }

    [TestMethod]
    public async Task Test_CheckServiceShouldReturnNullOnNonExistsUser()
    {
        var svc = _serviceProvider.GetRequiredService<ICheckUserService<User>>();
        var user = await svc.FindByNameAsync("hex");

        Assert.IsNull(user);
    }

    [TestMethod]
    public async Task Test_CheckServiceShouldAuthenticateCorrectUser()
    {
        var svc = _serviceProvider.GetRequiredService<ICheckUserService<User>>();
        var user = await svc.FindByNameAsync("admin");
        var status = svc.Authenticate(user, "1qaz!QAZ");

        Assert.IsNotNull(user);
        Assert.AreEqual(user.Username, "admin");
        Assert.IsTrue(status);
    }

    [TestMethod]
    public async Task Test_CheckHandlerShouldAuthenticateCorrectUser()
    {
        var mediator = _serviceProvider.GetRequiredService<IMediator>();
        var result = await mediator.Send(new CheckUser.Query("admin", "1qaz!QAZ"));

        Assert.IsNotNull(result);
        Assert.AreEqual(result.Result.Username, "admin");
    }

    [TestMethod]
    public async Task Test_CheckServiceShouldNotAuthenticateCorrectUserWithWrongPassword()
    {
        var svc = _serviceProvider.GetRequiredService<ICheckUserService<User>>();
        var user = await svc.FindByNameAsync("admin");
        var status = svc.Authenticate(user, "1qazQAZ");

        Assert.IsNotNull(user);
        Assert.AreEqual(user.Username, "admin");
        Assert.IsFalse(status);
    }
}
