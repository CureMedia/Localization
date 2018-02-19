using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Moq;

namespace Localization.Routing.UnitTests
{
    /// <summary>
    ///     Extension methods to ease testing of <see cref="IRouteConstraint" />.
    /// </summary>
    public static class RouteConstraintExtensions
    {
        /// <summary>
        ///     Extension method on <see cref="IRouteConstraint" /> that mocks parameters and create a reducer for
        ///     <see cref="IRouteConstraint.Match" /> that only require the route value to validate against the
        ///     <paramref name="constraint" />.
        /// </summary>
        /// <param name="constraint">The <see cref="IRouteConstraint" /> to reduce.</param>
        /// <returns>A <see cref="Func{TString, TResult}" /> that wrappes the <paramref name="constraint" /></returns>
        /// <code>Match</code>
        /// method.
        public static Func<string, bool> Matcher(this IRouteConstraint constraint)
        {
            var httpContext = new Mock<HttpContext>().Object;
            var router = new Mock<IRouter>().Object;

            return s => constraint.Match(
                httpContext,
                router,
                "culture",
                Values(s),
                RouteDirection.IncomingRequest);

            RouteValueDictionary Values(string value)
            {
                return new RouteValueDictionary(new
                {
                    culture = value
                });
            }
        }

        /// <summary>
        ///     Check if the <paramref name="value" /> is valid for the <paramref name="constraint" />.
        /// </summary>
        /// <param name="constraint">The <see cref="IRouteConstraint" />.</param>
        /// <param name="value">The value to match.</param>
        /// <returns><c>true</c> iff <see cref="IRouteConstraint.Match" />; otherwise <c>false</c>.</returns>
        public static bool Match(this IRouteConstraint constraint, string value)
        {
            return constraint.Matcher()(value);
        }
    }
}
