using System;
using ErrorOr;
using MediatR;

namespace Taxes.Application.Queries.GetTaxRateOnDate
{
    public record GetTaxRateOnDateQuery(string City, DateTime Date) : IRequest<ErrorOr<decimal>>;
}