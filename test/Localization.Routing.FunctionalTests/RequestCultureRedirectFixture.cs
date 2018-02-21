using System.Net;
using System.Threading.Tasks;
using Cure.AspNetCore.Localization.Routing;
using Cure.AspNetCore.Localization.Routing.Abstractions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Xunit;

namespace Localization.Routing.FunctionalTests
{
    public class RequestCultureRedirectFixture
    {
        private static MockRouteDataRequestCultureUrl RouteDataRequestCultureUrl()
        {
            return new MockRouteDataRequestCultureUrl(Options.Create(new RouteDataRequestCultureOptions()));
        }

        [Fact]
        public async Task Should_Redirect_To_CultureUrl_Based_On_AcceptHeaderLanguage()
        {
            var redirectShortCircuit = true;
            var cultureUrl = RouteDataRequestCultureUrl();
            var builder = new WebHostBuilder()
                          .ConfigureServices(services =>
                          {
                              services.AddRequestRouteLocalization("sv", "en");
                              services.AddSingleton<IRouteDataRequestCultureUrl>(cultureUrl);
                          })
                          .Configure(app =>
                              {
                                  app.UseRouteDataRequestLocalization(context =>
                                  {
                                      redirectShortCircuit = false;
                                      return Task.FromResult(0);
                                  });
                              }
                          );
            using (var server = new TestServer(builder))
            {
                var client = server.CreateClient();
                client.DefaultRequestHeaders.AcceptLanguage.ParseAdd("sv-SE,no,en-US");
                var response = await client.GetAsync(string.Empty);
                Assert.NotNull(cultureUrl.CultureFeature);
                Assert.IsType<AcceptLanguageHeaderRequestCultureProvider>(cultureUrl.CultureFeature.Provider);
                Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
                Assert.StartsWith("sv", response.Headers.Location.ToString().TrimStart('/').TrimEnd('/'));
            }

            Assert.True(redirectShortCircuit);
        }

        [Fact]
        public async Task Should_Redirect_To_CultureUrl_Based_On_Cookie()
        {
            var redirectShortCircuit = true;
            var cultureUrl = RouteDataRequestCultureUrl();
            var builder = new WebHostBuilder()
                          .ConfigureServices(services =>
                          {
                              services.AddRequestRouteLocalization("sv", "en", "fr");
                              services.AddSingleton<IRouteDataRequestCultureUrl>(cultureUrl);
                          })
                          .Configure(app =>
                              {
                                  app.UseRouteDataRequestLocalization(context =>
                                  {
                                      redirectShortCircuit = false;
                                      return Task.FromResult(0);
                                  });
                              }
                          );

            using (var server = new TestServer(builder))
            {
                var client = server.CreateClient();
                client.DefaultRequestHeaders.AcceptLanguage.ParseAdd("sv,no,en");
                client.DefaultRequestHeaders.Add("Cookie", CreateRequestCultureCookie("fr"));
                var response = await client.GetAsync(string.Empty);
                Assert.NotNull(cultureUrl.CultureFeature);
                Assert.IsType<CookieRequestCultureProvider>(cultureUrl.CultureFeature.Provider);
                Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
                Assert.StartsWith("fr", response.Headers.Location.ToString().TrimStart('/').TrimEnd('/'));
            }

            Assert.True(redirectShortCircuit);

            string CreateRequestCultureCookie(string culture)
            {
                var cookie = new Cookie(
                    CookieRequestCultureProvider.DefaultCookieName,
                    CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture, culture)), "/");
                return cookie.ToString();
            }
        }

        [Fact]
        public async Task Should_Return_Not_Found_To_Prevent_Redirect_Loop()
        {
            var routeNotMatched = false;
            var builder = new WebHostBuilder()
                          .ConfigureServices(services => { services.AddRequestRouteLocalization("sv", "en"); })
                          .Configure(app =>
                              {
                                  app.UseRouter(routes =>
                                  {
                                      routes.MapGet("{controller=Home}/{action=Index}",
                                          context =>
                                          {
                                              routeNotMatched = true;
                                              return Task.CompletedTask;
                                          });
                                  });
                                  app.UseRequestRouteLocalization();
                              }
                          );
            using (var server = new TestServer(builder))
            {
                var client = server.CreateClient();
                client.DefaultRequestHeaders.AcceptLanguage.ParseAdd("sv-SE,no,en-US");
                var response = await client.GetAsync("/sv/no/route/match");
                Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            }

            Assert.False(routeNotMatched);
        }

        [Fact]
        public async Task Should_Use_Route_Data_Culture()
        {
            var noRedirect = true;
            var cultureUrl = RouteDataRequestCultureUrl();
            var builder = new WebHostBuilder()
                          .ConfigureServices(services =>
                          {
                              services.AddRequestRouteLocalization("sv", "fr");
                              services.AddSingleton(cultureUrl);
                          })
                          .Configure(app =>
                              {
                                  app.UseRouteDataRequestLocalization(context =>
                                  {
                                      noRedirect = false;
                                      return Task.FromResult(0);
                                  });
                              }
                          );

            using (var server = new TestServer(builder))
            {
                var client = server.CreateClient();
                var response = await client.GetAsync("/sv");
                Assert.Null(cultureUrl.CultureFeature);
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            }

            Assert.False(noRedirect);
        }
    }
}