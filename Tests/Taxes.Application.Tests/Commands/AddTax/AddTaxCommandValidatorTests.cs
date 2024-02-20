using System;
using Moq;
using NUnit.Framework;
using Shouldly;
using Takes.Domain.Enums;
using Taxes.Application.Common.Interfaces;

namespace Taxes.Application.Commands.AddTax
{
    public class AddTaxCommandValidatorTests
    {
        private Mock<IDateValidationService> _dateValidationServiceMock = default!;
        private AddTaxCommandValidator _validator = default!;

        [SetUp]
        public void Setup()
        {
            _dateValidationServiceMock = new Mock<IDateValidationService>();
            _validator = new AddTaxCommandValidator(_dateValidationServiceMock.Object);
        }

        [Test]
        public void AddTaxCommandValidator_WhenCategoryInvalid_ShouldReturnFalse()
        {
            var command = new AddTaxCommand
            {
                City = "Kaunas",
                StartDate = new DateTime(2024, 01, 01),
                Category = 100,
                Rate = 10
            };

            var result = _validator.Validate(command);

            result.IsValid.ShouldBeFalse();
        }

        [Test]
        public void AddTaxCommandValidator_WhenDateInvalid_ShouldReturnFalse()
        {
            var command = new AddTaxCommand
            {
                City = "Kaunas",
                StartDate = new DateTime(2024, 01, 02),
                Category = 1,
                Rate = 10
            };
            _dateValidationServiceMock.Setup(x => x.IsValidDate(
                    command.StartDate,
                    (TaxCategory)command.Category))
                .Returns(false);

            var result = _validator.Validate(command);

            result.IsValid.ShouldBeFalse();
        }

        [Test]
        public void AddTaxCommandValidator_WhenParamsValid_ShouldReturnTrue()
        {
            var command = new AddTaxCommand
            {
                City = "Kaunas",
                StartDate = new DateTime(2024, 01, 01),
                Category = 1,
                Rate = 10
            };
            _dateValidationServiceMock.Setup(x => x.IsValidDate(
                    command.StartDate,
                    (TaxCategory)command.Category))
                .Returns(true);

            var result = _validator.Validate(command);

            result.IsValid.ShouldBeTrue();
        }
    }
}