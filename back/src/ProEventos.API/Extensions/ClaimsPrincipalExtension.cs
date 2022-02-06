using System.Security.Claims;

namespace ProEventos.API.Extensions
{
    public static class ClaimsPrincipalExtension
    {
        public static string GetUserName(this ClaimsPrincipal claim)
        {
            return claim.FindFirst(ClaimTypes.Name)?.Value;
        }

        public static int GetUserId(this ClaimsPrincipal claim)
        {
            return int.Parse(claim.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        }
    }
}
