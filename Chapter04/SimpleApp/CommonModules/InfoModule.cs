using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace CommonModules
{
    public class InfoModule:IHttpModule
    {
        /// <summary>
        /// Initializes a module and prepares it to handle requests.
        /// </summary>
        /// <param name="context">An <see cref="T:System.Web.HttpApplication" /> that provides
        /// access to the methods, properties, and events common to all application objects
        /// within an ASP.NET application</param>
        public void Init(HttpApplication app)
        {
            app.EndRequest += HandleEvent;
        }
  
        /// <summary>
        /// Handles the event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs" /> instance containing the event data.</param>
        private void HandleEvent(object src, EventArgs args)
        {
            HttpContext context = HttpContext.Current;
            context.Response.Write(
                string.Format("<div class='alert alert-success'>URL: {0} Status: {1}</div>",
                context.Request.RawUrl, context.Response.StatusCode)
                );
        }

        /// <summary>
        /// Disposes of the resources (other than memory) used by the module that
        /// implements <see cref="T:System.Web.IHttpModule" />.
        /// </summary>
        public void Dispose()
        {
            // Do nothing 
        }
    }
}
