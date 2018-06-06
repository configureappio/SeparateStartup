using System.Linq;
using ConfigureAppIo.Demos.SayHello.Common;
using FluentAssertions;
using Xunit;

namespace ConfigureAppIo.Demos.SayHello.UnitTests.Common.LanguageTests
{
    public class PersonValidationTests
    {
        [Fact]
        public void SupportedLanguagesEnumerableReturnsExpecetedList()
        {
            var expectedResults = new[] {SupportedLanguages.English, SupportedLanguages.French};
            SupportedLanguages.AsEnumerable().ToArray().Should().BeEquivalentTo(expectedResults);
        }

    }
}
