using FileUploadApp.Domain;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
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
        Users.Add(new()
        {
            Username = username,
            Passwhash = password
        });
        return this;
    }
}

public sealed class InMemoryCheckUserService : ICheckUserService<User>
{
    private readonly InMemoryCheckUserServiceOptions options;
    private readonly ImmutableSortedDictionary<string, User> usersHash;

    public InMemoryCheckUserService(IOptions<InMemoryCheckUserServiceOptions> options)
    {
        try
        {
            this.options = options.Value;
            this.usersHash = this.options.Users.Select((u, i) => new { u, i = i + 1 })
                .ToImmutableSortedDictionary(x => x.u.Username, y => new User
                {
                    CreatedAt = DateTime.Now,
                    Id = y.i,
                    Passwhash = y.u.Passwhash,
                    UpdatedAt = null,
                    Username = y.u.Username
                });
        }
        catch (Exception ex)
        {
            Debug.WriteLine(String.Format("There is error to read option for InMemoryCheckUserService. Error: {0}", ex)
                , "ERROR");
            this.options = new InMemoryCheckUserServiceOptions();
        }
    }

    public bool Authenticate(User user, string password)
        => user?.Passwhash == password;

    public async Task<bool> AuthenticateAsync(string username, string password)
    {
        var user = await FindByNameAsync(username).ConfigureAwait(false);

        return Authenticate(user, password);
    }

    public Task<User> FindByNameAsync(string username)
    {
        if (usersHash.TryGetValue(username, out var user))
        {
            return Task.FromResult(user);
        }

        return Task.FromResult(default(User));
    }
}
