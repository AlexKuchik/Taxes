using System.Collections.Generic;
using ErrorOr;
using MediatR;

namespace Taxes.Application.Queries.GetAppCities
{
    public record GetAllCitiesQuery : IRequest<ErrorOr<List<string>>>;
}