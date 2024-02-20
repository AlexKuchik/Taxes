using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Taxes.Application.Common.Behaviors;
using Taxes.Application.Common.Interfaces;
using Taxes.Application.Common.Services;
using Taxes.Application.Mapper;

namespace Taxes.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));
            services.AddTransient(
                typeof(IPipelineBehavior<,>),
                typeof(ValidationBehavior<,>));
            services.AddTransient<IDateValidationService, DateValidationService>();
            services.AddValidatorsFromAssemblyContaining(typeof(DependencyInjection));
            services.AddMapper();

            return services;
        }

        private static void AddMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(AutoMapperProfile));
            var mapperConfiguration = new MapperConfiguration(cfg => { cfg.AddProfile<AutoMapperProfile>(); });
            mapperConfiguration.AssertConfigurationIsValid();
        }
    }
}