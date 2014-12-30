namespace Users.Controllers
{
    using System;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Mvc;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.Owin;
    using Microsoft.Owin.Security;
    using Models.IdentityModels;
    using OtpSharp;
    using Users.Infrastructure.Helpers;
    using Base32;
    using Users.Infrastructure.IdentityInfrastructure;
    using Users.ViewModels.IdentityViewModels;

    [Authorize]
    public class AccountController : Controller
    {
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                return View("Error", new string[] { "Достъпът е отказан" });
            }

            TempData["returnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel details)
        {
            string returnUrl = TempData["returnUrl"] == null ? "" : TempData["returnUrl"].ToString();

            if (ModelState.IsValid)
            {
                AppUser user = await UserManager.FindAsync(details.UserName,
                    details.Password);
                if (user == null)
                {
                    ModelState.AddModelError("", "Невалидно потребителско име или парола!");
                }
                else
                {

                    //ClaimsIdentity ident = await UserManager.CreateIdentityAsync(user,
                    //    DefaultAuthenticationTypes.ApplicationCookie);

                    //for two factors authentication
                    ClaimsIdentity ident = await UserManager.CreateIdentityAsync(user,
                        DefaultAuthenticationTypes.ApplicationCookie);
                    ident.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id));
                   
                    AuthManager.SignOut();

                    if (!user.IsGoogleAuthenticatorEnabled)
                    {
                        return RedirectToAction("EnableGoogleAuthenticator", new { returnUrl = returnUrl });
                    }

                    if (!await TryToSignInAsync(user, details.Password))
                    {
                        if (!await SignInHelper.SendTwoFactorCode("GoogleAuthenticator"))
                        {
                            return View("Error", new string[] { "Въвели сте невалиден Google код!" });
                        }
                    }

                    return RedirectToAction("VerifyCode", new { Provider = "GoogleAuthenticator", ReturnUrl = returnUrl });
                }
            }
            TempData["returnUrl"] = returnUrl;
            return View(details);
        }

        [Authorize]
        public ActionResult Logout()
        {
            AuthManager.SignOut();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> EnableGoogleAuthenticator(string returnUrl)
        {
            byte[] secretKey = KeyGeneration.GenerateRandomKey(20);
            string userName = User.Identity.GetUserName();
            string barcodeUrl = KeyUrl.GetTotpUrl(secretKey, userName) + "&issuer=" + Properties.Settings.Default.ApplicationName;

            var model = new GoogleAuthenticatorViewModel
            {
                SecretKey = Base32Encoder.Encode(secretKey),
                BarcodeUrl = HttpUtility.UrlEncode(barcodeUrl)
            };
            TempData["returnAction"] = returnUrl;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EnableGoogleAuthenticator(GoogleAuthenticatorViewModel model)
        {
            string returnUrl = TempData["returnUrl"] == null ? "" : TempData["returnUrl"].ToString();
            
            if (ModelState.IsValid)
            {
                byte[] secretKey = Base32Encoder.Decode(model.SecretKey);

                long timeStepMatched = 0;
                var otp = new Totp(secretKey);
                if (otp.VerifyTotp(model.Code, out timeStepMatched, new VerificationWindow(2, 2)))
                {
                    var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                    user.IsGoogleAuthenticatorEnabled = true;
                    user.TwoFactorEnabled = true;
                    user.GoogleAuthenticatorSecretKey = model.SecretKey;
                    await UserManager.UpdateAsync(user);

                    return Redirect(returnUrl);
                }
                else
                    ModelState.AddModelError("Code", "The Code is not valid");
            }

            TempData["returnUrl"] = returnUrl;
            return View(model);
        }

        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl)
        {
            var userId = await SignInHelper.GetVerifiedUserIdAsync();
            if (userId == null)
            {
                return View("Error");
            }
            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl });
        }

        //
        // POST: /Account/SendCode
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            // Generate the token and send it
            if (!ModelState.IsValid)
            {
                return View();
            }

            if (!await SignInHelper.SendTwoFactorCode(model.SelectedProvider))
            {
                return View("Error");
            }
            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl });
        }

        private AppUserManager UserManager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<AppUserManager>();
            }
        }

        //
        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl)
        {
            // Require that the user has already logged in via username/password or external login
            if (!await SignInHelper.HasBeenVerified())
            {
                return View("Error", new string[] { "Не сте влезли в системата!" });
            }
            var user = await UserManager.FindByIdAsync(await SignInHelper.GetVerifiedUserIdAsync());
            if (user != null)
            {
                // To exercise the flow without actually sending codes, uncomment the following line
                ViewBag.Status = "For DEMO purposes the current " + provider + " code is: " + await UserManager.GenerateTwoFactorTokenAsync(user.Id, provider);
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl });
        }

        //
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            Users.Infrastructure.Helpers.SignInStatus result = await SignInHelper.TwoFactorSignIn(model.Provider, model.Code, isPersistent: false, rememberBrowser: model.RememberBrowser);
            switch (result)
            {
                case Users.Infrastructure.Helpers.SignInStatus.Success:
                    var ident = HttpContext.User.Identity as ClaimsIdentity;
                    ident.AddClaims(AdministratorClaimsProvider.AddAdministratorAccessToRoles(this, ident));
                    return RedirectToLocal(model.ReturnUrl);
                case Users.Infrastructure.Helpers.SignInStatus.LockedOut:
                    return View("Lockout");
                case Users.Infrastructure.Helpers.SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid code.");
                    return View(model);
            }
        }

        private IAuthenticationManager AuthManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        #region Helpers

        private SignInHelper _helper;

        private SignInHelper SignInHelper
        {
            get
            {
                if (_helper == null)
                {
                    _helper = new SignInHelper(UserManager, AuthManager);
                }
                return _helper;
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        private async Task<bool> TryToSignInAsync(AppUser user, string password) 
        {
            Infrastructure.Helpers.SignInStatus result = await SignInHelper.PasswordSignIn(user.UserName, password, false, shouldLockout: false);
            switch (result)
            {
                case Infrastructure.Helpers.SignInStatus.Success:
                    return true;
                default:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return false;
            }
        }

        #endregion
    }
}