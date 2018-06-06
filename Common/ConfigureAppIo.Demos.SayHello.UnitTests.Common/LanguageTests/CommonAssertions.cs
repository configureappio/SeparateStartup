using ConfigureAppIo.Demos.SayHello.Common;
using FluentAssertions;

namespace ConfigureAppIo.Demos.SayHello.UnitTests.Common.LanguageTests
{
    public static class CommonAssertions
    {
        public static void TestLanguagePopulated(ISayHelloLanguage sut)
        {
            sut.Language.Should().NotBeNullOrWhiteSpace();
        }

        public static void TestNamePopulated(ISayHello sut)
        {
            sut.GetHello().Should().NotBeNullOrWhiteSpace();
        }
    }
}
