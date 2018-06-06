using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using Xunit;
using FluentAssertions;

namespace ConfigureAppIo.Demos.SayHello.UnitTests.Shared
{
    [ExcludeFromCodeCoverage]
    public abstract class BaseExceptionTests<T> where T : Exception, new()
    {
        private const string ExpectedMessage = "An error occurred.";

        protected virtual string GetExpectedMessage() => ExpectedMessage;

        protected virtual string GetMessageInput() => ExpectedMessage;

        protected abstract string ExpectedMessageWhenEmptyConstructor { get; }

        private readonly AccessViolationException _expectedInnerException =
            new AccessViolationException(ExpectedMessage);

        [Fact]
        public virtual void ConstructorWithNoParametersShouldHaveNoMessage()
        {
            var sut = new T();
            Action invocation = () => throw sut;
            invocation.Should().Throw<T>().WithMessage(ExpectedMessageWhenEmptyConstructor);
        }

        [Fact]
        public virtual void ConstructorWithSingleStringParametersShouldHaveMatchingMessage()
        {
            var expectedMessage = GetExpectedMessage();
            var sut = (T) Activator.CreateInstance(typeof(T), GetMessageInput());
            Action invocation = () => { throw sut; };
            invocation.Should().Throw<T>()
                .WithMessage(expectedMessage);
        }

        [Fact]
        public virtual void ConstructorWithStringAndExceptionParametersShouldHaveMatchingMessageAndException()
        {
            var sut = (T) Activator.CreateInstance(typeof(T), GetMessageInput(), _expectedInnerException);
            Action invocation = () => { throw sut; };
            invocation.Should().Throw<T>().WithMessage(GetExpectedMessage())
                .WithInnerExceptionExactly<AccessViolationException>()
                .WithMessage(_expectedInnerException.Message);
        }

        [Fact]
        public virtual void ConstructorWithSerializationShouldMatchSerializedException()
        {
            using (var stream = new MemoryStream())
            {
                var serializedException =
                    (T) Activator.CreateInstance(typeof(T), GetMessageInput(), _expectedInnerException);
                var formatter = new BinaryFormatter(null, new StreamingContext(StreamingContextStates.File));
                formatter.Serialize(stream, serializedException);
                stream.Position = 0;
                var deserializedException = (T) formatter.Deserialize(stream);

                Action invocation = () => throw deserializedException;
                invocation.Should().Throw<T>().WithMessage(GetExpectedMessage())
                    .WithInnerExceptionExactly<AccessViolationException>()
                    .WithMessage(_expectedInnerException.Message);
            }
        }
    }
}
