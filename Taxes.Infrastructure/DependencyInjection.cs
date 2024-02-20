using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Taxes.Application.Common.Interfaces;
using Taxes.Infrastructure.Handlers;
using Taxes.Infrastructure.Persistence;
using Taxes.Infrastructure.Security;
using Taxes.Infrastructure.Security.Constants;
using Taxes.Infrastructure.Security.Settings;

namespace Taxes.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IHostApplicationBuilder builder)
        {
            services.AddSingleton<IExceptionHandler, GlobalExceptionHandler>();
            services.AddAuthentication()
                .AddAuthorization()
                .AddPersistence(builder);

            return services;
        }

        private static IServiceCollection AddPersistence(
            this IServiceCollection services,
            IHostApplicationBuilder builder)
        {
            services.AddDbContext<TaxDbContext>(options =>
            {
                var connectionString = builder.Configuration.GetConnectionString("ConnectionString");
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
            });
            services.AddScoped<ITaxRepository, TaxRepository>();

            return services;
        }

        private static IServiceCollection AddAuthorization(this IServiceCollection services)
        {
            services.AddAuthorizationBuilder()
                .AddPolicy(Policies.AdminOnly,
                    policy =>
                        policy.RequireRole(Roles.Admin));

            return services;
        }

        private static IServiceCollection AddAuthentication(this IServiceCollection services)
        {
            services.AddAuthentication
                    (AuthenticationSettings.DefaultScheme)
                .AddScheme<AuthenticationSettings, AuthenticationHandler>(AuthenticationSettings.DefaultScheme,
                    options => { });

            return services;
        }
    }
}