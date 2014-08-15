using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using CommonModules;

namespace SimpleApp.Infrastructure
{
    public class TimerModule:IHttpModule
    {
        private Stopwatch timer;
        public event EventHandler<RequestTimerEventArgs> RequestTimed;

        /// <summary>
        /// Initializes a module and prepares it to handle requests.
        /// </summary>
        /// <param name="context">An <see cref="T:System.Web.HttpApplication" /> that provides
        /// access to the methods, properties, and events common to all application objects
        /// within an ASP.NET application</param>
        public void Init(HttpApplication app)
        {
            app.BeginRequest += HandleEvent;
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
            if (context.CurrentNotification == RequestNotification.BeginRequest)
            {
                timer = Stopwatch.StartNew();
            }
            else 
            {
                float duration =((float)timer.ElapsedTicks)/Stopwatch.Frequency;
                context.Response.Write(string.Format(
                    "<div class='alert alert-success'> Elabsed: {0:F5} seconds</div>",
                    ((float)timer.ElapsedTicks)/Stopwatch.Frequency));
                if (RequestTimed != null) 
                {
                    RequestTimed(this, new RequestTimerEventArgs() { Duration = duration });
                }
            }
        }

        /// <summary>
        /// Disposes of the resources (other than memory) used by the module that
        /// implements <see cref="T:System.Web.IHttpModule" />.
        /// </summary>
        public void Dispose()
        {
            // do nothing - no resource to release
        }
    }
}