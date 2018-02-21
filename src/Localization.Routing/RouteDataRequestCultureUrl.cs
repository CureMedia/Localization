using System.Threading.Tasks;
using Cure.AspNetCore.Localization.Routing.Abstractions;
using Microsoft.AspNetCore.Http;

namespace Cure.AspNetCore.Localization.Routing
{
    /// <summary>
    ///     Extensions methods for implementations of <see cref="IRouteDataRequestCultureUrl" />.
    /// </summary>
    public static class RouteDataRequestCultureUrl
    {
        /// <summary>
        ///     Redirect the <paramref name="context" /> to the URL provided by <paramref name="url" />.
        /// </summary>
        /// <param name="url"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static Task RedirectToCulture(this IRouteDataRequestCultureUrl url, HttpContext context)
        {
            context.Response.Redirect(url.GetUrl(context));
            return Task.CompletedTask;
        }

        /// <summary>
        ///     If the culture route data is missing we might end in an redirect loop if the path does not match any previously
        ///     registered route.
        /// </summary>
        /// <param name="url"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static Task PreventRedirectLoop(this IRouteDataRequestCultureUrl url, HttpContext context)
        {
            context.Response.StatusCode = 404; // Not Found
            return Task.CompletedTask;
        }
    }
}