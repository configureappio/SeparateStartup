using FluentValidation;

namespace ConfigureAppIo.Demos.SayHello.Domain
{
    public class PersonValidation : AbstractValidator<Person>
    {
        public PersonValidation()
        {
            RuleFor(p => p.Name).NotEmpty().WithMessage("Name is required."); 
            RuleFor(p => p.Language).NotEmpty().WithMessage("Language is required.");
        }
    }
}
