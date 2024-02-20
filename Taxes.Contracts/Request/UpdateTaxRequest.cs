using System;

namespace Taxes.Contracts.Request
{
    public record UpdateTaxRequest
    {
        public DateTime? Date { get; init; }

        public decimal? Rate { get; init; }
    }
}