using System;
using Takes.Domain.Enums;

namespace Taxes.Application.Extensions
{
    public static class DateTimeExtensions
    {
        public static DateTime? GetEndDate(this DateTime date, TaxCategory category)
        {
            return category switch
            {
                TaxCategory.Year => date.AddYears(1).AddDays(-1),
                TaxCategory.Month => date.AddMonths(1).AddDays(-1),
                TaxCategory.Week => date.AddDays(7).AddDays(-1),
                TaxCategory.Day => null,
                _ => throw new ArgumentOutOfRangeException(nameof(category),
                    $"Unexpected value for {nameof(TaxCategory)}: {category}")
            };
        }
    }
}