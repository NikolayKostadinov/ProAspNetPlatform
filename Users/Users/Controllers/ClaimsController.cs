namespace Users.Controllers
{
    using System.Security.Claims;
    using System.Web;
    using System.Web.Mvc;

    public class ClaimsController : Controller
    {
        [Authorize]
        public ActionResult Index()
        {
            ClaimsIdentity ident = HttpContext.User.Identity as ClaimsIdentity;
            if (ident == null)
            {
                return View("Error", new string[] {"No Claims Available"});
            }
            return View(ident.Claims);
        }
    }
}