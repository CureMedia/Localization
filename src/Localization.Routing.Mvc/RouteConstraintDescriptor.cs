using System;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;

namespace Localization.Routing.Mvc
{
    /// <summary>
    ///     Descriptor for adding <see cref="IRouteConstraint" /> to the <see cref="FilterCollection" />.
    /// </summary>
    public class RouteConstraintDescriptor
    {
        /// <summary>
        ///     Name for the route constraint.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     Type of the route constraint.
        /// </summary>
        public Type Type { get; set; }
    }
}