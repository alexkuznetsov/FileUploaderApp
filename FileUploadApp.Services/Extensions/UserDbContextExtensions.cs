using Dapper;
using FileUploadApp.Domain;
using System.Data.Common;
using System.Threading.Tasks;

namespace FileUploadApp.Services.Extensions
{
    public static class UserDbContextExtensions
    {
        private const string FindClientByNameSQL = @"select id Id
  , created_at CreatedAt
  , updated_at UpdatedAt
  , username Username
  , passwhash Passwhash from users where username=@username";

        public static Task<User> FindClientByUserNameAsync(this DbConnection dBContext, string username)
            => dBContext.QueryFirstOrDefaultAsync<User>(FindClientByNameSQL, new { username });
    }
}
