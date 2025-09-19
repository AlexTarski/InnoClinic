using FluentValidation;

using InnoClinic.Offices.Business.Models;

namespace InnoClinic.Offices.Business.ModelValidators
{
    public class OfficeModelValidator : AbstractValidator<OfficeModel>
    {
        public OfficeModelValidator()
        {
            RuleFor(x => x.Address)
                .SetValidator(new OfficeModelAddressValidator());
        }
    }
}