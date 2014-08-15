using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SimpleApp.Infrastructure
{
    public class DayOfWeekHandler : IHttpHandler
    {
        /// <summary>
        /// Enables processing of HTTP Web requests by a custom HttpHandler that
        /// implements the <see cref="T:System.Web.IHttpHandler" /> interface.
        /// </summary>
        /// <param name="context">An <see cref="T:System.Web.HttpContext" /> object that provides
        /// references to the intrinsic server objects (for example, Request, Response, Session,
        /// and Server) used to service HTTP requests.</param>
        public void ProcessRequest(HttpContext context)
        {
            string day = DateTime.Now.DayOfWeek.ToString();
            if (context.Request.CurrentExecutionFilePathExtension == ".json")
            {
                context.Response.ContentType = "application/json";
                context.Response.Write(string.Format("{{\"day\": \"{0}\"}}", day));
            }
            else
            {
                context.Response.ContentType = "text/html";
                context.Response.Write(string.Format("<span>It is: {0}</span>", day));

            }
        }

        /// <summary>
        /// Gets a value indicating whether another request can use the <see cref="T:System.Web.IHttpHandler" />
        /// instance.
        /// </summary>
        /// <returns>true if the <see cref="T:System.Web.IHttpHandler" /> instance is reusable;
        /// otherwise, false.</returns>
        /// <value></value>
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}