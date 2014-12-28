using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Users.Controllers
{
    [Authorize(Roles="Administrators")]
    public abstract class BaseController : Controller
    {
    }
}