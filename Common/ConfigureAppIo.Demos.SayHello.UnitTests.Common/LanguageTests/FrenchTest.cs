using ConfigureAppIo.Demos.SayHello.French;
using Xunit;

namespace ConfigureAppIo.Demos.SayHello.UnitTests.Common.LanguageTests
{
    public class FrenchTest
    {
        [Fact]
        public void IsNamePopulated()
        {
            CommonAssertions.TestNamePopulated(new FrenchHello());
        }

        [Fact]
        public void IsLanguagePopulated()
        {
            CommonAssertions.TestLanguagePopulated(new FrenchHello());
        }
    }
}
