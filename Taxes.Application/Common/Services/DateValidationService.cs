using System;
using Taxes.Application.Common.Interfaces;
using Taxes.Domain.Enums;

namespace Taxes.Application.Common.Services
{
    public class DateValidationService : IDateValidationService
    {
        public bool IsValidDate(DateTime startDate, TaxCategory category)
        {
            return category switch
            {
                TaxCategory.Year => IsFirstDayOfYear(startDate),
                TaxCategory.Month => IsFirstDayOfMonth(startDate),
                TaxCategory.Week => IsFirstDayOfWeek(startDate),
                TaxCategory.Day => true,
                _ => throw new ArgumentOutOfRangeException(nameof(category),
                    $"Unexpected value for {nameof(TaxCategory)}: {category}")
            };
        }

        private static bool IsFirstDayOfYear(DateTime date)
        {
            return date.Month == 1 && date.Day == 1;
        }

        private static bool IsFirstDayOfMonth(DateTime date)
        {
            return date.Day == 1;
        }

        private static bool IsFirstDayOfWeek(DateTime date)
        {
            return date.DayOfWeek == DayOfWeek.Monday; // Assuming Monday is the first day of the week
        }
    }
}