using System;
using System.Runtime.Serialization;

namespace ConfigureAppIo.Demos.SayHello.SeparateStartupClean.Infrastructure
{
    [Serializable]
    public class DuplicateLanguageException : Exception
    {
        internal DuplicateLanguageException(string language, int index, Type type) : this(language,index,type, null)
        {
        }

        internal DuplicateLanguageException(string language, int index, Type type, Exception innerException) : base(GetMessage(language, index, type), innerException)
        {
        }

        protected internal DuplicateLanguageException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        internal static string GetMessage(string language, int index, Type type) => $"The language '{language}' at index[{index}] cannot be added as already registered. Type: {type.Name}";
    }
}
