using System;
using NUnit.Framework;
using Shouldly;
using Taxes.Contracts.Request;
using Taxes.Validators;

namespace Taxes.API.Tests.Validators
{
    public class AddTaxRequestValidatorTests
    {
        private AddTaxRequestValidator _validator = default!;

        [SetUp]
        public void Setup()
        {
            _validator = new AddTaxRequestValidator();
        }

        [Test]
        public void AddTaxRequestValidator_WhenCityNotProvided_ShouldReturnInvalidResult()
        {
            var request = new AddTaxRequest
            {
                StartDate = new DateTime(2024, 01, 01),
                Rate = 10,
                Category = 1
            };

            var result = _validator.Validate(request);

            result.IsValid.ShouldBeFalse();
        }

        [Test]
        public void AddTaxRequestValidator_WhenStartDateNotProvided_ShouldReturnInvalidResult()
        {
            var request = new AddTaxRequest
            {
                City = "Kaunas",
                Rate = 10,
                Category = 1
            };

            var result = _validator.Validate(request);

            result.IsValid.ShouldBeFalse();
        }

        [Test]
        public void AddTaxRequestValidator_WhenRateNotProvided_ShouldReturnInvalidResult()
        {
            var request = new AddTaxRequest
            {
                City = "Kaunas",
                StartDate = new DateTime(2024, 01, 01),
                Category = 1
            };

            var result = _validator.Validate(request);

            result.IsValid.ShouldBeFalse();
        }

        [Test]
        public void AddTaxRequestValidator_WhenCategoryNotProvided_ShouldReturnInvalidResult()
        {
            var request = new AddTaxRequest
            {
                City = "Kaunas",
                StartDate = new DateTime(2024, 01, 01),
                Rate = 10
            };

            var result = _validator.Validate(request);

            result.IsValid.ShouldBeFalse();
        }

        [Test]
        public void AddTaxRequestValidator_WhenAllRequiredParamsProvided_ShouldReturnValidResult()
        {
            var request = new AddTaxRequest
            {
                City = "Kaunas",
                StartDate = new DateTime(2024, 01, 01),
                Rate = 10,
                Category = 1
            };

            var result = _validator.Validate(request);

            result.IsValid.ShouldBeTrue();
        }
    }
}