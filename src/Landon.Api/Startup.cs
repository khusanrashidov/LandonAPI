using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1); // Adding services needed for MVC for routing and controller logic for ASP.NET Core.
            services.AddRouting(options => options.LowercaseUrls = true); // To avoid routing middleware from rendering controller names in Pascal case.
            services.AddOpenApiDocument(); // Add OpenAPI generator to the services collection. For OpenAPI = Swagger = SwaggerUI.
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline middleware.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env) // The order matters.
        {
            if (env.IsDevelopment()) // We can set environment in launchSettings.json file that will point to EnvironmentName static class' public static readonly string's value.
            {
                app.UseDeveloperExceptionPage();
                app.UseOpenApi(); // Add OpenAPI 3.0 document serving middleware. Available at: http://localhost:<port>/swagger/v1/swagger.json. Requires AddOpenApiDocument() method added to the service collection.
                app.UseSwaggerUi(options => options.CustomHeadContent = "Landon API"); // Add Swagger UI to interact with the document. Available at: http://localhost:<port>/swagger. Demands UseOpenApi() method registered to the request middleware pipeline.
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
