using Landon.Api.Filters;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Landon.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) // The order does not matter.
        {
            services
                .AddMvc(options => options.Filters.Add<JsonExceptionFilter>()) // Adding our custom exception filter to ASP.NET.
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1); // Adding services needed for MVC for routing and controller logic for ASP.NET Core.
            services.AddRouting(options => options.LowercaseUrls = true); // To avoid routing middleware from rendering controller names in Pascal case.
            services.AddOpenApiDocument(options =>
            {
                options.Title = "Landon API";
                options.Version = "K8";
                options.DocumentName = "Landon";
            }); // Add OpenAPI generator to the services collection. For OpenAPI = Swagger = SwaggerUI.
            services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1, 0); // 1 is the major version, 0 is the minor version. Same as the [ApiVersion("1.0")] in the RootController class.
                options.ApiVersionReader = new MediaTypeApiVersionReader(); // To read from Content Type aka Media Type.
                options.AssumeDefaultVersionWhenUnspecified = true; // Assume early defined DefaultApiVerion 1.0 if the version is not specified.
                options.ReportApiVersions = true; // To get the API version information on the responses.
                options.ApiVersionSelector = new CurrentImplementationApiVersionSelector(options); // Pass the current options instance as the implementation of the ApiVersionSelector.
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline middleware.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env) // The order matters.
        {
            if (env.IsDevelopment()) // We can set environment in launchSettings.json file that will point to EnvironmentName static class' public static readonly string's value.
            {
                app.UseDeveloperExceptionPage();
                app.UseOpenApi(); // Add OpenAPI 3.0 document serving middleware. Available at: http://localhost:<port>/swagger/v1/swagger.json. Requires AddOpenApiDocument() method added to the service collection.
                app.UseSwaggerUi(options => { options.DocumentTitle = "Landon API Swagger"; }); // Add Swagger UI to interact with the document. Available at: http://localhost:<port>/swagger. Demands UseOpenApi() method registered to the request middleware pipeline.
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
