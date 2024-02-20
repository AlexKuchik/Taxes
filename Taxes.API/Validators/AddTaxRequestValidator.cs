using FluentValidation;
using Taxes.Contracts.Request;

namespace Taxes.Validators
{
    public class AddTaxRequestValidator : AbstractValidator<AddTaxRequest>
    {
        public AddTaxRequestValidator()
        {
            RuleFor(x => x.City).NotEmpty();
            RuleFor(x => x.StartDate).NotEmpty();
            RuleFor(x => x.Category).NotEmpty();
            RuleFor(x => x.Rate).NotEmpty();
        }
    }
}