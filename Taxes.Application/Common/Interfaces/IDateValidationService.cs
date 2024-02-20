using System;
using Takes.Domain.Enums;

namespace Taxes.Application.Common.Interfaces
{
    public interface IDateValidationService
    {
        bool IsValidDate(DateTime startDate, TaxCategory category);
    }
}