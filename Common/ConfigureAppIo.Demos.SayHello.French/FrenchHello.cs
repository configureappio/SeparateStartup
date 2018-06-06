using ConfigureAppIo.Demos.SayHello.Common;

namespace ConfigureAppIo.Demos.SayHello.French
{
    public class FrenchHello : IFrenchHello
    {
        public string GetHello() => "Bonjour";
        public string Language => SupportedLanguages.French;
    }

    public interface IFrenchHello : ISayHelloLanguage
    {
    }
}
