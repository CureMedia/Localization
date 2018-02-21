using Cure.AspNetCore.Localization.Routing;
using Cure.AspNetCore.Localization.Routing.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace
namespace Microsoft.AspNetCore.Builder
{
    public static class RouteDataRequestCultureUrlExtensions
    {
        /// <summary>
        ///     Add the <see cref="RequestLocalizationMiddleware" /> to the execution pipeline with the configured
        ///     <see cref="RequestLocalizationOptions" />.
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseRequestRouteLocalization(
            this IApplicationBuilder app)
        {
            var options = app.ApplicationServices.GetService<RequestLocalizationOptions>();
            app.UseRequestLocalization(options);
            return app;
        }

        /// <summary>
        ///     Add the<see cref="RequestLocalizationMiddleware" /> to the execution pipeline with the configured
        ///     <see cref="RequestLocalizationOptions" /> and set up two routes; one to execute <paramref name="requestDelegate" />
        ///     and the other to handle redirects to url culture route.
        /// </summary>
        /// <remarks>
        ///     The template for <paramref name="requestDelegate" /> is '{culture:culture}/{*path}' and the template for redirect
        ///     is '{*path}'.
        /// </remarks>
        /// <param name="app"></param>
        /// <param name="requestDelegate"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseRouteDataRequestLocalization(
            this IApplicationBuilder app,
            RequestDelegate requestDelegate)
        {
            // Configure default request localization options
            app.UseRequestRouteLocalization();
            var cultureUrl = app.ApplicationServices.GetService<IRouteDataRequestCultureUrl>();
            // TODO(joacar) Create templates based on values provided in options class
            app.UseRouter(routes =>
            {
                routes.MapGet("{culture:culture}/{*path}", requestDelegate);
                routes.MapGet("{*path}", context => cultureUrl.RedirectToCulture(context));
            });
            return app;
        }
    }
}