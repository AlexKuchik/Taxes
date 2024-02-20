using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using ErrorOr;
using Moq;
using NUnit.Framework;
using Shouldly;
using Taxes.Application.Common.Interfaces;
using Taxes.Application.Helpers;
using Taxes.Application.Mapper;
using Taxes.Application.Models;

namespace Taxes.Application.Queries.GetAllSchedule
{
    public class GetScheduleOfTaxesQueryHandlerTests
    {
        private GetScheduleOfTaxesQueryHandler _handler = default!;
        private Mock<ITaxRepository> _taxRepositoryMock = default!;

        [SetUp]
        public void Setup()
        {
            _taxRepositoryMock = new Mock<ITaxRepository>();
            var config = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfile>());
            var mapper = config.CreateMapper();
            _handler = new GetScheduleOfTaxesQueryHandler(_taxRepositoryMock.Object, mapper);
        }

        [Test]
        public async Task Handle_WhenTaxesNotFound_ShouldReturnNotFoundError()
        {
            var query = new GetScheduleOfTaxesQuery(It.IsAny<string>());
            _taxRepositoryMock.Setup(x => x.GetAllTaxesByCityAsync(query.City)).ReturnsAsync([]);

            var result = await _handler.Handle(query, It.IsAny<CancellationToken>());

            result.IsError.ShouldBeTrue();
            result.Errors.ShouldBeEquivalentTo(
                new List<Error>
                    { Error.NotFound(description: $"Tax records with the provided City({query.City}) not found") });
        }

        [Test]
        public async Task Handle_WhenTaxesFound_ShouldReturnOrderedCorrectResult()
        {
            var taxes = InitialDataHelper.GetInitTaxes();
            var query = new GetScheduleOfTaxesQuery(It.IsAny<string>());
            _taxRepositoryMock.Setup(x => x.GetAllTaxesByCityAsync(query.City)).ReturnsAsync(taxes);
            var expectedResult = GetExpectedTaxes();

            var actualResult = await _handler.Handle(query, It.IsAny<CancellationToken>());

            actualResult.Value.ShouldBeEquivalentTo(expectedResult);
        }

        private static List<TaxDto> GetExpectedTaxes()
        {
            return
            [
                new TaxDto
                {
                    StartDate = new DateTime(2024, 01, 01),
                    EndDate = new DateTime(2024, 12, 31),
                    Rate = (decimal)3.0
                },

                new TaxDto
                {
                    StartDate = new DateTime(2024, 01, 01),
                    EndDate = new DateTime(2024, 12, 31),
                    Rate = (decimal)2.0
                },

                new TaxDto
                {
                    StartDate = new DateTime(2024, 01, 01),
                    EndDate = new DateTime(2024, 12, 31),
                    Rate = (decimal)1.0
                },

                new TaxDto
                {
                    StartDate = new DateTime(2024, 02, 01),
                    EndDate = new DateTime(2024, 02, 29),
                    Rate = (decimal)4.0
                },

                new TaxDto
                {
                    StartDate = new DateTime(2024, 01, 12),
                    EndDate = new DateTime(2024, 01, 18),
                    Rate = (decimal)5.0
                },

                new TaxDto
                {
                    StartDate = new DateTime(2024, 03, 10),
                    EndDate = null,
                    Rate = (decimal)6.0
                }
            ];
        }
    }
}