using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Moq;
using NUnit.Framework;
using Shouldly;
using Takes.Domain.Models;
using Taxes.Application.Common.Interfaces;
using Taxes.Application.Mapper;

namespace Taxes.Application.Commands.AddTax
{
    public class AddTaxCommandHandlerTests
    {
        private AddTaxCommandHandler _handler = default!;
        private Mock<ITaxRepository> _taxRepositoryMock = default!;

        [SetUp]
        public void Setup()
        {
            _taxRepositoryMock = new Mock<ITaxRepository>();
            var config = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfile>());
            var mapper = config.CreateMapper();
            _handler = new AddTaxCommandHandler(_taxRepositoryMock.Object, mapper);
        }

        [Test]
        public async Task Handle_ShouldAddTaxRecord()
        {
            var id = Guid.NewGuid();
            _taxRepositoryMock.Setup(x => x.AddAsync(It.IsAny<Tax>()))
                .ReturnsAsync(id)
                .Verifiable();
            var command = new AddTaxCommand
            {
                City = "Kaunas",
                StartDate = new DateTime(2024, 01, 01),
                Category = 1,
                Rate = 10
            };

            var result = await _handler.Handle(command, It.IsAny<CancellationToken>());

            _taxRepositoryMock.VerifyAll();
            result.Value.ShouldBeEquivalentTo(id);
        }
    }
}