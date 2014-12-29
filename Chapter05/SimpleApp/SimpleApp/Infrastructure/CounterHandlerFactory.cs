using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SimpleApp.Infrastructure
{
    public class CounterHandlerFactory : IHttpHandlerFactory
    {
        private int counter = 0;
        private int handlerMaxCount = 3;
        private int handlerCount = 0;
        private BlockingCollection<CounterHandler> pool =
            new BlockingCollection<CounterHandler>();
        public IHttpHandler GetHandler(HttpContext context, string requestType,
            string url, string pathTranslated)
        {
            CounterHandler handler;
            if (!pool.TryTake(out handler))
            {
                if (this.handlerCount < this.handlerMaxCount)
                {
                    handlerCount++;
                    handler = new CounterHandler(++this.counter);
                    pool.Add(handler);
                }
                else 
                {
                    handler = pool.Take();
                }
            }

            return handler;
            //if (context.Request.UserAgent.Contains("Chrome"))
            //{
            //    return new SiteLenghtHandler();
            //}
            //else
            //{
            //    return new CounterHandler(++this.counter);
            //}
        }

        public void ReleaseHandler(IHttpHandler handler)
        {
            if (handler.IsReusable)
            {
                pool.Add((CounterHandler)handler);
            }
            else 
            {
                this.handlerCount--;
            }
        }
    }
}