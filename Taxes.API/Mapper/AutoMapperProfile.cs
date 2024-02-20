using AutoMapper;
using Taxes.Application.Commands.AddTax;
using Taxes.Application.Models;
using Taxes.Constants;
using Taxes.Contracts.Request;
using Taxes.Contracts.Response;
using Taxes.Extensions;

namespace Taxes.Mapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<TaxDto, TaxScheduleResponse>()
                .ForMember(dest => dest.DateRange, opt => opt.MapFrom(src => src.StartDate.GetDateRange(src.EndDate)))
                .ForMember(dest => dest.Rate, opt => opt.MapFrom(src => src.Rate.ToString(Formats.Decimal)));

            CreateMap<AddTaxRequest, AddTaxCommand>();
        }
    }
}