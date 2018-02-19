using Cure.AspNetCore.Localization.Routing;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;

namespace Localization.Routing.Sample
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddLocalization();
            services.AddRouting(options =>
            {
                options.AppendTrailingSlash = true;
                options.LowercaseUrls = true;
            });
            services.AddRequestRouteLocalization("sv-SE", "en");
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(
            IApplicationBuilder app, 
            IHostingEnvironment env,
            IStringLocalizerFactory localizerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouteDataRequestLocalization(async context =>
            {
                var localizer = context.RequestServices.GetService<IStringLocalizer<Resource>>();
                var f = context.Features.Get<IRequestCultureFeature>();
                var rv = context.GetRouteValue("culture");
                var rd = context.GetRouteData();
                await context.Response.WriteAsync(localizer["HelloWorld"]);
            });
        }
    }
}
