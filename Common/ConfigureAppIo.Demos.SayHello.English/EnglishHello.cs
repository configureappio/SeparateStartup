using ConfigureAppIo.Demos.SayHello.Common;

namespace ConfigureAppIo.Demos.SayHello.English
{
    public class EnglishHello : IEnglishHello
    {
        public string GetHello() => "Hello";
        public string Language => SupportedLanguages.English;
    }

    public interface IEnglishHello : ISayHelloLanguage
    {
    }
}
