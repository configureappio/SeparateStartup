using ConfigureAppIo.Demos.SayHello.English;
using Xunit;

namespace ConfigureAppIo.Demos.SayHello.UnitTests.Common.LanguageTests
{
    public class EnglishTest
    {
        [Fact]
        public void IsNamePopulated()
        {
            CommonAssertions.TestNamePopulated(new EnglishHello());
        }

        [Fact]
        public void IsLanguagePopulated()
        {
            CommonAssertions.TestLanguagePopulated(new EnglishHello());
        }
    }
}
