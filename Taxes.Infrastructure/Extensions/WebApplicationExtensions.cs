using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Taxes.Infrastructure.Persistence;

namespace Taxes.Infrastructure.Extensions
{
    public static class WebApplicationExtensions
    {
        public static void UseDatabaseMigration(this WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<TaxDbContext>();
                db.Database.Migrate();
            }
        }
    }
}