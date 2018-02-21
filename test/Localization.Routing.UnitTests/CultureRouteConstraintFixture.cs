using Cure.AspNetCore.Localization.Routing;
using Xunit;

namespace Localization.Routing.UnitTests
{
    /// <summary>
    ///     Fixture to verify that <see cref="CultureRouteConstraint"/> works properly.
    /// </summary>
    public class CultureRouteConstraintFixture
    {        
        [Theory]
        [ClassData(typeof(CultureTestDataGenerator))]
        public void Should_Match_Culture(
            string route,
            bool shouldMatch)
        {
            // Arrange
            var constraint = new CultureRouteConstraint();

            // Act
            var match = constraint.Match(route);

            // Assert
            Assert.Equal(shouldMatch, match);
        }
    }
}