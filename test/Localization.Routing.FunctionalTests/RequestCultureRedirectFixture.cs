using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Localization.Routing.FunctionalTests
{
    public class RequestCultureRedirectFixture
    {
        [Fact]
        public async Task Should_Redirect_To_CultureUrl_Based_On_AcceptHeaderLanguage()
        {
            var redirectShortCircuit = true;
            var builder = new WebHostBuilder()
                          .ConfigureServices(services =>
                          {
                              services.AddRequestRouteLocalization("sv", "en");
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
                Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
                Assert.StartsWith("sv", response.Headers.Location.ToString().TrimStart('/').TrimEnd('/'));
            }

            Assert.True(redirectShortCircuit);
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