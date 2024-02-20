using System;

namespace Taxes.Contracts.Request
{
    public record AddTaxRequest
    {
        public string City { get; init; } = string.Empty;

        public DateTime StartDate { get; init; }

        public int Category { get; init; }

        public decimal Rate { get; init; }
    }
}