using System;
using System.Collections.Generic;
using Taxes.Domain.Enums;
using Taxes.Domain.Models;

namespace Taxes.Application.Helpers
{
    internal static class InitialDataHelper
    {
        internal static List<Tax> GetInitTaxes()
        {
            return
            [
                new Tax
                {
                    Category = TaxCategory.Year,
                    StartDate = new DateTime(2024, 01, 01),
                    CreatedAt = new DateTime(2024, 01, 01),
                    Rate = (decimal)1.0
                },

                new Tax
                {
                    Category = TaxCategory.Year,
                    StartDate = new DateTime(2024, 01, 01),
                    CreatedAt = new DateTime(2024, 01, 01),
                    ModifiedAt = new DateTime(2024, 02, 01),
                    Rate = (decimal)2.0
                },

                new Tax
                {
                    Category = TaxCategory.Year,
                    StartDate = new DateTime(2024, 01, 01),
                    CreatedAt = new DateTime(2024, 04, 01),
                    Rate = (decimal)3.0
                },

                new Tax
                {
                    Category = TaxCategory.Day,
                    StartDate = new DateTime(2024, 03, 10),
                    CreatedAt = new DateTime(2024, 04, 01),
                    Rate = (decimal)6.0
                },

                new Tax
                {
                    Category = TaxCategory.Month,
                    StartDate = new DateTime(2024, 02, 01),
                    CreatedAt = new DateTime(2024, 04, 01),
                    Rate = (decimal)4.0
                },

                new Tax
                {
                    Category = TaxCategory.Week,
                    StartDate = new DateTime(2024, 01, 12),
                    CreatedAt = new DateTime(2024, 04, 01),
                    Rate = (decimal)5.0
                }
            ];
        }
    }
}