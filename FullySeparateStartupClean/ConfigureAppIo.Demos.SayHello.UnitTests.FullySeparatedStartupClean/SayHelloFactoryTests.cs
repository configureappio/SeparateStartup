using System;
using System.Collections.Generic;
using System.Linq;
using ConfigureAppIo.Demos.SayHello.Common;
using ConfigureAppIo.Demos.SayHello.FullySeparatedStartupClean.Infra;
using FluentAssertions;
using Xunit;

namespace ConfigureAppIo.Demos.SayHello.UnitTests.FullySeparatedStartup
{
    public class SayHelloFactoryTests
    {
        [Fact]
        public void SayHelloFactory_Should_Throw_Exception_With_Null_Parameter()
        {
            Action actionToTest = () =>
            {
                var dummy = new SayHelloFactory(null);
            };

            actionToTest.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void SayHelloFactory_Should_Ignore_Null_Instances_In_Parameter()
        {
            var instances = new ISayHello[] {
                new English.EnglishHello(), new EnglishInformal.EnglishInformalHello(), new French.FrenchHello()
            };

            IEnumerable<ISayHello> InterleavedEnumerable()
            {
                foreach (var instance in instances)
                {
                    yield return instance;
                    yield return null;
                }
            }

            var sut = new SayHelloFactory(InterleavedEnumerable());

            sut.GetAllHellos().Should().BeEquivalentTo(instances.Cast<object>());
        }

        [Fact]
        public void SayHelloFactory_Should_Throw_Single_Exception_When_One_Duplicate()
        {
            var duplicateInstance = new English.EnglishHello();

            var instances = new List<ISayHello> {
                new English.EnglishHello(), new EnglishInformal.EnglishInformalHello(), new French.FrenchHello(), duplicateInstance
            };

            Action actionToTest = () =>
            {
                var dummy = new SayHelloFactory(instances);
            };


            var expectedMessage = DuplicateLanguageException.GetMessage(duplicateInstance.Language,
                instances.IndexOf(duplicateInstance), duplicateInstance.GetType());

            actionToTest.Should().Throw<DuplicateLanguageException>().WithMessage(expectedMessage);
        }

        [Fact]
        public void SayHelloFactory_Should_Throw_Aggregate_Exception_When_Two_Duplicates()
        {
            var duplicateInstance1 = new English.EnglishHello();
            var duplicateInstance2 = new French.FrenchHello();

            var instances = new List<ISayHello> {
                new English.EnglishHello(), new EnglishInformal.EnglishInformalHello(), new French.FrenchHello(), duplicateInstance1, duplicateInstance2
            };

            var expectedMessage1 = DuplicateLanguageException.GetMessage(duplicateInstance1.Language,
                instances.IndexOf(duplicateInstance1), duplicateInstance1.GetType());

            var expectedMessage2 = DuplicateLanguageException.GetMessage(duplicateInstance2.Language,
                instances.IndexOf(duplicateInstance2), duplicateInstance2.GetType());

            AggregateException exceptionCaught = null;

            var expectedMessages = new[] { expectedMessage1, expectedMessage2 };

            try
            {
                var dummy = new SayHelloFactory(instances);
            }
            catch (Exception ex)
            {
                exceptionCaught = ex as AggregateException;
            }

            Assert.NotNull(exceptionCaught);

            exceptionCaught.InnerExceptions.Cast<DuplicateLanguageException>().Select(d => d.Message).Should().BeEquivalentTo(expectedMessages);

        }
    }
}
