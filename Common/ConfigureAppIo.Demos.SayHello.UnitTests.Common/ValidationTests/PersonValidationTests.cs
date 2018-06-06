using ConfigureAppIo.Demos.SayHello.Domain;
using FluentValidation.TestHelper;
using Xunit;

namespace ConfigureAppIo.Demos.SayHello.UnitTests.Common.ValidationTests
{
    public class PersonValidationTests
    {
        [Fact]
        public void PersonValidation_Has_NoError_When_FullyPopulated()
        {
            var sut = new PersonValidation();
            sut.ShouldHaveValidationErrorFor(x => x.Name, new Person { Language = "English" });
        }

        [Fact]
        public void PersonValidation_Has_Error_When_Null_Name()
        {
            var sut = new PersonValidation();
            sut.ShouldHaveValidationErrorFor(x => x.Name, new Person {Language = "English"});
        }

        [Fact]
        public void PersonValidation_Has_Error_When_Empty_Name()
        {
            var sut = new PersonValidation();
            sut.ShouldHaveValidationErrorFor(x => x.Name, new Person {Name = string.Empty, Language = "English" });
        }

        [Fact]
        public void PersonValidation_Has_Error_When_Whitespace_Name()
        {
            var sut = new PersonValidation();
            sut.ShouldHaveValidationErrorFor(x => x.Name, new Person { Name = " ", Language = "English" });
        }

        [Fact]
        public void PersonValidation_Has_Error_When_Null_Language()
        {
            var sut = new PersonValidation();
            sut.ShouldHaveValidationErrorFor(x => x.Language, new Person { Name = "Test" });
        }

        [Fact]
        public void PersonValidation_Has_Error_When_Empty_Language()
        {
            var sut = new PersonValidation();
            sut.ShouldHaveValidationErrorFor(x => x.Language, new Person { Language = string.Empty, Name = "Test" });
        }

        [Fact]
        public void PersonValidation_Has_Error_When_Whitespace_Language()
        {
            var sut = new PersonValidation();
            sut.ShouldHaveValidationErrorFor(x => x.Language, new Person { Language = " ", Name = "Test" });
        }
    }
}
