using System;

namespace Taxes.Application.Models
{
    public record TaxDto
    {
        public DateTime StartDate { get; init; }
        public DateTime? EndDate { get; init; }
        public decimal Rate { get; init; }
    }
}