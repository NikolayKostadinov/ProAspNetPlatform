namespace Users.Infrastructure.IdentityInfrastructure
{
    using System;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using Data;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Microsoft.AspNet.Identity.Owin;
    using Microsoft.Owin;
    using Models.IdentityModels;

    public class AppUserManager : UserManager<AppUser>
    {
        private readonly Users.Properties.Settings passwordSettings = Properties.Settings.Default;

        public AppUserManager(IUserStore<AppUser> store)
            : base(store)
        {
            this.PasswordValidator = new PasswordValidator
            {
                RequiredLength = passwordSettings.RequiredLength,
                RequireNonLetterOrDigit = passwordSettings.RequireNonLetterOrDigit,
                RequireDigit = passwordSettings.RequireDigit,
                RequireLowercase = passwordSettings.RequireLowercase,
                RequireUppercase = passwordSettings.RequireUppercase,
            };
        }

        /// <summary>
        /// Validates the email async.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <returns></returns>
        public async Task<IdentityResult> ValidateEmailAsync(string email)
        {
            string pattern = @"^[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,4}$";

            // Instantiate the regular expression object.
            Regex r = new Regex(pattern, RegexOptions.IgnoreCase);
            if (r.IsMatch(email))
            {
                return IdentityResult.Success;
            }
            else 
            {
                return IdentityResult.Failed(new string[] { "Невалидна електронна поща" });
            }
        }

        /// <summary>
        /// Passwords the validator.
        /// </summary>
        /// <param name="newPassword">The new password.</param>
        /// <returns></returns>

        public static AppUserManager Create(
            IdentityFactoryOptions<AppUserManager> options,
            IOwinContext context)
        {
            var manager = new AppUserManager(new UserStore<AppUser>(context.Get<DataContext>()));
            var passwordSettings = Properties.Settings.Default;
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = passwordSettings.RequiredLength,
                RequireNonLetterOrDigit = passwordSettings.RequireNonLetterOrDigit,
                RequireDigit = passwordSettings.RequireDigit,
                RequireLowercase = passwordSettings.RequireLowercase,
                RequireUppercase = passwordSettings.RequireUppercase,
            };
<<<<<<< HEAD

            manager.RegisterTwoFactorProvider("GoogleAuthenticator", new GoogleAuthenticatorTokenProvider());

=======
>>>>>>> f3b07ea5dc9aaa7841c016eb3f0dcb9e18295e02
            return manager;
        }


    }
}