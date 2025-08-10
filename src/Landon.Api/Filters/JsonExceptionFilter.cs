using Landon.Api.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Landon.Api.Filters
{
    public class JsonExceptionFilter : IExceptionFilter // A filter that runs after an action has thrown an System.Exception.
    {
        private readonly IHostingEnvironment env; // Or: _env;.

        public JsonExceptionFilter(IHostingEnvironment env)
        {
            this.env = env; // Or: _env = env;.
        }

        public void OnException(ExceptionContext context)
        {
            var error = new ApiError();

            if ((env.IsDevelopment() || env.IsStaging()) && !env.IsProduction()) // Or just env.IsDevelopment().
            {
                error.Message = context.Exception.Message;
                error.Detail = context.Exception.StackTrace;
            }
            else
            {
                error.Message = "An unexpected API error occurred in the server. Please try again later, client.";
                error.Detail = context.Exception.Message;
            }

            // Setting context.Result is like returning a response from the exception filter.
            context.Result = new ObjectResult(error) // It will serialize the error object to JSON.
            {
                StatusCode = 500 // Status Code for server error or unhandled exception from the Web API.
            };
        }
    }
}
