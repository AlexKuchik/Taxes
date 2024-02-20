using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Takes.Domain.Models;
using Taxes.Application.Common.Interfaces;

namespace Taxes.Infrastructure.Persistence
{
    public class TaxRepository : ITaxRepository
    {
        private readonly TaxDbContext _context;

        public TaxRepository(TaxDbContext context)
        {
            _context = context;
        }

        public async Task<List<string>> GetAllCitiesAsync()
        {
            return await _context
                .Taxes
                .Select(x => x.City)
                .Distinct()
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<Tax>> GetAllTaxesByCityAsync(string city)
        {
            return await _context
                .Taxes
                .Where(x => x.City.Equals(city))
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Tax?> GetAsync(Guid id)
        {
            return await _context.Taxes.FindAsync(id);
        }

        public async Task DeleteAsync(Tax entity)
        {
            _context.Taxes.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Tax entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task<Guid> AddAsync(Tax entity)
        {
            _context.Taxes.Add(entity);
            await _context.SaveChangesAsync();

            return entity.Id;
        }
    }
}