using System;
using Microsoft.EntityFrameworkCore;
using Takes.Domain.Enums;
using Takes.Domain.Models;

namespace Taxes.Infrastructure.Extensions
{
    internal static class ModelBuilderExtensions
    {
        private const string InitCity = "Kaunas";

        public static void SeedTaxData(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Tax>()
                .HasData(
                    new Tax
                    {
                        Id = Guid.NewGuid(),
                        City = InitCity,
                        StartDate = new DateTime(2024, 1, 1),
                        Category = TaxCategory.Year,
                        Rate = (decimal)3.3,
                        CreatedAt = DateTime.UtcNow
                    },
                    new Tax
                    {
                        Id = Guid.NewGuid(),
                        City = InitCity,
                        StartDate = new DateTime(2024, 6, 1),
                        Category = TaxCategory.Month,
                        Rate = 5,
                        CreatedAt = DateTime.UtcNow
                    },
                    new Tax
                    {
                        Id = Guid.NewGuid(),
                        City = InitCity,
                        StartDate = new DateTime(2024, 7, 1),
                        Category = TaxCategory.Month,
                        Rate = 4,
                        CreatedAt = DateTime.UtcNow
                    },
                    new Tax
                    {
                        Id = Guid.NewGuid(),
                        City = InitCity,
                        StartDate = new DateTime(2024, 8, 1),
                        Category = TaxCategory.Month,
                        Rate = 6,
                        CreatedAt = DateTime.UtcNow
                    },
                    new Tax
                    {
                        Id = Guid.NewGuid(),
                        City = InitCity,
                        StartDate = new DateTime(2024, 2, 5),
                        Category = TaxCategory.Week,
                        Rate = (decimal)2.5,
                        CreatedAt = DateTime.UtcNow
                    },
                    new Tax
                    {
                        Id = Guid.NewGuid(),
                        City = InitCity,
                        StartDate = new DateTime(2024, 3, 4),
                        Category = TaxCategory.Week,
                        Rate = (decimal)2.5,
                        CreatedAt = DateTime.UtcNow
                    },
                    new Tax
                    {
                        Id = Guid.NewGuid(),
                        City = InitCity,
                        StartDate = new DateTime(2024, 6, 1),
                        Category = TaxCategory.Day,
                        Rate = (decimal)1.5,
                        CreatedAt = DateTime.UtcNow
                    },
                    new Tax
                    {
                        Id = Guid.NewGuid(),
                        City = InitCity,
                        StartDate = new DateTime(2024, 10, 23),
                        Category = TaxCategory.Day,
                        Rate = (decimal)1.2,
                        CreatedAt = DateTime.UtcNow
                    });
        }
    }
}