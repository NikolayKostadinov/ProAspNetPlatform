using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Base32;
using Microsoft.AspNet.Identity;
using Models.IdentityModels;
using OtpSharp;

namespace Users.Infrastructure.IdentityInfrastructure
{
    public class GoogleAuthenticatorTokenProvider : IUserTokenProvider<AppUser, string>
    {
        public Task<string> GenerateAsync(string purpose, UserManager<AppUser, string> manager, AppUser user)
        {
            return Task.FromResult((string)null);
        }

        public Task<bool> ValidateAsync(string purpose, string token, UserManager<AppUser, string> manager, AppUser user)
        {
            long timeStepMatched = 0;

            var otp = new Totp(Base32Encoder.Decode(user.GoogleAuthenticatorSecretKey));
            bool valid = otp.VerifyTotp(token, out timeStepMatched, new VerificationWindow(2, 2));

            return Task.FromResult(valid);
        }

        public Task NotifyAsync(string token, UserManager<AppUser, string> manager, AppUser user)
        {
            return Task.FromResult(true);
        }

        public Task<bool> IsValidProviderForUserAsync(UserManager<AppUser, string> manager, AppUser user)
        {
            return Task.FromResult(true);
        }
    }
}