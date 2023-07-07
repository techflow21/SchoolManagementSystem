using System.Security.Claims;

namespace SchoolManagementSystem.Api.Extensions
{
    public static class UserExtension
    {
        public static string? GetUserId(this ClaimsPrincipal user)
        {
            return user.FindFirst("Id")?.Value;
        }
    }
}
