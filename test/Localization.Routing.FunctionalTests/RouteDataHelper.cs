using Cure.AspNetCore.Localization.Routing;
using Microsoft.Extensions.Options;

namespace Localization.Routing.FunctionalTests
{
    internal static class RouteDataHelper
    {
        public static MockRouteDataRequestCultureUrl RouteDataRequestCultureUrl()
        {
            return new MockRouteDataRequestCultureUrl(Options.Create(new RouteDataRequestCultureOptions()));
        }
    }
}