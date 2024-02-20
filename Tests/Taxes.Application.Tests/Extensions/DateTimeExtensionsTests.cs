using System;
using NUnit.Framework;
using Shouldly;
using Takes.Domain.Enums;

namespace Taxes.Application.Extensions
{
    public class DateTimeExtensionsTests
    {
        private static object[] _testCases =
        [
            new object[]
            {
                new DateTime(2024, 01, 01),
                TaxCategory.Year,
                new DateTime(2024, 12, 31)
            },
            new object[]
            {
                new DateTime(2024, 01, 1),
                TaxCategory.Month,
                new DateTime(2024, 01, 31)
            },
            new object[]
            {
                new DateTime(2024, 01, 12),
                TaxCategory.Week,
                new DateTime(2024, 01, 18)
            },
            new object?[]
            {
                new DateTime(2024, 01, 12),
                TaxCategory.Day,
                null
            }
        ];

        [Test]
        public void GetEndDate_WhenUnexpectedCategory_ShouldThrowArgumentOutOfRangeException()
        {
            Assert.Throws(typeof(ArgumentOutOfRangeException),
                () => new DateTime(2024, 01, 01).GetEndDate((TaxCategory)100));
        }

        [TestCaseSource(nameof(_testCases))]
        public void GetEndDate_ShouldReturnCorrectEndDate(
            DateTime startDate,
            TaxCategory category,
            DateTime? expectedEndDate)
        {
            var actualEndDate = startDate.GetEndDate(category);

            actualEndDate.ShouldBeEquivalentTo(expectedEndDate);
        }
    }
}