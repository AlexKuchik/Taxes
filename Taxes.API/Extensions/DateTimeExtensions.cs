using System;
using Taxes.Constants;

namespace Taxes.Extensions
{
    public static class DateTimeExtensions
    {
        public static string GetDateRange(this DateTime startDate, DateTime? endDate)
        {
            return endDate.HasValue ?
                $"{startDate.ToString(Formats.Date)} - {endDate?.ToString(Formats.Date)}" :
                startDate.ToString(Formats.Date);
        }
    }
}