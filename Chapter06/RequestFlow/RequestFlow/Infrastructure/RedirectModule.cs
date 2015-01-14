using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace RequestFlow.Infrastructure
{
    public class RedirectModule:IHttpModule
    {
        /// <summary>
        /// Initializes a module and prepares it to handle requests.
        /// </summary>
        /// <param name="app">An <see cref="T:System.Web.HttpApplication" /> that provides
        /// access to the methods, properties, and events common to all application objects
        /// within an ASP.NET application</param>
        public void Init(HttpApplication app)
        {
            app.MapRequestHandler += (src, attr) => 
            {
                RouteValueDictionary rvd = 
                    app.Context.Request.RequestContext.RouteData.Values;
                if (Compare(rvd, "controller", "Home")&&
                    Compare(rvd, "action", "Authenticate"))
                {
                    string url = UrlHelper.GenerateUrl("","Index","Home",rvd,RouteTable.Routes
                        ,app.Context.Request.RequestContext,false);
                    app.Context.Response.Redirect(url);
                }
            };
        }
 
        /// <summary>
        /// Compares the specified RVD.
        /// </summary>
        /// <param name="rvd">The RVD.</param>
        /// <param name="p">The p.</param>
        /// <param name="p1">The p1.</param>
        /// <returns></returns>
        private bool Compare(RouteValueDictionary rvd, string key, string value)
        {
            return string.Equals((string)rvd[key], value,StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Disposes of the resources (other than memory) used by the module that
        /// implements <see cref="T:System.Web.IHttpModule" />.
        /// </summary>
        public void Dispose()
        {
            //do notheng
        }
    }
}