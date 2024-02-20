using System;
using ErrorOr;
using MediatR;

namespace Taxes.Application.Commands.UpdateTax
{
    public record UpdateTaxCommand(Guid Id, DateTime? Date, decimal? Rate) : IRequest<ErrorOr<Updated>>;
}