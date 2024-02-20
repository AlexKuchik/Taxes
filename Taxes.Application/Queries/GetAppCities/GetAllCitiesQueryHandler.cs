using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ErrorOr;
using MediatR;
using Taxes.Application.Common.Interfaces;

namespace Taxes.Application.Queries.GetAppCities
{
    public class GetAllCitiesQueryHandler : IRequestHandler<GetAllCitiesQuery, ErrorOr<List<string>>>
    {
        private readonly ITaxRepository _taxRepository;

        public GetAllCitiesQueryHandler(ITaxRepository taxRepository)
        {
            _taxRepository = taxRepository;
        }

        public async Task<ErrorOr<List<string>>> Handle(GetAllCitiesQuery query, CancellationToken cancellationToken)
        {
            return await _taxRepository.GetAllCitiesAsync();
        }
    }
}