using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;

namespace Cure.AspNetCore.Localization.Routing.Mvc
{
    /// <summary>
    ///     Adapter to invoke add the <see cref="RequestLocalizationMiddleware" />
    /// </summary>
    public class LocalizationPipeline
    {
        /// <summary>
        ///     Add the <see cref="RequestLocalizationMiddleware" /> to <paramref name="app" /> with <paramref name="options" />.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="options"></param>
        public void Configure(IApplicationBuilder app, RequestLocalizationOptions options)
        {
            app.UseRequestLocalization(options);
        }
    }
}