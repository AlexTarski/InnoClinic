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
                .MaximumLength(200).WithMessage("City can be maximum {MaxLength} symbols long, but you entered {TotalLength}.");

            RuleFor(x => x.Street)
                .NotEmpty().WithMessage("Please, enter the office’s street")
                .MaximumLength(100).WithMessage("Street can be maximum {MaxLength} symbols long, but you entered {TotalLength}.");

            RuleFor(x => x.HouseNumber)
                .NotEmpty().WithMessage("Please, enter the office’s house number")
                .MaximumLength(10).WithMessage("House number can be maximum {MaxLength} symbols long, but you entered {TotalLength}.");

            RuleFor(x => x.OfficeNumber)
                .NotEmpty().WithMessage("Please, enter the office’s number")
                .MaximumLength(10).WithMessage("Office number can be maximum {MaxLength} symbols long, but you entered {TotalLength}.");
        }
    }
}