using System.Collections.Generic;

namespace ConfigureAppIo.Demos.SayHello.Common
{
    public static class SupportedLanguages
    {
        private static readonly string[] KnownLanguages = {"English", "French"};

        public static string English => KnownLanguages[0];

        public static string French => KnownLanguages[1];

        public static IEnumerable<string> AsEnumerable() => KnownLanguages;

    }
}
