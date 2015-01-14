using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RequestFlow.Infrastructure
{
    public class DebugModule:IHttpModule
    {
        private static List<string> requestUrls = new List<string>();
        private static object lockObject = new object();
        /// <summary>
        /// Initializes a module and prepares it to handle requests.
        /// </summary>
        /// <param name="app">An <see cref="T:System.Web.HttpApplication" /> that provides
        /// access to the methods, properties, and events common to all application objects
        /// within an ASP.NET application</param>
        public void Init(HttpApplication app)
        {
            app.BeginRequest += (src, args) => 
            {
                lock (lockObject) 
                {
                    if (app.Request.RawUrl == "/Stats")
                    {
                        app.Response.Write(string.Format("<div>There have been {0} requests</div>",
                            requestUrls.Count));
                        app.Response.Write("<table><tr><th>ID</th><th>URL</th></tr>");
                        for (int i = 0; i < requestUrls.Count; i++)
                        {
                            app.Response.Write(
                                string.Format("<tr><td>{0}</td><td>{1}</td></tr>",
                                i, requestUrls[i]));
                        }
                        app.Response.Write("</table>");
                        app.CompleteRequest();
                    }
                    else 
                    {
                        requestUrls.Add(app.Request.RawUrl);
                    }
                }
            };
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