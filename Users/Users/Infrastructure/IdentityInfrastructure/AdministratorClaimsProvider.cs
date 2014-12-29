namespace Users.Infrastructure.IdentityInfrastructure
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Web;
    using System.Web.Mvc;
    using Microsoft.AspNet.Identity.Owin;
    using Models.IdentityModels;

    public static class AdministratorClaimsProvider
    {
        public static IEnumerable<Claim> AddAdministratorAccessToRoles(Controller ctrl,  ClaimsIdentity user) 
        {
            if (user.IsAuthenticated && user.HasClaim(x => (x.Type == ClaimTypes.Role) && (x.Value == "Administrators")))
            {
                var roleManager = ctrl.HttpContext.GetOwinContext().GetUserManager<AppRoleManager>();
                IEnumerable<AppRole> roles = roleManager.Roles.Where(x => x.IsAvailableForAdministrators);
                List<Claim> claims = new List<Claim>();

                foreach (AppRole role in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role.Name, "Authomaticly provided for Administrator"));
                }
                return claims;
            }
            else
            {
                return new List<Claim>();
            }
        }
    }
}