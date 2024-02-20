using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ErrorOr;
using Moq;
using NUnit.Framework;
using Shouldly;
using Takes.Domain.Enums;
using Takes.Domain.Models;
using Taxes.Application.Common.Interfaces;

namespace Taxes.Application.Commands.UpdateTax
{
    public class UpdateTaxCommandHandlerTests
    {
        private Mock<IDateValidationService> _dateValidationServiceMock = default!;
        private UpdateTaxCommandHandler _handler = default!;
        private Mock<ITaxRepository> _taxRepositoryMock = default!;

        [SetUp]
        public void Setup()
        {
            _dateValidationServiceMock = new Mock<IDateValidationService>();
            _taxRepositoryMock = new Mock<ITaxRepository>();
            _handler = new UpdateTaxCommandHandler(_dateValidationServiceMock.Object, _taxRepositoryMock.Object);
        }

        [Test]
        public async Task Handle_WhenTaxRecordNotFound_ShouldReturnNotFoundError()
        {
            var id = Guid.NewGuid();
            _taxRepositoryMock.Setup(x => x.GetAsync(id)).ReturnsAsync((Tax?)null);
            var command = new UpdateTaxCommand(id, It.IsAny<DateTime>(), It.IsAny<decimal>());

            var result = await _handler.Handle(command, It.IsAny<CancellationToken>());

            result.IsError.ShouldBeTrue();
            result.Errors.ShouldBeEquivalentTo(
                new List<Error> { Error.NotFound(description: $"Tax record with the provided Id({id}) not found") });
            _taxRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<Tax>()), Times.Never);
        }

        [Test]
        public async Task Handle_WhenInvalidDateProvided_ShouldReturnValidationError()
        {
            var id = Guid.NewGuid();
            var existingTax = new Tax { Category = TaxCategory.Year };
            _taxRepositoryMock.Setup(x => x.GetAsync(id)).ReturnsAsync(existingTax);
            var date = new DateTime(2024, 01, 02);
            var command = new UpdateTaxCommand(id, date, It.IsAny<decimal>());
            _dateValidationServiceMock.Setup(x => x.IsValidDate((DateTime)command.Date!, existingTax.Category))
                .Returns(false);

            var result = await _handler.Handle(command, It.IsAny<CancellationToken>());

            result.IsError.ShouldBeTrue();
            result.Errors.ShouldBeEquivalentTo(
                new List<Error> { Error.Validation(description: $"Invalid {nameof(command.Date)}({command.Date})") });
            _taxRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<Tax>()), Times.Never);
        }

        [Test]
        public async Task Handle_WhenValidParamsProvided_ShouldUpdateTax()
        {
            var id = Guid.NewGuid();
            var existingTax = new Tax
            {
                Id = id,
                Category = TaxCategory.Year,
                StartDate = new DateTime(2023, 01, 01),
                Rate = (decimal)1.0
            };
            _taxRepositoryMock.Setup(x => x.GetAsync(id)).ReturnsAsync(existingTax);
            var date = new DateTime(2024, 01, 01);
            const decimal rate = (decimal)10.0;
            var command = new UpdateTaxCommand(id, date, rate);
            _dateValidationServiceMock.Setup(x => x.IsValidDate((DateTime)command.Date!, existingTax.Category))
                .Returns(true);
            var expectedTax = new Tax
            {
                Id = existingTax.Id,
                Category = existingTax.Category,
                StartDate = (DateTime)command.Date!,
                Rate = (decimal)command.Rate!
            };

            var result = await _handler.Handle(command, It.IsAny<CancellationToken>());

            _taxRepositoryMock.Verify(repo => repo.UpdateAsync(expectedTax), Times.Once);
            result.Value.ShouldBeEquivalentTo(Result.Updated);
        }
    }
}