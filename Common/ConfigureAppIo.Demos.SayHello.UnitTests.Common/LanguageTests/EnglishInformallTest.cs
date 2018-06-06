using ConfigureAppIo.Demos.SayHello.EnglishInformal;
using Xunit;

namespace ConfigureAppIo.Demos.SayHello.UnitTests.Common.LanguageTests
{
    public class EnglishInformalTest
    {
        [Fact]
        public void IsNamePopulated()
        {
            CommonAssertions.TestNamePopulated(new EnglishInformalHello());
        }
    }
}
