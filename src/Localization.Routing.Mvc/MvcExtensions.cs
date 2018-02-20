using Cure.AspNetCore.Localization.Routing;
using Cure.AspNetCore.Localization.Routing.Abstractions;
using Cure.AspNetCore.Localization.Routing.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    public static class MvcExtensions
    {
        /// <summary>
        ///     Add the class <see cref="LocalizationPipeline" /> inside <see cref="MiddlewareFilterAttribute" /> to the
        ///     <see cref="FilterCollection" /> for
        ///     <paramref name="mvc" />.
        /// </summary>
        /// <param name="mvc"></param>
        /// <returns>Instance of <see cref="IMvcBuilder"/>.</returns>
        public static IMvcBuilder AddMvcDefaultRouteLocalization(this IMvcBuilder mvc)
        {
            return mvc
                .AddRequestLocalizationFilter<LocalizationPipeline>();
        }

        /// <summary>
        ///     Add the class <typeparamref name="T" /> to the <see cref="FilterCollection" /> for <paramref name="mvc" />.
        /// </summary>
        /// <typeparam name="T">Type to wrap a <see cref="MiddlewareFilterAttribute" /> around.</typeparam>
        /// <param name="mvc"></param>
        /// <returns>Instance of <see cref="IMvcBuilder"/>.</returns>
        public static IMvcBuilder AddRequestLocalizationFilter<T>(this IMvcBuilder mvc)
        {
            mvc.AddMvcOptions(options =>
                options.Filters.Add(new MiddlewareFilterAttribute(typeof(T))));
            return mvc;
        }

        /// <summary>
        ///     Register two routes for <c>GET</c> request.
        ///     1. '{culture:culture}/{*path}' is to prevent infinte redirect loop if the 'default' route does not match.
        ///     2. Handle redirect to 'default' route (Template value '{*path}'
        /// </summary>
        /// <param name="routes"></param>
        public static void UseRouteLocalization(this IRouteBuilder routes)
        {
            var cultureUrl = routes.ApplicationBuilder.ApplicationServices.GetService<IRouteDataRequestCultureUrl>();
            // TODO(joacar) Create templates using options
            routes.MapGet("{culture:culture}/{*path}", context => cultureUrl.PreventRedirectLoop(context));
            routes.MapGet("{*path}", context => cultureUrl.RedirectToCulture(context));
        }
    }
}