using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SimpleApp.Infrastructure
{
    public class DayModule:IHttpModule
    {
        public void Init(HttpApplication app)
        {
            app.PostMapRequestHandler += (src, args) => 
            {
                if (app.Context.Handler is IRequestDate)
                {
                    app.Context.Items["DayModule_Time"] = DateTime.Now;   
                }
            };
        }

        public void Dispose()
        {
            //do nothing
        }
    }
}