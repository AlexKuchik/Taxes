using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Takes.Domain.Models;
using Takes.Domain.Models.Interfaces;
using Taxes.Infrastructure.Extensions;

namespace Taxes.Infrastructure.Persistence
{
    public class TaxDbContext : DbContext
    {
        public DbSet<Tax> Taxes { get; set; }

        public TaxDbContext(DbContextOptions<TaxDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Tax>(entity => { entity.Property<Guid>(nameof(Tax.Id)).ValueGeneratedOnAdd(); });

            modelBuilder.SeedTaxData();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker
                .Entries()
                .Where(e => e.Entity is IAuditEntity &&
                    e.State is EntityState.Added or EntityState.Modified);

            foreach (var entityEntry in entries)
            {
                var auditEntity = (IAuditEntity)entityEntry.Entity;

                if (entityEntry.State == EntityState.Added)
                {
                    auditEntity.CreatedAt = DateTime.UtcNow;
                }
                else
                {
                    Entry(auditEntity).Property(p => p.CreatedAt).IsModified = false;
                    auditEntity.ModifiedAt = DateTime.UtcNow;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}