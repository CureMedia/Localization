using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Cure.AspNetCore.Localization.Routing
{
    /// <summary>
    ///     Simple non-regex tester for culture compatible strings.
    /// </summary>
    public class CultureRouteConstraint : IRouteConstraint
    {
        /// <inheritdoc />
        public bool Match(
            HttpContext httpContext,
            IRouter route,
            string routeKey,
            RouteValueDictionary values,
            RouteDirection routeDirection)
        {
            if (httpContext == null)
            {
                throw new ArgumentNullException(nameof(httpContext));
            }
            if (route == null)
            {
                throw new ArgumentNullException(nameof(route));
            }
            if (string.IsNullOrEmpty(routeKey))
            {
                throw new ArgumentNullException(nameof(routeKey));
            }
            if (values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }
            if (values.TryGetValue(routeKey, out var obj) && obj is string value)
            {
                return IsMatch(value);
            }

            return false;
        }

        protected bool IsMatch(string value)
        {
            // Short circuit based on length
            if (value.Length != 2 && value.Length != 5)
            {
                return false;
            }

            if (!IsAlpha(value[0]) || !IsAlpha(value[1]))
            {
                return false;
            }

            switch (value.Length)
            {
                case 2:
                    return true;
                case 5 when value[2] == '-':
                    return IsAlpha(value[3]) && IsAlpha(value[4]);
                default:
                    return false;
            }

            bool IsAlpha(char c)
            {
                return 'A' <= c && c <= 'Z' ||
                       'a' <= c && c <= 'z';
            }
        }
    }
}