namespace Cure.AspNetCore.Localization.Routing
{
    public class RouteDataRequestCultureOptions
    {
        /// <summary>
        ///     Default regex pattern used to validate culture route values of the format <c>en-us</c> and <c>en</c>.
        /// </summary>
        public const string DefaultRegexPattern = @"^[a-zA-Z]{2}(\-[a-zA-Z]{2})?$";

        /// <summary>
        ///     Regex pattern used to validate culture route values.
        /// </summary>
        public string RegexPattern = DefaultRegexPattern;

        public string RouteConstraintKey { get; set; } = "culture";

        /// <summary>
        /// </summary>
        public string CultureRouteKey { get; set; } = "culture";

        /// <summary>
        /// </summary>
        public string UiCultureRouteKey { get; set; } = "ui-culture";
    }
}