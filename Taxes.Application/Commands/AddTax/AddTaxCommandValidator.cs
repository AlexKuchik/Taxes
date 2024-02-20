using System;
using FluentValidation;
using Takes.Domain.Enums;
using Taxes.Application.Common.Interfaces;

namespace Taxes.Application.Commands.AddTax
{
    public class AddTaxCommandValidator : AbstractValidator<AddTaxCommand>
    {
        public AddTaxCommandValidator(IDateValidationService dateValidationService)
        {
            RuleFor(x => x)
                .Custom((command, context) =>
                {
                    if (!Enum.IsDefined(typeof(TaxCategory), command.Category))
                    {
                        context.AddFailure($"Invalid {nameof(command.Category)}({command.Category})");
                        return;
                    }

                    if (!dateValidationService.IsValidDate(command.StartDate, (TaxCategory)command.Category))
                    {
                        context.AddFailure($"Invalid {nameof(command.StartDate)}({command.StartDate})");
                    }
                });
        }
    }
}