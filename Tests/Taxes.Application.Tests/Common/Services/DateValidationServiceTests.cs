using System;
using Moq;
using NUnit.Framework;
using Shouldly;
using Takes.Domain.Enums;

namespace Taxes.Application.Common.Services
{
    public class DateValidationServiceTests
    {
        private static object[] _testCases =
        [
            new object[]
            {
                new DateTime(2024, 01, 01),
                TaxCategory.Year,
                true
            },
            new object[]
            {
                new DateTime(2024, 02, 01),
                TaxCategory.Year,
                false
            },
            new object[]
            {
                new DateTime(2024, 01, 1),
                TaxCategory.Month,
                true
            },
            new object[]
            {
                new DateTime(2024, 01, 2),
                TaxCategory.Month,
                false
            },
            new object[]
            {
                new DateTime(2024, 02, 12),
                TaxCategory.Week,
                true
            },
            new object[]
            {
                new DateTime(2024, 01, 11),
                TaxCategory.Week,
                false
            },
            new object[]
            {
                new DateTime(2024, 01, 12),
                TaxCategory.Day,
                true
            },
            new object?[]
            {
                new DateTime(2024, 12, 12),
                TaxCategory.Day,
                true
            }
        ];

        private DateValidationService _service = default!;

        [SetUp]
        public void Setup()
        {
            _service = new DateValidationService();
        }

        [Test]
        public void IsValidDate_WhenUnexpectedCategory_ShouldThrowArgumentOutOfRangeException()
        {
            Assert.Throws(typeof(ArgumentOutOfRangeException),
                () => _service.IsValidDate(It.IsAny<DateTime>(), (TaxCategory)100));
        }

        [TestCaseSource(nameof(_testCases))]
        public void IsValidDate_ShouldReturnCorrectResult(DateTime startDate, TaxCategory category, bool expectedResult)
        {
            var actualResult = _service.IsValidDate(startDate, category);

            actualResult.ShouldBeEquivalentTo(expectedResult);
        }
    }
}