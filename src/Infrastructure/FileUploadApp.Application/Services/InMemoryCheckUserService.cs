using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

using FileUploadApp.Domain;
using FileUploadApp.Interfaces;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace FileUploadApp.Application.Services;

internal sealed class InMemoryCheckUserServiceOptionsSetup(IConfiguration configuration)
    : IConfigureOptions<InMemoryCheckUserServiceOptions>
{

    const string SectionKey = "AuthServer:InMemoryUsers";

    public void Configure(InMemoryCheckUserServiceOptions options)
    {
        configuration.GetSection(SectionKey).Bind(options);
    }
}

public class InMemoryCheckUserServiceOptions
{
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

internal sealed class InMemoryCheckUserService : ICheckUserService<User>
{
    private readonly InMemoryCheckUserServiceOptions _options;
    private readonly ImmutableSortedDictionary<string, User> _usersHash;

    public InMemoryCheckUserService(IOptions<InMemoryCheckUserServiceOptions> options)
    {
        try
        {
            _options = options.Value;
            _usersHash = CreateUsers(_options.Users);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(string.Format("There is error to read option for InMemoryCheckUserService. Error: {0}", ex)
                , "ERROR");
            _options = new InMemoryCheckUserServiceOptions();
            _usersHash = CreateUsers([]);
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
