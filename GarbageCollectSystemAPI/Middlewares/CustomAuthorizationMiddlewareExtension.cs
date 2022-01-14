using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Net;
using System.Threading.Tasks;

namespace GarbageCollectSystemAPI.Middlewares
{
    //Create custom middleware class
    public class CustomAuthorizationMiddleware
    {
        private readonly RequestDelegate _next;

        public CustomAuthorizationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            //Catch GetByID endpoint of Vehicle Controller
            if (context.Request.Method == "GET" && context.Request.Path == "/api/Vehicle/GetByID")
            {
                //Return error message
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.Forbidden;

                string message = "HTTP/1.1 403 Forbidden";
                var result = JsonConvert.SerializeObject(new { Errormessage = message }, Formatting.None);
                await context.Response.WriteAsync(result);
                return;
            }
           
            //Call other middleware
            await _next.Invoke(context);
        }
    }

    //Create middleware extension
    static public class CustomAuthorizationMiddlewareExtension { 
    
      static public IApplicationBuilder CustomAuthorizationMiddleware(this IApplicationBuilder builder)
      {
            return builder.UseMiddleware<CustomAuthorizationMiddleware>();
      }
    }
}
