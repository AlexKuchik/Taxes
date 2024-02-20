using System;
using ErrorOr;
using MediatR;

namespace Taxes.Application.Commands.AddTax
{
    public record AddTaxCommand : IRequest<ErrorOr<Guid>>
    {
        public string City { get; init; } = string.Empty;

        public DateTime StartDate { get; init; }

        public int Category { get; init; }

        public decimal Rate { get; init; }
    }
}