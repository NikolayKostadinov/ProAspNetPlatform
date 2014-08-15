using System;
using System.Linq;
using System.Web;

namespace CommonModules
{
    public class ModuleRegistration
    {
        public static void RegisterModule() 
        {
            HttpApplication.RegisterModule(typeof(CommonModules.InfoModule));
        }
    }
}
