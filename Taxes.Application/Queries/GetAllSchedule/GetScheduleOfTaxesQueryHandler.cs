using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using ErrorOr;
using MediatR;
using Taxes.Application.Common.Interfaces;
using Taxes.Application.Models;

namespace Taxes.Application.Queries.GetAllSchedule
{
    public class GetScheduleOfTaxesQueryHandler : IRequestHandler<GetScheduleOfTaxesQuery, ErrorOr<List<TaxDto>>>
    {
        private readonly IMapper _mapper;
        private readonly ITaxRepository _taxRepository;

        public GetScheduleOfTaxesQueryHandler(ITaxRepository taxRepository, IMapper mapper)
        {
            _taxRepository = taxRepository;
            _mapper = mapper;
        }

        public async Task<ErrorOr<List<TaxDto>>> Handle(
            GetScheduleOfTaxesQuery query,
            CancellationToken cancellationToken)
        {
            var taxes = (await _taxRepository.GetAllTaxesByCityAsync(query.City))
                .OrderBy(x => x.Category)
                .ThenBy(x => x.StartDate)
                .ThenByDescending(x => new[] { x.CreatedAt, x.ModifiedAt }.Max())
                .ToList();

            if (!taxes.Any())
            {
                return Error.NotFound(description: $"Tax records with the provided City({query.City}) not found");
            }

            return _mapper.Map<List<TaxDto>>(taxes);
        }
    }
}