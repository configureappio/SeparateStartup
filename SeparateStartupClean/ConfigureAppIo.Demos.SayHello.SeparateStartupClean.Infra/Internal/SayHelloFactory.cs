using System;
using System.Collections.Generic;
using System.Linq;
using ConfigureAppIo.Demos.SayHello.Common;

namespace ConfigureAppIo.Demos.SayHello.SeparateStartupClean.Infrastructure.Internal
{
    internal class SayHelloFactory : ISayHelloFactory
    {
        private readonly List<ISayHello> _nonLanguageImplementations = new List<ISayHello>();
        private readonly List<ISayHelloLanguage> _languageImplementations = new List<ISayHelloLanguage>();

        private readonly int[] _supportedLanguagesPointers;

        internal SayHelloFactory(IEnumerable<ISayHello> implementations)
        {
            if (implementations == null) throw new ArgumentNullException(nameof(implementations));

            var supportedLangs = SupportedLanguages.AsEnumerable().ToArray();
            int enumerableIndex = -1;
            int pointer = 0;
            var pointers = new List<int>();
            var exceptions = new List<DuplicateLanguageException>();

            foreach (var implementation in implementations)
            {
                enumerableIndex++;

                if (implementation == null) continue;

                if (implementation is ISayHelloLanguage impl)
                {
                    if (_languageImplementations.Any(l => string.Compare(impl.Language, l.Language, StringComparison.OrdinalIgnoreCase) == 0))
                    {
                        exceptions.Add(new DuplicateLanguageException(impl.Language, enumerableIndex, impl.GetType()));
                    }
                    else
                    {
                        _languageImplementations.Add(impl);
                        if (supportedLangs.Contains(impl.Language)) pointers.Add(pointer++);
                    }
                }
                else
                {
                    _nonLanguageImplementations.Add(implementation);
                }
            }

            _supportedLanguagesPointers = pointers.ToArray();

            if (exceptions.Any())
            {
                if (exceptions.Count > 1)
                {
                    throw new AggregateException(exceptions);
                }

                throw exceptions[0];
            }
        }
        
        public IEnumerable<ISayHelloLanguage> GetAllLanguageHellos() => _supportedLanguagesPointers.Select(p => _languageImplementations[p]);

        public IEnumerable<ISayHelloLanguage> GetSupportedLanguageHellos() => _languageImplementations;

        public IEnumerable<ISayHello> GetAllHellos() => _nonLanguageImplementations.Union(_languageImplementations);
    }
}
