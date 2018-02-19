using Microsoft.AspNetCore.Http;

namespace Cure.AspNetCore.Localization.Routing.Abstractions
{
    /// <summary>
    ///     Abstraction for resolving a URL with culture route data.
    /// </summary>
    public interface IRouteDataRequestCultureUrl
    {
        /// <summary>
        ///     Get the URL with culture route data for <paramref name="context" />.
        /// </summary>
        /// <param name="context">Current <see cref="HttpContext" />.</param>
        /// <returns>A URL with culture route data.</returns>
        string GetUrl(HttpContext context);

        /// <summary>
        ///     Check if the URL for <paramref name="context" /> as culture route data.
        /// </summary>
        /// <param name="context">Current <see cref="HttpContext" />.</param>
        /// <returns><c>true</c> if <paramref name="context" /> has culture route data; otherwise <c>false</c>.</returns>
        bool HasCultureRouteData(HttpContext context);
    }
}