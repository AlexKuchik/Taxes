using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Taxes.Application.Commands.AddTax;
using Taxes.Application.Commands.DeleteTax;
using Taxes.Application.Commands.UpdateTax;
using Taxes.Application.Queries.GetAllSchedule;
using Taxes.Application.Queries.GetAppCities;
using Taxes.Application.Queries.GetTaxRateOnDate;
using Taxes.Constants;
using Taxes.Contracts.Request;
using Taxes.Contracts.Response;
using Taxes.Infrastructure.Security.Constants;

namespace Taxes.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/taxes")]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    public class TaxesController : ApiController
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public TaxesController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpPost]
        [Authorize(Policy = Policies.AdminOnly)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        public async Task<ActionResult> Add([FromBody] AddTaxRequest request)
        {
            var command = _mapper.Map<AddTaxCommand>(request);
            var result = await _mediator.Send(command);

            return result.Match(
                _ => Ok(result.Value),
                Problem);
        }


        [HttpPatch("{id}")]
        [Authorize(Policy = Policies.AdminOnly)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        public async Task<ActionResult> Update(Guid id, [FromBody] UpdateTaxRequest request)
        {
            var command = new UpdateTaxCommand(id, request.Date, request.Rate);
            var result = await _mediator.Send(command);

            return result.Match(
                _ => Ok(),
                Problem);
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = Policies.AdminOnly)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        public async Task<ActionResult> Delete(Guid id)
        {
            var command = new DeleteTaxCommand(id);
            var result = await _mediator.Send(command);

            return result.Match(
                _ => NoContent(),
                Problem);
        }

        [HttpGet("cities")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<ActionResult> GetCities()
        {
            var query = new GetAllCitiesQuery();
            var result = await _mediator.Send(query);

            return result.Match(
                cities => cities.Any() ?
                    Ok(cities) :
                    NoContent(),
                Problem);
        }

        [HttpGet("cities/{city}/schedules")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> GetCityScheduleOfTaxes(string city)
        {
            var query = new GetScheduleOfTaxesQuery(city);
            var result = await _mediator.Send(query);

            return result.Match(
                schedules => Ok(_mapper.Map<List<TaxScheduleResponse>>(schedules)),
                Problem);
        }

        [HttpGet("cities/{city}/dates/{date}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> GetCityTaxRateOnDate(string city, DateTime date)
        {
            var query = new GetTaxRateOnDateQuery(city, date);
            var result = await _mediator.Send(query);

            return result.Match(
                _ => Ok(result.Value.ToString(Formats.Decimal)),
                Problem);
        }
    }
}