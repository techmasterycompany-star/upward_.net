using System.Security.Claims;

namespace Upward.API.Helpers
{
    public static class ClaimsHelper
    {
        public static long GetUserId(ClaimsPrincipal user)
        {
            return 1;
            var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!long.TryParse(userId, out var id))
                throw new UnauthorizedAccessException("Invalid user identity.");

            return id;
        }
    }
}
