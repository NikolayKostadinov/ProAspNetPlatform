using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;

namespace RequestFlow.Infrastructure
{
    public class HandlerSelectionModule:IHttpModule
    {
        /// <summary>
        /// Initializes a module and prepares it to handle requests.
        /// </summary>
        /// <param name="app">An <see cref="T:System.Web.HttpApplication" /> that provides
        /// access to the methods, properties, and events common to all application objects
        /// within an ASP.NET application</param>
        public void Init(HttpApplication app)
        {
            app.PostResolveRequestCache += (src, args) => 
            {
                if (!Compare(app.Context.Request.RequestContext.RouteData.Values, "controller","Home"))
                {
                    app.Context.RemapHandler(new InfoHandler());
                }
            };
        }
 
        /// <summary>
        /// Compares the specified rvd.
        /// </summary>
        /// <param name="rvd">The rvd.</param>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        private bool Compare(RouteValueDictionary rvd, string key, string value)
        {
            return string.Equals((string)rvd[key], value,
                StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Disposes of the resources (other than memory) used by the module that
        /// implements <see cref="T:System.Web.IHttpModule" />.
        /// </summary>
        public void Dispose()
        {
            //do nothing
        }
    }
}