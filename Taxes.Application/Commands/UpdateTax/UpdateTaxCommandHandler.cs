using System;
using System.Threading;
using System.Threading.Tasks;
using ErrorOr;
using MediatR;
using Taxes.Application.Common.Interfaces;
using Taxes.Domain.Models;

namespace Taxes.Application.Commands.UpdateTax
{
    public class UpdateTaxCommandHandler : IRequestHandler<UpdateTaxCommand, ErrorOr<Updated>>
    {
        private readonly IDateValidationService _dateValidationService;
        private readonly ITaxRepository _taxRepository;

        public UpdateTaxCommandHandler(IDateValidationService dateValidationService, ITaxRepository taxRepository)
        {
            _dateValidationService = dateValidationService;
            _taxRepository = taxRepository;
        }

        public async Task<ErrorOr<Updated>> Handle(UpdateTaxCommand command, CancellationToken cancellationToken)
        {
            var existingTax = await _taxRepository.GetAsync(command.Id);
            if (existingTax == null)
            {
                return Error.NotFound(description: $"Tax record with the provided Id({command.Id}) not found");
            }

            if (command.Date.HasValue &&
                !_dateValidationService.IsValidDate((DateTime)command.Date, existingTax.Category))
            {
                return Error.Validation(description: $"Invalid {nameof(command.Date)}({command.Date})");
            }

            UpdateTaxProperties(existingTax, command.Date, command.Rate);

            await _taxRepository.UpdateAsync(existingTax);

            return Result.Updated;
        }

        private static void UpdateTaxProperties(Tax existingTax, DateTime? date, decimal? rate)
        {
            if (date.HasValue)
            {
                existingTax.StartDate = date.Value;
            }

            if (rate.HasValue)
            {
                existingTax.Rate = rate.Value;
            }
        }
    }
}