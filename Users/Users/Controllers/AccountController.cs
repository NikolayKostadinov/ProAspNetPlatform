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
    using Users.Infrastructure.IdentityInfrastructure;
    using Users.ViewModels.IdentityViewModels;
    using Base32;

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
            ViewBag.retunUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel details, string returnUrl)
        {
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

                    ClaimsIdentity ident = await UserManager.CreateIdentityAsync(user,
                        DefaultAuthenticationTypes.ApplicationCookie);
                    ident.AddClaims(AdministratorClaimsProvider.AddAdministratorAccessToRoles(this, ident));
                    AuthManager.SignOut();
                    AuthManager.SignIn(new AuthenticationProperties
                    {
                        IsPersistent = true
                    }, ident);
                    return Redirect(returnUrl);
                }
            }
            ViewBag.returnUrl = returnUrl;
            return View(details);
        }

        [Authorize]
        public ActionResult Logout() 
        {
            AuthManager.SignOut();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult> EnableGoogleAuthenticator()
        {
            byte[] secretKey = KeyGeneration.GenerateRandomKey(20);
            string userName = User.Identity.GetUserName();
            string barcodeUrl = KeyUrl.GetTotpUrl(secretKey, userName) + "&issuer="+ Properties.Settings.Default.ApplicationName;

            var model = new GoogleAuthenticatorViewModel
            {
                SecretKey = Base32Encoder.Encode(secretKey),
                BarcodeUrl = HttpUtility.UrlEncode(barcodeUrl)
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EnableGoogleAuthenticator(GoogleAuthenticatorViewModel model)
        {
            if (ModelState.IsValid)
            {
                byte[] secretKey = Base32Encoder.Decode(model.SecretKey);

                long timeStepMatched = 0;
                var otp = new Totp(secretKey);
                if (otp.VerifyTotp(model.Code, out timeStepMatched, new VerificationWindow(2, 2)))
                {
                    var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                    //user.IsGoogleAuthenticatorEnabled = true;
                    user.GoogleAuthenticatorSecretKey = model.SecretKey;
                    await UserManager.UpdateAsync(user);

                    return RedirectToAction("Index", "Manage");
                }
                else
                    ModelState.AddModelError("Code", "The Code is not valid");
            }

            return View(model);
        }


        private AppUserManager UserManager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<AppUserManager>();
            }
        }

        private IAuthenticationManager AuthManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }
    }
}