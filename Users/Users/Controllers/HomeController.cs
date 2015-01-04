using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Users.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View(GetData("Index"));
        }

        [Authorize(Roles="Users")]
        public ActionResult OtherAction()
        {
            return View("Index", GetData("OtherAction"));
        }
  
        /// <summary>
        /// Gets the data.
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        private Dictionary<string, object> GetData(string actionName)
        {
            Dictionary<string, object> dict =
                new Dictionary<string, object>();
            dict.Add("Action", actionName);
            dict.Add("User", HttpContext.User.Identity.Name);
            dict.Add("Authenticated", HttpContext.User.Identity.IsAuthenticated);
            dict.Add("Auth Type", HttpContext.User.Identity.AuthenticationType);
            dict.Add("In Roles Role", HttpContext.User.IsInRole("Users"));
            return dict;
        }

    }
}