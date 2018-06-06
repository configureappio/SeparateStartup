using System.Collections.Generic;

namespace ConfigureAppIo.Demos.SayHello.Common
{
    public interface ISayHelloFactory
    {
        IEnumerable<ISayHelloLanguage> GetAllLanguageHellos();
        IEnumerable<ISayHelloLanguage> GetSupportedLanguageHellos();
        IEnumerable<ISayHello> GetAllHellos();
    }
}
