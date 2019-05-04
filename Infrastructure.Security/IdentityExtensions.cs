using System;
using System.Security.Claims;
using System.Security.Principal;

namespace Infrastructure.Security
{
    public static class IdentityExtensions
    {
        public static Guid GetUserId(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst("UserId");
            // Test for null to avoid issues during local testing
            return claim != null ? Guid.Parse(claim.Value) : throw new ArgumentNullException(nameof(claim));
        }
        public static string GetFullName(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst("Name");
            // Test for null to avoid issues during local testing
            return !string.IsNullOrWhiteSpace(claim?.Value) ? claim.Value : throw new ArgumentNullException(nameof(claim));
        }
    }
}