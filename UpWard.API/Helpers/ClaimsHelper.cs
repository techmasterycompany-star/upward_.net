using System.Security.Claims;

namespace Upwork.API.Helpers
{
    public static class ClaimsHelper
    {
        public static long GetUserId(ClaimsPrincipal user)
        {
            var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!long.TryParse(userId, out var id))
                throw new UnauthorizedAccessException("User identity could not be determined from the token.");

            return id;
        }
    }
}
