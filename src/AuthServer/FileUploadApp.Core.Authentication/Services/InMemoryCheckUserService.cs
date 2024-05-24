using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

using FileUploadApp.Domain;

using Microsoft.Extensions.Options;

namespace FileUploadApp.Authentication.Services;

public class InMemoryCheckUserServiceOptions
{
    public const string SectionKey = "AuthServer:InMemoryUsers";

    public List<User> Users { get; set; } = [];

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
    private readonly InMemoryCheckUserServiceOptions _options;
    private readonly ImmutableSortedDictionary<string, User> _usersHash;

    public InMemoryCheckUserService(IOptions<InMemoryCheckUserServiceOptions> options)
    {
        try
        {
            this._options = options.Value;
            this._usersHash = CreateUsers(this._options.Users);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(String.Format("There is error to read option for InMemoryCheckUserService. Error: {0}", ex)
                , "ERROR");
            this._options = new InMemoryCheckUserServiceOptions();
            this._usersHash = CreateUsers([]);
        }
    }

    public bool Authenticate(User? user, string password)
        => user?.Passwhash == password;

    public async Task<bool> AuthenticateAsync(string username, string password)
    {
        var user = await FindByNameAsync(username).ConfigureAwait(false);

        return user != null && Authenticate(user, password);
    }

    public Task<User?> FindByNameAsync(string username)
    {
        _usersHash.TryGetValue(username, out var user);

        return Task.FromResult(user);
    }

    private static ImmutableSortedDictionary<string, User> CreateUsers(IEnumerable<User> users)
        => users.Select((u, i) => new { u, i = i + 1 })
                .ToImmutableSortedDictionary(x => x.u.Username, y => new User
                {
                    CreatedAt = DateTime.Now,
                    Id = y.i,
                    Passwhash = y.u.Passwhash,
                    UpdatedAt = null,
                    Username = y.u.Username
                });
}
