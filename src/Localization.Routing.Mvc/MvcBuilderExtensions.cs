using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace
namespace Microsoft.AspNetCore.Builder
{
    public static class MvcBuilderExtensions
    {
        /// <summary>
        ///     Adds MVC to the <see cref="IApplicationBuilder" /> request execution pipeline
        ///     with a default route named 'default' and the following template:
        ///     '{culture:culture}/{controller=Home}/{action=Index}/{id?}'.
        /// </summary>
        /// <remarks>
        ///     Two more templates are added to map <c>GET</c> requests.
        ///     1. '{culture:culture}/{*path}' is to prevent infinte redirect loop if the 'default' route does not match.
        ///     2. Handle redirect to 'default' route (Template value '{*path}'        
        /// </remarks>
        /// <param name="app">The <see cref="IApplicationBuilder" />.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static IApplicationBuilder UseMvcWithDefaultRouteLocalization(this IApplicationBuilder app)
        {
            return app.UseMvc(routes =>
            {
                // TODO(joacar) Create templates based on values provided in options class
                routes.MapRoute(
                    "default",
                    "{culture:culture}/{controller=Home}/{action=Index}/{id?}");
                routes.UseRouteLocalization();                
            });
        }
    }
}
