using Cure.AspNetCore.Localization.Routing;
using Cure.AspNetCore.Localization.Routing.Abstractions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
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
                Assert.IsType< AcceptLanguageHeaderRequestCultureProvider>(cultureUrl.CultureFeature.Provider);
                Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
                Assert.StartsWith("sv", response.Headers.Location.ToString().TrimStart('/').TrimEnd('/'));
            }

            Assert.True(redirectShortCircuit);
        }

        [Fact]
        public async Task Should_Use_Route_Data_Culture()
        {
            var redirectShortCircuit = true;
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
                                  redirectShortCircuit = false;
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

            Assert.False(redirectShortCircuit);            
        }

        [Fact(Skip = "TODO How to pass cookie to TesteServer.Client")]
        public async Task Should_Redirect_To_CultureUrl_Based_On_Cookie()
        {
            var redirectShortCircuit = true;
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
                                      redirectShortCircuit = false;
                                      return Task.FromResult(0);
                                  });
                              }
                          );
            
            using (var server = new TestServer(builder))
            {
                
                var client = server.CreateClient();                
                var cookieContainer = CreateRequestCultureContainer(client.BaseAddress);
                client = new HttpClient(new HttpClientHandler
                {
                    CookieContainer = cookieContainer
                })
                {
                    BaseAddress = client.BaseAddress
                };
                client.DefaultRequestHeaders.AcceptLanguage.ParseAdd("sv-SE,no,en-US");
                var response = await client.GetAsync(string.Empty);
                Assert.NotNull(cultureUrl.CultureFeature);
                Assert.IsType<CookieRequestCultureProvider>(cultureUrl.CultureFeature.Provider);
                Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
                Assert.StartsWith("fr", response.Headers.Location.ToString().TrimStart('/').TrimEnd('/'));
            }

            Assert.True(redirectShortCircuit);

            CookieContainer CreateRequestCultureContainer(Uri baseAddress)
            {
                var cookieContainer = new CookieContainer();
                var cookieCultureProvider = new CookieRequestCultureProvider();
                cookieContainer.Add(baseAddress, new Cookie(cookieCultureProvider.CookieName,
                    CookieRequestCultureProvider.MakeCookieValue(new RequestCulture("fr", "FR")), "/"));
                return cookieContainer;
            }
        }

        [Fact]
        public async Task Should_Return_Not_Found_To_Prevent_Redirect_Loop()
        {
            var routeNotMatched = false;
            var builder = new WebHostBuilder()
                          .ConfigureServices(services =>
                          {
                              services.AddRequestRouteLocalization("sv", "en");
                          })
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
    }
}