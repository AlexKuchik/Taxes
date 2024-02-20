using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using ErrorOr;
using MediatR;
using Taxes.Application.Common.Interfaces;
using Taxes.Domain.Models;

namespace Taxes.Application.Commands.AddTax
{
    public class AddTaxCommandHandler : IRequestHandler<AddTaxCommand, ErrorOr<Guid>>
    {
        private readonly IMapper _mapper;
        private readonly ITaxRepository _taxRepository;

        public AddTaxCommandHandler(
            ITaxRepository taxRepository,
            IMapper mapper)
        {
            _taxRepository = taxRepository;
            _mapper = mapper;
        }

        public async Task<ErrorOr<Guid>> Handle(AddTaxCommand command, CancellationToken cancellationToken)
        {
            var tax = _mapper.Map<Tax>(command);

            return await _taxRepository.AddAsync(tax);
        }
    }
}