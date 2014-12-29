using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace Users.Infrastructure.IdentityInfrastructure
{
    public static class LocationClaimsProvider
    {
        public static IEnumerable<Claim> GetClaims(ClaimsIdentity user) 
        {
            List<Claim> claims = new List<Claim>();
            if (user.Name.ToLower() == "niki")
            {
                claims.Add(CreateClaim(ClaimTypes.PostalCode, "8000"));
                claims.Add(CreateClaim(ClaimTypes.StateOrProvince, "Burgas"));
            }
            else 
            {
                claims.Add(CreateClaim(ClaimTypes.PostalCode, "9123"));
                claims.Add(CreateClaim(ClaimTypes.StateOrProvince, "Сусурлеоо"));
            }
            return claims;
        }
 
        /// <summary>
        /// Creates the claim.
        /// </summary>
        /// <param name="postalCode">The Claim Type.</param>
        /// <param name="p">The Claim Value.</param>
        /// <returns></returns>
        private static Claim CreateClaim(string claimType, string value)
        {
            return new Claim(claimType, value, "External Claim", "Bate Hattan");
        }
    }
}