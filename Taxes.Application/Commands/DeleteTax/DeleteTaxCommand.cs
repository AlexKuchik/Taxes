using System;
using ErrorOr;
using MediatR;

namespace Taxes.Application.Commands.DeleteTax
{
    public record DeleteTaxCommand(Guid Id) : IRequest<ErrorOr<Deleted>>;
}