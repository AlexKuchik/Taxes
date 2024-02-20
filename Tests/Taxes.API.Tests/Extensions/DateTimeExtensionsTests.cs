using System;
using NUnit.Framework;
using Shouldly;
using Taxes.Constants;
using Taxes.Extensions;

namespace Taxes.API.Tests.Extensions
{
    public class DateTimeExtensionsTests
    {
        [Test]
        public void GetDateRange_WhenEndDateProvided_ShouldReturnCorrectResult()
        {
            var startDate = new DateTime(2024, 01, 01);
            var endDate = new DateTime(2024, 12, 31);
            var expectedResult = $"{startDate.ToString(Formats.Date)} - {endDate.ToString(Formats.Date)}";

            var actualResult = startDate.GetDateRange(endDate);

            actualResult.ShouldBeEquivalentTo(expectedResult);
        }

        [Test]
        public void GetDateRange_WhenEndDateNotProvided_ShouldReturnCorrectResult()
        {
            var startDate = new DateTime(2024, 01, 01);
            var expectedResult = startDate.ToString(Formats.Date);

            var actualResult = startDate.GetDateRange(null);

            actualResult.ShouldBeEquivalentTo(expectedResult);
        }
    }
}