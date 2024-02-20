using System.Threading;
using System.Threading.Tasks;
using ErrorOr;
using MediatR;
using Taxes.Application.Common.Interfaces;

namespace Taxes.Application.Commands.DeleteTax
{
    public class DeleteTaxCommandHandler : IRequestHandler<DeleteTaxCommand, ErrorOr<Deleted>>
    {
        private readonly ITaxRepository _taxRepository;

        public DeleteTaxCommandHandler(ITaxRepository taxRepository)
        {
            _taxRepository = taxRepository;
        }

        public async Task<ErrorOr<Deleted>> Handle(DeleteTaxCommand command, CancellationToken cancellationToken)
        {
            var existingTax = await _taxRepository.GetAsync(command.Id);
            if (existingTax == null)
            {
                return Error.NotFound(description: $"Tax record with the provided Id({command.Id}) not found");
            }

            await _taxRepository.DeleteAsync(existingTax);

            return Result.Deleted;
        }
    }
}