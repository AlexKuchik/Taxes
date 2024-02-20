using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using ErrorOr;
using MediatR;
using Taxes.Application.Common.Interfaces;
using Taxes.Application.Models;

namespace Taxes.Application.Queries.GetTaxRateOnDate
{
    public class GetTaxRateOnDateQueryHandler : IRequestHandler<GetTaxRateOnDateQuery, ErrorOr<decimal>>
    {
        private readonly IMapper _mapper;
        private readonly ITaxRepository _taxRepository;

        public GetTaxRateOnDateQueryHandler(ITaxRepository taxRepository, IMapper mapper)
        {
            _taxRepository = taxRepository;
            _mapper = mapper;
        }

        public async Task<ErrorOr<decimal>> Handle(GetTaxRateOnDateQuery query, CancellationToken cancellationToken)
        {
            var taxes = (await _taxRepository.GetAllTaxesByCityAsync(query.City))
                .OrderByDescending(x => x.Category)
                .ThenByDescending(x => new[] { x.CreatedAt, x.ModifiedAt }.Max())
                .ToList();
            if (!taxes.Any())
            {
                return Error.NotFound(description: $"Tax records with the provided City({query.City}) not found");
            }

            var taxesDto = _mapper.Map<List<TaxDto>>(taxes);
            var result = taxesDto
                .Where(dto => IsDateWithinRange(query.Date, dto.StartDate, dto.EndDate))
                .Select(dto => dto.Rate)
                .FirstOrDefault();

            return result;
        }

        private static bool IsDateWithinRange(DateTime dateToCheck, DateTime startDate, DateTime? endDate)
        {
            return endDate.HasValue ? dateToCheck >= startDate && dateToCheck <= endDate : dateToCheck == startDate;
        }
    }
}