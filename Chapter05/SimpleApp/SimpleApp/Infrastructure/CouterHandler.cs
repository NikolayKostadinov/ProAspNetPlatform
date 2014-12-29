using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SimpleApp.Infrastructure
{
    public class CounterHandler:IHttpHandler
    {
        private int handlerCounter;
        private int requestCounter = 0;

        public CounterHandler(int counter) 
        {
            this.handlerCounter = counter;
        }
        public void ProcessRequest(HttpContext context)
        {
            this.requestCounter++;
            context.Response.ContentType = "text/html";
            context.Response.Write(
                string.Format(
                "The counter vaue is {0} (Request {1} of 3)", 
                this.handlerCounter, this.requestCounter)
                );
        }

        public bool IsReusable
        {
            get
            {
               return (this.requestCounter < 2);
            }
        }
    }
}