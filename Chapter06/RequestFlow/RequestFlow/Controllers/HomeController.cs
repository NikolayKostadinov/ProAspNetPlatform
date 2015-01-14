using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RequestFlow.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Authenticate() 
        {
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Calculate(int value = 0) 
        {
            int result = 100 / value;
            return View("Index");
        }
    }
}