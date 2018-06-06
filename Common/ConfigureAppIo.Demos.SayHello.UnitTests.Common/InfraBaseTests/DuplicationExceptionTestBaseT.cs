using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using ConfigureAppIo.Demos.SayHello.English;
using FluentAssertions;
using Xunit;

namespace ConfigureAppIo.Demos.SayHello.UnitTests.Common.InfraBaseTests
{
    public abstract class DuplicateLanguageExceptionTestBase<T> where T:Exception
    {

        protected abstract string GetExpectedMessage(string language, int index, Type type);

        protected abstract T GetDuplicateLanguageException(string language, int index, Type type);

        protected abstract T GetDuplicateLanguageException(string language, int index, Type type, Exception innerException);

        
        private static string AccessViolationMessage = "Dummy Message";

        private readonly AccessViolationException _expectedInnerException =
            new AccessViolationException(AccessViolationMessage);

        private static readonly string Language = "Lang";

        private static readonly int Index = 10;

        private readonly Type _typeIdentified = typeof(EnglishHello);


        [Fact]
        public void ConstructorWithNoParametersShouldHaveNoMessage()
        {

            var sut = GetDuplicateLanguageException(Language, Index, _typeIdentified);
            Action invocation = () => throw sut;
            invocation.Should().Throw<T>().WithMessage(GetExpectedMessage(Language, Index, _typeIdentified));
        }

        [Fact]
        public void ConstructorWithStringAndExceptionParametersShouldHaveMatchingMessageAndException()
        {
            var sut = GetDuplicateLanguageException(Language, Index, _typeIdentified, _expectedInnerException);
            Action invocation = () => throw sut;
            invocation.Should().Throw<T>().WithMessage(GetExpectedMessage(Language, Index, _typeIdentified))
                .WithInnerExceptionExactly<AccessViolationException>()
                .WithMessage(_expectedInnerException.Message);
        }

        [Fact]
        public void ConstructorWithSerializationShouldMatchSerializedException()
        {
            using (var stream = new MemoryStream())
            {
                var serializedException = GetDuplicateLanguageException(Language, Index, _typeIdentified, _expectedInnerException);
                var formatter = new BinaryFormatter(null, new StreamingContext(StreamingContextStates.File));
                formatter.Serialize(stream, serializedException);
                stream.Position = 0;
                var deserializedException = (T) formatter.Deserialize(stream);

                Action invocation = () => throw deserializedException;
                invocation.Should().Throw<T>().WithMessage(GetExpectedMessage(Language, Index, _typeIdentified))
                    .WithInnerExceptionExactly<AccessViolationException>()
                    .WithMessage(AccessViolationMessage);
            }
        }
    }
}
