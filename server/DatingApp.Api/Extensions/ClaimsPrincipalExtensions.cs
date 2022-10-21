using System.Security.Claims;

namespace DatingApp.Api.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static string GetUserName(this ClaimsPrincipal principal)
        => principal.FindFirst(ClaimTypes.Name)?.Value;

        public static Guid? GetUserId(this ClaimsPrincipal principal) {

            if (!Guid.TryParse(principal.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var userId))
                return default;

            return userId;
        }
    }
}