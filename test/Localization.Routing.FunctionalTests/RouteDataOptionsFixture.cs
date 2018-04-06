using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Xunit;

namespace Localization.Routing.FunctionalTests
{
    public class RouteOptionsFixture
    {
        [Fact]
        public async Task Should_UseDefault()
        {
            var builder = new WebHostBuilder()
                          .ConfigureServices(services =>
                          {
                              services.AddRequestRouteLocalization("sv-SE", "en-US", "fr-FR");
                          })
                          .Configure(app =>
                              {
                                  app.UseRouteDataRequestLocalization(context => Task.FromResult(0));
                              }
                          );

            using (var server = new TestServer(builder))
            {
                var client = server.CreateClient();
                client.DefaultRequestHeaders.AcceptLanguage.ParseAdd("sv-SE");
                var response = await client.GetAsync(string.Empty);
                Assert.StartsWith("/sv-SE", response.Headers.Location.ToString());
            }
        }

        [Fact]
        public async Task Should_RespectRouteOptions_Lowercase()
        {
            var builder = new WebHostBuilder()
                          .ConfigureServices(services =>
                          {
                              services.AddRouting(options =>
                              {
                                  options.LowercaseUrls = true;
                              });
                              services.AddRequestRouteLocalization("sv-SE", "en-US", "fr-FR");
                          })
                          .Configure(app =>
                              {
                                  app.UseRouteDataRequestLocalization(context => Task.FromResult(0));
                              }
                          );
            using (var server = new TestServer(builder))
            {
                var client = server.CreateClient();
                client.DefaultRequestHeaders.AcceptLanguage.ParseAdd("sv");
                var response = await client.GetAsync(string.Empty);
                Assert.StartsWith("/sv-se", response.Headers.Location.ToString());
            }
        }

        [Fact]
        public async Task Should_RespectRouteOptions_TrailingSlash()
        {
            var builder = new WebHostBuilder()
                          .ConfigureServices(services =>
                          {
                              services.AddRouting(options =>
                              {
                                  options.AppendTrailingSlash = true;
                              });
                              services.AddRequestRouteLocalization("sv-SE", "en-US", "fr-FR");
                          })
                          .Configure(app =>
                              {
                                  app.UseRouteDataRequestLocalization(context => Task.FromResult(0));
                              }
                          );
            using (var server = new TestServer(builder))
            {
                var client = server.CreateClient();
                client.DefaultRequestHeaders.AcceptLanguage.ParseAdd("sv");
                var response = await client.GetAsync(string.Empty);
                Assert.StartsWith("/sv-SE/", response.Headers.Location.ToString());
            }
        }
    }
}
