using FileUploadApp.Domain;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileUploadApp.Authentication.Services;

public class InMemoryCheckUserServiceOptions
{
    public const string SectionKey = "AuthServer:InMemoryUsers";

    public List<User> Users { get; set; }
        = new List<User>();

    public InMemoryCheckUserServiceOptions WithUser(string username, string password)
    {
        Users.Add(new() { Username = username, Passwhash = password });
        return this;
    }
}

public sealed class InMemoryCheckUserService : ICheckUserService<User>
{
    private readonly InMemoryCheckUserServiceOptions options;

    public InMemoryCheckUserService(IOptions<InMemoryCheckUserServiceOptions> options)
    {
        try
        {
            this.options = options.Value;
        }
        catch (Exception ex)
        {
            this.options = new InMemoryCheckUserServiceOptions();
        }
    }

    public bool Authenticate(User user, string password)
        => user.Passwhash == password;

    public async Task<bool> AuthenticateAsync(string username, string password)
    {
        var user = await FindByNameAsync(username).ConfigureAwait(false);

        return Authenticate(user, password);
    }

    public Task<User> FindByNameAsync(string username)
    {
        var user = options.Users.OrderBy(x => x.Username)
            .Select((r, i) => new User
            {
                CreatedAt = DateTime.Now,
                Id = i,
                Passwhash = r.Passwhash,
                UpdatedAt = null,
                Username = r.Username
            }).Where(x => x.Username.Equals(username, StringComparison.OrdinalIgnoreCase))
            .FirstOrDefault();

        return Task.FromResult(user);
    }
}
