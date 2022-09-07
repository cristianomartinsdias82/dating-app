using DatingApp.Api.Entities;

namespace DatingApp.Api.Services.AuthTokenIssuing
{
    public interface IAuthTokenIssuing
    {
         string IssueToken(AppUser user);
    }
}