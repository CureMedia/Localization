using System.Globalization;
using System.Text.RegularExpressions;
using Cure.AspNetCore.Localization.Routing.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;

namespace Cure.AspNetCore.Localization.Routing
{
    /// <summary>
    ///     Implemenation of <see cref="IRouteDataRequestCultureUrl" /> that use <see cref="RouteDataRequestCultureOptions" />
    ///     and
    ///     <see cref="IRequestCultureFeature" /> to generate correct redirects URL if culture route data is missing.
    /// </summary>
    public class DefaultRouteDataRequestCultureUrl : IRouteDataRequestCultureUrl
    {
        private readonly RouteDataRequestCultureOptions _options;

        /// <summary>
        ///     Create an instance of <see cref="DefaultRouteDataRequestCultureUrl" /> with <paramref name="options" />.
        /// </summary>
        /// <param name="options">The <see cref="RouteDataRequestCultureOptions" /> to use.</param>
        public DefaultRouteDataRequestCultureUrl(IOptions<RouteDataRequestCultureOptions> options) =>
            _options = options.Value;

        /// <inheritdoc />
        public string GetUrl(HttpContext context)
        {
            var culture = GetRequestCulture(context);
            var redirectUrl = GetRedirectUrl(context, culture.Culture, culture.UICulture);
            return redirectUrl;
        }


        /// <inheritdoc />
        public bool HasCultureRouteData(HttpContext context)
        {
            var routeData = GetCultureRouteData(context);
            // TODO(joacar) Refactor or resolve the IRouteConstraint that is registered with the key
            return Regex.IsMatch(routeData, _options.RegexPattern);
        }

        /// <summary>
        ///     Get the culture route data for <paramref name="context" />.
        /// </summary>
        /// <remarks>
        ///     When overriding this then <see cref="GetRedirectUrl" /> MUST also be overridden.
        /// </remarks>
        /// <param name="context"></param>
        /// <returns></returns>
        protected string GetCultureRouteData(HttpContext context)
        {
            var routeData = context.GetRouteValue(_options.CultureRouteKey) as string;
            if (!string.IsNullOrEmpty(routeData))
            {
                return routeData;
            }

            // Routing has not been applied. Resolve to manual detection
            var index = context.Request.Path.HasValue ? context.Request.Path.Value.IndexOf('/', 1) : -1;
            if (index < 0)
            {
                return string.Empty;
            }

            // TODO(joacar) This should be refactored since it is extremely coupled to GetRedirectUrl implementation
            // It is not clear that overriding this also MUST override GetRedirectUrl.
            return context.Request.Path.Value.Substring(0, index).TrimStart('/').TrimEnd('/');            
        }

        /// <summary>
        ///     Get the <see cref="RequestCulture" /> for <paramref name="context" />.
        /// </summary>
        /// <remarks>
        ///     This is done by resolving the <see cref="IRequestCultureFeature" /> from <see cref="HttpContext.Features" /> and
        ///     taking the <see cref="RequestCulture" /> object.
        /// </remarks>
        /// <param name="context">Current <see cref="HttpContext" />.</param>
        /// <returns>The <see cref="RequestCulture" />.</returns>
        protected virtual RequestCulture GetRequestCulture(HttpContext context)
        {
            var requestCultureFeature = context.Features.Get<IRequestCultureFeature>();
            return requestCultureFeature.RequestCulture;
        }

        /// <summary>
        ///     Get the relative culture URL to redirect.
        /// </summary>
        /// <remarks>
        ///     When overriding this then <see cref="GetCultureRouteData" /> MUST also be overridden.
        /// </remarks>
        /// <param name="context">Current <see cref="HttpContext" />.</param>
        /// <param name="culture">The culture.</param>
        /// <param name="uiCulture">The UI culture.</param>
        /// <returns>A relative URL including the culture.</returns>
        protected virtual string GetRedirectUrl(
            HttpContext context,
            CultureInfo culture,
            CultureInfo uiCulture)
        {
            // TODO(joacar) Perhaps move to RouteDataRequestCultureOptions and have GetRedirectUrl
            var path = GetPath(context);            
            return $"/{culture.Name}{path}";
        }

        /// <summary>
        ///     Get the relative path to append to the culture template.
        /// </summary>
        /// <param name="context">Current <see cref="HttpContext" />.</param>
        /// <returns>The relative path.</returns>
        protected virtual string GetPath(HttpContext context)
        {
            return context.Request.GetEncodedPathAndQuery();
        }
    }
}