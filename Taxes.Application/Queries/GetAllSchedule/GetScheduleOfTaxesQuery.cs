using System.Collections.Generic;
using ErrorOr;
using MediatR;
using Taxes.Application.Models;

namespace Taxes.Application.Queries.GetAllSchedule
{
    public record GetScheduleOfTaxesQuery(string City) : IRequest<ErrorOr<List<TaxDto>>>;
}