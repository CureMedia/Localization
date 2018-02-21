using Cure.AspNetCore.Localization.Routing;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Options;

namespace Localization.Routing.FunctionalTests
{
    public sealed class MockRouteDataRequestCultureUrl : DefaultRouteDataRequestCultureUrl
    {
        public IRequestCultureFeature CultureFeature { get; private set; }

        public MockRouteDataRequestCultureUrl(IOptions<RouteDataRequestCultureOptions> options) : base(options)
        {
        }

        protected override RequestCulture GetRequestCulture(HttpContext context)
        {
            CultureFeature = context.Features.Get<IRequestCultureFeature>();
            return base.GetRequestCulture(context);
        }
    }
}