using System;
using System.Web;
using System.Web.Routing;

namespace SimpleApp.Infrastructure
{
    /// <summary>
    /// 
    /// </summary>
    public class CustomRouteHandler : IRouteHandler
    {
        public Type HandlerType { get; set; }

        /// <summary>
        /// Provides the object that processes the request.
        /// </summary>
        /// <param name="requestContext">An object that encapsulates information about the
        /// request.</param>
        /// <returns>An object that processes the request.</returns>
        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            return (IHttpHandler)Activator.CreateInstance(HandlerType);
        }
    }
}