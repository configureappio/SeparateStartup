using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ConfigureAppIo.Demos.SayHello.Common;
using ConfigureAppIo.Demos.SayHello.Domain;
using ConfigureAppIo.Demos.SayHello.English;
using ConfigureAppIo.Demos.SayHello.French;
using Microsoft.AspNetCore.Mvc;

namespace ConfigureAppIo.Demos.SayHello.FullySeparatedStartupClean.Controllers
{
    public class HomeController : Controller
    {
        private readonly ISayHelloFactory _helloFactory;
        private readonly IEnumerable<ISayHelloLanguage> _languageHellos;
        private readonly IEnumerable<ISayHello> _allHellos;
        private readonly IEnglishHello _englishHello;
        private readonly IFrenchHello _frenchHello;

        public HomeController(ISayHelloFactory helloFactory,
            IEnumerable<ISayHelloLanguage> languageHellos,
            IEnumerable<ISayHello> allHellos,
            IEnglishHello englishHello,
            IFrenchHello frenchHello)
        {
            _helloFactory = helloFactory;
            _languageHellos = languageHellos;
            _allHellos = allHellos;
            _englishHello = englishHello;
            _frenchHello = frenchHello;
        }


        public string Index()
        {
            return GetAllMessagesAsSingleString();
        }

        [HttpGet("[action]")]
        public IEnumerable<string> GetMessages()
        {
            return GetAllMessagesAsSingleString().Split(Environment.NewLine).Where(s => !string.IsNullOrWhiteSpace(s));
        }

        [HttpPost("[action]")]
        public string SayHelloToMe([FromBody] Person person)
        {
            var localLanguage = _languageHellos.LastOrDefault(l => string.Compare(person.Language.Trim(), l.Language, StringComparison.CurrentCultureIgnoreCase) == 0);
            var prefixMessage = string.Empty;

            if (localLanguage == null)
            {
                localLanguage = _englishHello;
                prefixMessage =
                    $"I'm sorry, I don't speak {person.Language}. I can talk to you in {localLanguage.Language}. ";
            }

            return $"{prefixMessage}{localLanguage.GetHello()} {person.Name}";
        }

        private string GetAllMessagesAsSingleString() => BuildAllMessagesAsSingleMessage(_helloFactory, _languageHellos, _allHellos, _englishHello,
            _frenchHello);


        // Internal for unit testing purposes.
        internal static string BuildAllMessagesAsSingleMessage(ISayHelloFactory helloFactory,
            IEnumerable<ISayHelloLanguage> languageHellos,
            IEnumerable<ISayHello> allHellos,
            IEnglishHello englishHello,
            IFrenchHello frenchHello)
        {
            var sb = new StringBuilder();

            sb.AppendLine("Here are all the results from the Say Hello injected classes");
            sb.AppendLine();
            sb.AppendLine($"The IEnglishHello says in {englishHello.Language} '{englishHello.GetHello()}'");
            sb.AppendLine($"The IFrenchHello says in {frenchHello.Language} '{frenchHello.GetHello()}'");
            sb.AppendLine();
            sb.AppendLine("Here are all the results from the IEnumerable<ISayHelloLanguage> injected classes");
            sb.AppendLine();

            foreach (var langHello in languageHellos)
            {
                sb.AppendLine($"The {langHello.GetType().Name} says in {langHello.Language} '{langHello.GetHello()}'");
            }

            sb.AppendLine();
            sb.AppendLine("Here are all the results from the IEnumerable<ISayHello> injected classes\n");

            foreach (var langHello in allHellos)
            {
                sb.AppendLine($"The {langHello.GetType().Name} says '{langHello.GetHello()}'");
            }

            sb.AppendLine();
            sb.AppendLine("Here are all the results from the ISayHelloFactory.GetSupportedLanguageHellos()");
            sb.AppendLine();

            foreach (var langHello in helloFactory.GetSupportedLanguageHellos())
            {
                sb.AppendLine($"The {langHello.GetType().Name} says in {langHello.Language} '{langHello.GetHello()}'");
            }

            sb.AppendLine();
            sb.AppendLine("Here are all the results from the ISayHelloFactory.GetAllLanguageHellos()");
            sb.AppendLine();

            foreach (var langHello in helloFactory.GetAllLanguageHellos())
            {
                sb.AppendLine($"The {langHello.GetType().Name} says in {langHello.Language} '{langHello.GetHello()}'");
            }

            sb.AppendLine();
            sb.AppendLine("Here are all the results from the ISayHelloFactory.GetAllHellos()");
            sb.AppendLine();

            foreach (var langHello in helloFactory.GetAllHellos())
            {
                sb.AppendLine($"The {langHello.GetType().Name} says '{langHello.GetHello()}'");
            }

            return sb.ToString();
        }
    }
}
