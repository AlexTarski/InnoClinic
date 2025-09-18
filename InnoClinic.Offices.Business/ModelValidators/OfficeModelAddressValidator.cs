using FluentValidation;

using InnoClinic.Offices.Domain;

namespace InnoClinic.Offices.Business.ModelValidators
{
    public class OfficeModelAddressValidator : AbstractValidator<Address>
    {
        public OfficeModelAddressValidator()
        {
            RuleFor(x => x.City)
                .NotEmpty().WithMessage("Please, enter the office’s city")
                .MaximumLength(200);

            RuleFor(x => x.Street)
                .NotEmpty().WithMessage("Please, enter the office’s street")
                .MaximumLength(100);

            RuleFor(x => x.HouseNumber)
                .NotEmpty().WithMessage("Please, enter the office’s house number")
                .MaximumLength(10);

            RuleFor(x => x.OfficeNumber)
                .NotEmpty().WithMessage("Please, enter the office’s number")
                .MaximumLength(10);
        }
    }
}