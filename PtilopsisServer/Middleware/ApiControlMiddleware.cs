using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PtilopsisServer.Middleware
{
    public class ApiControlMiddleware : IMiddleware
    {
        public ApiControlMiddleware(IConfiguration config)
        {

        }
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            context.Response.Headers.Add("test", "111");
            await next.Invoke(context);
        }
    }
}
