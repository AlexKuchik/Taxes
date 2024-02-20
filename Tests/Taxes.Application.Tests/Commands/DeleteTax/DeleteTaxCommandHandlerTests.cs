using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ErrorOr;
using Moq;
using NUnit.Framework;
using Shouldly;
using Taxes.Application.Common.Interfaces;
using Taxes.Domain.Models;

namespace Taxes.Application.Commands.DeleteTax
{
    public class DeleteTaxCommandHandlerTests
    {
        private DeleteTaxCommandHandler _handler = default!;
        private Mock<ITaxRepository> _taxRepositoryMock = default!;

        [SetUp]
        public void Setup()
        {
            _taxRepositoryMock = new Mock<ITaxRepository>();
            _handler = new DeleteTaxCommandHandler(_taxRepositoryMock.Object);
        }

        [Test]
        public async Task Handle_WhenTaxRecordNotFound_ShouldReturnNotFoundError()
        {
            var id = Guid.NewGuid();
            _taxRepositoryMock.Setup(x => x.GetAsync(id)).ReturnsAsync((Tax?)null);
            var command = new DeleteTaxCommand(id);

            var result = await _handler.Handle(command, It.IsAny<CancellationToken>());

            result.IsError.ShouldBeTrue();
            result.Errors.ShouldBeEquivalentTo(
                new List<Error> { Error.NotFound(description: $"Tax record with the provided Id({id}) not found") });
            _taxRepositoryMock.Verify(repo => repo.DeleteAsync(It.IsAny<Tax>()), Times.Never);
        }

        [Test]
        public async Task Handle_WhenTaxRecordFound_ShouldReturnDeleted()
        {
            var id = Guid.NewGuid();
            var existingRecord = new Tax();
            _taxRepositoryMock.Setup(x => x.GetAsync(id)).ReturnsAsync(existingRecord);
            var command = new DeleteTaxCommand(id);

            var result = await _handler.Handle(command, It.IsAny<CancellationToken>());

            _taxRepositoryMock.Verify(repo => repo.DeleteAsync(existingRecord), Times.Once);
            result.Value.ShouldBeEquivalentTo(Result.Deleted);
        }
    }
}