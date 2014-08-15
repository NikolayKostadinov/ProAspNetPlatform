using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SimpleApp
{
    public class MvcApplication : System.Web.HttpApplication
    {
        public MvcApplication()
        {
            BeginRequest += RecordEvent;
            AuthenticateRequest += RecordEvent;
            PostAuthenticateRequest += RecordEvent;

            PostAcquireRequestState += (src, args) =>
            {
                CreateTimeStamp();
            };
            //BeginRequest += (src, args) =>
            //{
            //    RecordEvent("BeginRequest");
            //};
            //AuthenticateRequest += (src, args) =>
            //{ 
            //    RecordEvent("AuthenticateRequest");
            //};
            //PostAuthenticateRequest += (src, args) =>
            //{ 
            //    RecordEvent("PostAuthenticateRequest"); 
            //};
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            CreateTimeStamp();
        }

        //protected void Application_BeginRequest() 
        //{ 
        //    RecordEvent("BeginRequest");
        //}

        //protected void Application_AuthenticateRequest() 
        //{
        //    RecordEvent("AuthenticateRequest");
        //}

        //protected void Application_PostAuthenticateRequest()
        //{
        //    RecordEvent("PostAuthenticateRequest");
        //}

        /// <summary>
        /// Records the event.
        /// </summary>
        /// <param name="name">The name of the event.</param>
        private void RecordEvent(object src, EventArgs argd)
        {
            List<string> eventList = Application["events"] as List<string>;
            if (eventList == null)
            {
                eventList = new List<string>();
                Application["events"] = eventList;
            }
            var name =
                Context.IsPostNotification ?
                "Post" + Context.CurrentNotification.ToString()
                : Context.CurrentNotification.ToString();

            eventList.Add(name);
        }

        private void CreateTimeStamp()
        {
            string stamp = Context.Timestamp.ToLongTimeString();
            if (Context.Session != null)
            {
                Session["request_timestamp"] = stamp;
            }
            else
            {
                Application["app_timestamp"] = stamp;
            }
        }
    }
}