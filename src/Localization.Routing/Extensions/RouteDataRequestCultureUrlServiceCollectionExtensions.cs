using System;
using System.Globalization;
using System.Linq;
using Cure.AspNetCore.Localization.Routing;
using Cure.AspNetCore.Localization.Routing.Abstractions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Localization.Routing;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    ///     Extensions methods to configure request localization on <see cref="IServiceCollection" />.
    /// </summary>
    public static class RouteDataRequestCultureUrlServiceCollectionExtensions
    {
        /// <summary>
        ///     Add an <see cref="IRouteConstraint" /> of <typeparamref name="T" /> with the <paramref name="name" /> to the
        ///     <see cref="RouteOptions.ConstraintMap" />.
        /// </summary>
        /// <typeparam name="T">Type of <see cref="IRouteConstraint"/>.</typeparam>
        /// <param name="services">Instance of <see cref="IServiceCollection"/> to configure.</param>
        /// <param name="name">Name for the constraint.</param>
        /// <returns>Instance of <see cref="IServiceCollection"/>.</returns>
        public static IServiceCollection AddRoute<T>(this IServiceCollection services,
            string name = "culture")
        {
            services.Configure<RouteOptions>(route => { route.ConstraintMap.Add(name, typeof(T)); });
            return services;
        }

        public static IServiceCollection AddRequestRouteLocalization(
            this IServiceCollection services,
            params string[] cultures
        )
        {
            if (cultures == null)
            {
                throw new ArgumentNullException(nameof(cultures));
            }

            if (cultures.Length == 0)
            {
                throw new ArgumentOutOfRangeException("At least one culture must be provided");
            }

            var cultureInfos = cultures.Select(c => new CultureInfo(c)).ToArray();
                          
            services.Configure<RequestLocalizationOptions>(options =>
            {
                options.DefaultRequestCulture = new RequestCulture(cultureInfos[0]);
                options.SupportedCultures = cultureInfos;
                options.SupportedUICultures = cultureInfos;
                options.RequestCultureProviders = new IRequestCultureProvider[]
                {
                    new RouteDataRequestCultureProvider
                    {
                        Options = options
                    },
                    new CookieRequestCultureProvider
                    {
                        Options = options
                    },
                    new AcceptLanguageHeaderRequestCultureProvider
                    {
                        Options = options
                    }
                };
            });
            services.AddSingleton(provider => provider.GetService<IOptions<RequestLocalizationOptions>>().Value);
            AddServices(services);
            return services;
        }

        private static void AddServices(IServiceCollection services)
        {
            services.AddRouting();
            services.AddRoute<CultureRouteConstraint>();
            services.Configure<RouteDataRequestCultureOptions>(options => { options.CultureRouteKey = "culture"; });
            services.AddSingleton<IRouteDataRequestCultureUrl, DefaultRouteDataRequestCultureUrl>();
        }
    }
}