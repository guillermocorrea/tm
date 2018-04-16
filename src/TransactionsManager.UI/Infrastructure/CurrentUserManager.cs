using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace TransactionsManager.UI.Infrastructure
{
    /// <summary>
    /// Extension methods to handle User claims.
    /// </summary>
    public static class CurrentUserManager
    {
        public static string Name(this ClaimsPrincipal user)
        {
            if (user != null && user.Claims != null)
            {
                var name = user.Claims.FirstOrDefault(c => c.Type == "name");
                if (name != null) return name.Value;
            }

            return "User";
        }

        public static bool HasRole(this ClaimsPrincipal user, string role)
        {
            if (user != null && user.Claims != null)
            {
                var name = user.Claims.FirstOrDefault(c => c.Type == "role");
                if (name == null) return false;
                return name.Value == role;
            }

            return false;
        }

        public static bool IsAssistant(this ClaimsPrincipal user)
        {
            return HasRole(user, "assistant");
        }

        public static bool IsManager(this ClaimsPrincipal user)
        {
            return HasRole(user, "manager");
        }

        public static bool IsAdministrator(this ClaimsPrincipal user)
        {
            return HasRole(user, "admin");
        }
    }
}
