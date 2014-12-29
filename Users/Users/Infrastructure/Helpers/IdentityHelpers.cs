namespace Users.Infrastructure.Helpers
{
    using System.Web;
    using System.Web.Mvc;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.Owin;
    using Models.IdentityModels;
    using Users.Infrastructure.IdentityInfrastructure;

    public static class IdentityHelpers
    {
        public static MvcHtmlString GetUserName(this HtmlHelper html, string id)
        {
            var mgr = HttpContext.Current.GetOwinContext().GetUserManager<AppUserManager>();
            return new MvcHtmlString(mgr.FindById<AppUser, string>(id).UserName);
        }
    }
}