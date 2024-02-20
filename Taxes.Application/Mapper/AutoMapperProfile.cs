using AutoMapper;
using Taxes.Application.Commands.AddTax;
using Taxes.Application.Extensions;
using Taxes.Application.Models;
using Taxes.Domain.Models;

namespace Taxes.Application.Mapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Tax, TaxDto>()
                .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.StartDate.GetEndDate(src.Category)));

            CreateMap<AddTaxCommand, Tax>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.ModifiedAt, opt => opt.Ignore());
        }
    }
}