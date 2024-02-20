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

namespace Taxes.Application.Queries.GetTaxRateOnDate
{
    public class GetTaxRateOnDateQueryHandlerTests
    {
        private static object[] _testCases =
        {
            new object[]
            {
                new DateTime(2024, 01, 01),
                (decimal)3.0
            },
            new object[]
            {
                new DateTime(2024, 01, 10),
                (decimal)3.0
            },
            new object[]
            {
                new DateTime(2024, 01, 13),
                (decimal)5.0
            },
            new object[]
            {
                new DateTime(2024, 01, 20),
                (decimal)3.0
            },
            new object[]
            {
                new DateTime(2024, 03, 10),
                (decimal)6.0
            },
            new object[]
            {
                new DateTime(2024, 02, 10),
                (decimal)4.0
            }
        };

        private GetTaxRateOnDateQueryHandler _handler = default!;
        private Mock<ITaxRepository> _taxRepositoryMock = default!;

        [SetUp]
        public void Setup()
        {
            _taxRepositoryMock = new Mock<ITaxRepository>();
            var config = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfile>());
            var mapper = config.CreateMapper();
            _handler = new GetTaxRateOnDateQueryHandler(_taxRepositoryMock.Object, mapper);
        }

        [Test]
        public async Task Handle_WhenTaxesNotFound_ShouldReturnNotFoundError()
        {
            var query = new GetTaxRateOnDateQuery(It.IsAny<string>(), It.IsAny<DateTime>());
            _taxRepositoryMock.Setup(x => x.GetAllTaxesByCityAsync(query.City)).ReturnsAsync([]);

            var result = await _handler.Handle(query, It.IsAny<CancellationToken>());

            result.IsError.ShouldBeTrue();
            result.Errors.ShouldBeEquivalentTo(
                new List<Error>
                    { Error.NotFound(description: $"Tax records with the provided City({query.City}) not found") });
        }

        [TestCaseSource(nameof(_testCases))]
        public async Task Handle_WhenTaxesFound_ShouldReturnCorrectResult(DateTime date, decimal expectedResult)
        {
            var taxes = InitialDataHelper.GetInitTaxes();
            var query = new GetTaxRateOnDateQuery(It.IsAny<string>(), date);
            _taxRepositoryMock.Setup(x => x.GetAllTaxesByCityAsync(query.City)).ReturnsAsync(taxes);

            var actualResult = await _handler.Handle(query, It.IsAny<CancellationToken>());

            actualResult.Value.ShouldBeEquivalentTo(expectedResult);
        }
    }
}