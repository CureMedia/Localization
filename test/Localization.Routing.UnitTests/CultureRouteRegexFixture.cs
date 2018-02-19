using System;
using System.Text.RegularExpressions;
using Cure.AspNetCore.Localization.Routing;
using Xunit;

namespace Localization.Routing.UnitTests
{
    /// <summary>
    ///     Fixture to ensure that the regex pattern used to validate culture values works.
    /// </summary>
    public class CultureRouteRegexFixture
    {
        private readonly Regex _regex = new Regex(RouteDataRequestCultureOptions.DefaultRegexPattern);

        [Theory]
        [ClassData(typeof(CultureTestDataGenerator))]
        public void Should_Match_Culture(
            string route,
            bool shouldMatch)
        {
            // Arrange
            // Act
            var match = _regex.IsMatch(route);

            // Assert
            Assert.Equal(shouldMatch, match);
        }
    }
}