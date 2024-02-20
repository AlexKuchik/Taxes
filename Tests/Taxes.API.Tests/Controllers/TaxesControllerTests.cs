using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using Shouldly;
using Taxes.Application.Commands.AddTax;
using Taxes.Application.Commands.DeleteTax;
using Taxes.Application.Commands.UpdateTax;
using Taxes.Application.Models;
using Taxes.Application.Queries.GetAllSchedule;
using Taxes.Application.Queries.GetAppCities;
using Taxes.Application.Queries.GetTaxRateOnDate;
using Taxes.Contracts.Request;
using Taxes.Controllers;
using Taxes.Mapper;

namespace Taxes.API.Tests.Controllers
{
    public class TaxesControllerTests
    {
        private readonly Error _error = Error.Validation("test");
        private TaxesController _controller = default!;
        private Mock<IMediator> _mediatorMock = default!;

        [SetUp]
        public void Setup()
        {
            _mediatorMock = new Mock<IMediator>();
            var config = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfile>());
            var mapper = config.CreateMapper();
            _controller = new TaxesController(_mediatorMock.Object, mapper);
        }

        [Test]
        public async Task Add_WhenNoErrors_ShouldReturnOk()
        {
            var request = new AddTaxRequest
            {
                City = "Kaunas",
                StartDate = new DateTime(2024, 01, 01),
                Rate = 10,
                Category = 1
            };
            _mediatorMock
                .Setup(x => x.Send(It.IsAny<AddTaxCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ErrorOr<Guid>())
                .Verifiable();

            var result = await _controller.Add(request);

            _mediatorMock.VerifyAll();
            result.ShouldBeAssignableTo<OkObjectResult>();
        }

        [Test]
        public async Task Add_WhenErrors_ShouldNotReturnOk()
        {
            var request = new AddTaxRequest
            {
                City = "Kaunas",
                StartDate = new DateTime(2024, 01, 01),
                Rate = 10,
                Category = 1
            };
            _mediatorMock
                .Setup(x => x.Send(It.IsAny<AddTaxCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(_error)
                .Verifiable();

            var result = await _controller.Add(request);

            _mediatorMock.VerifyAll();
            result.ShouldNotBeAssignableTo<OkObjectResult>();
        }

        [Test]
        public async Task Update_WhenNoErrors_ShouldReturnOk()
        {
            var request = new UpdateTaxRequest
            {
                Date = new DateTime(2024, 01, 01),
                Rate = 10
            };
            _mediatorMock
                .Setup(x => x.Send(It.IsAny<UpdateTaxCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ErrorOr<Updated>())
                .Verifiable();

            var result = await _controller.Update(Guid.NewGuid(), request);

            _mediatorMock.VerifyAll();
            result.ShouldBeAssignableTo<OkResult>();
        }

        [Test]
        public async Task Update_WhenErrors_ShouldNotReturnOk()
        {
            var request = new UpdateTaxRequest
            {
                Date = new DateTime(2024, 01, 01),
                Rate = 10
            };
            _mediatorMock
                .Setup(x => x.Send(It.IsAny<UpdateTaxCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(_error)
                .Verifiable();

            var result = await _controller.Update(Guid.NewGuid(), request);

            _mediatorMock.VerifyAll();
            result.ShouldNotBeAssignableTo<OkResult>();
        }

        [Test]
        public async Task Delete_WhenNoErrors_ShouldReturnNoContent()
        {
            _mediatorMock
                .Setup(x => x.Send(It.IsAny<DeleteTaxCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ErrorOr<Deleted>())
                .Verifiable();

            var result = await _controller.Delete(Guid.NewGuid());

            _mediatorMock.VerifyAll();
            result.ShouldBeAssignableTo<NoContentResult>();
        }

        [Test]
        public async Task Update_WhenErrors_ShouldNotReturnNoContent()
        {
            _mediatorMock
                .Setup(x => x.Send(It.IsAny<DeleteTaxCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(_error)
                .Verifiable();

            var result = await _controller.Delete(Guid.NewGuid());

            _mediatorMock.VerifyAll();
            result.ShouldNotBeAssignableTo<NoContentResult>();
        }

        [Test]
        public async Task GetCities_WhenNoErrorsAndCitiesExist_ShouldReturnOk()
        {
            _mediatorMock
                .Setup(x => x.Send(It.IsAny<GetAllCitiesQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(ErrorOrFactory.From(new List<string> { "Kaunas" }))
                .Verifiable();

            var result = await _controller.GetCities();

            _mediatorMock.VerifyAll();
            result.ShouldBeAssignableTo<OkObjectResult>();
        }

        [Test]
        public async Task GetCities_WhenNoErrorsAndNoCities_ShouldReturnNoContent()
        {
            _mediatorMock
                .Setup(x => x.Send(It.IsAny<GetAllCitiesQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(ErrorOrFactory.From(new List<string>()))
                .Verifiable();

            var result = await _controller.GetCities();

            _mediatorMock.VerifyAll();
            result.ShouldBeAssignableTo<NoContentResult>();
        }

        [Test]
        public async Task GetCities_WhenErrors_ShouldNotReturnNoContentOrOk()
        {
            _mediatorMock
                .Setup(x => x.Send(It.IsAny<GetAllCitiesQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(_error)
                .Verifiable();

            var result = await _controller.GetCities();

            _mediatorMock.VerifyAll();
            result.ShouldNotBeAssignableTo<NoContentResult>();
            result.ShouldNotBeAssignableTo<OkObjectResult>();
        }

        [Test]
        public async Task GetCityScheduleOfTaxes_WhenNoErrors_ShouldReturnOk()
        {
            _mediatorMock
                .Setup(x => x.Send(It.IsAny<GetScheduleOfTaxesQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(ErrorOrFactory.From(new List<TaxDto>()))
                .Verifiable();

            var result = await _controller.GetCityScheduleOfTaxes(It.IsAny<string>());

            _mediatorMock.VerifyAll();
            result.ShouldBeAssignableTo<OkObjectResult>();
        }

        [Test]
        public async Task GetCityScheduleOfTaxes_WhenErrors_ShouldNotReturnOk()
        {
            _mediatorMock
                .Setup(x => x.Send(It.IsAny<GetScheduleOfTaxesQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(_error)
                .Verifiable();

            var result = await _controller.GetCityScheduleOfTaxes(It.IsAny<string>());

            _mediatorMock.VerifyAll();
            result.ShouldNotBeAssignableTo<OkObjectResult>();
        }

        [Test]
        public async Task GetCityTaxRateOnDate_WhenNoErrors_ShouldReturnOk()
        {
            _mediatorMock
                .Setup(x => x.Send(It.IsAny<GetTaxRateOnDateQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(ErrorOrFactory.From<decimal>(10))
                .Verifiable();

            var result = await _controller.GetCityTaxRateOnDate(It.IsAny<string>(), It.IsAny<DateTime>());

            _mediatorMock.VerifyAll();
            result.ShouldBeAssignableTo<OkObjectResult>();
        }

        [Test]
        public async Task GetCityTaxRateOnDate_WhenErrors_ShouldNotReturnOk()
        {
            _mediatorMock
                .Setup(x => x.Send(It.IsAny<GetTaxRateOnDateQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(_error)
                .Verifiable();

            var result = await _controller.GetCityTaxRateOnDate(It.IsAny<string>(), It.IsAny<DateTime>());

            _mediatorMock.VerifyAll();
            result.ShouldNotBeAssignableTo<OkObjectResult>();
        }
    }
}