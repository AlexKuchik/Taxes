using FluentValidation;
using Taxes.Contracts.Request;

namespace Taxes.Validators
{
    public class UpdateTaxRequestValidator : AbstractValidator<UpdateTaxRequest>
    {
        public UpdateTaxRequestValidator()
        {
            RuleFor(x => x)
                .Custom((request, context) =>
                {
                    if (!request.Rate.HasValue && !request.Date.HasValue)
                    {
                        context.AddFailure($"{nameof(request.Rate)} or {nameof(request.Date)} should not be empty");
                    }
                });
        }
    }
}