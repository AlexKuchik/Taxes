using System;
using NUnit.Framework;
using Shouldly;
using Taxes.Contracts.Request;
using Taxes.Validators;

namespace Taxes.API.Tests.Validators
{
    public class UpdateTaxRequestValidatorTests
    {
        private UpdateTaxRequestValidator _validator = default!;

        [SetUp]
        public void Setup()
        {
            _validator = new UpdateTaxRequestValidator();
        }

        [Test]
        public void UpdateTaxRequestValidator_WhenRateAndDateNotProvided_ShouldReturnInvalidResult()
        {
            var request = new UpdateTaxRequest();

            var result = _validator.Validate(request);

            result.IsValid.ShouldBeFalse();
        }

        [Test]
        public void UpdateTaxRequestValidator_WhenAllRequiredParamsProvided_ShouldReturnValidResult()
        {
            var request = new UpdateTaxRequest { Rate = 10, Date = new DateTime(2024, 01, 01) };

            var result = _validator.Validate(request);

            result.IsValid.ShouldBeTrue();
        }
    }
}