using System.Collections;
using System.Collections.Generic;

namespace Localization.Routing.UnitTests
{
    /// <summary>
    ///     Test data for culture.
    /// </summary>
    public class CultureTestDataGenerator : IEnumerable<object[]>
    {
        private readonly IList<object[]> _data = new List<object[]>
        {
            new object[] {"en", true},
            new object[] { "EN", true },
            new object[] { "en-us", true },
            new object[] { "en-US", true },
            new object[] { "",false },
            new object[] { "en-us ", false },
            new object[] {"gibberish", false}

        };

        public IEnumerator<object[]> GetEnumerator() => _data.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}