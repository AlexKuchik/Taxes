using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using Shouldly;
using Taxes.Application.Common.Interfaces;

namespace Taxes.Application.Queries.GetAppCities
{
    public class GetAllCitiesQueryHandlerTests
    {
        private GetAllCitiesQueryHandler _handler = default!;
        private Mock<ITaxRepository> _taxRepositoryMock = default!;

        [SetUp]
        public void Setup()
        {
            _taxRepositoryMock = new Mock<ITaxRepository>();
            _handler = new GetAllCitiesQueryHandler(_taxRepositoryMock.Object);
        }

        [Test]
        public async Task Handle_ShouldReturnCities()
        {
            var query = new GetAllCitiesQuery();
            var expectedResult = new List<string> { "Kaunas", "Minsk" };
            _taxRepositoryMock
                .Setup(x => x.GetAllCitiesAsync())
                .ReturnsAsync(expectedResult)
                .Verifiable();

            var actualResult = await _handler.Handle(query, It.IsAny<CancellationToken>());

            _taxRepositoryMock.VerifyAll();
            actualResult.Value.ShouldBeEquivalentTo(expectedResult);
        }
    }
}