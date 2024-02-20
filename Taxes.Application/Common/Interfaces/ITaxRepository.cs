using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Taxes.Domain.Models;

namespace Taxes.Application.Common.Interfaces
{
    public interface ITaxRepository
    {
        Task<List<string>> GetAllCitiesAsync();
        Task<List<Tax>> GetAllTaxesByCityAsync(string city);
        Task<Tax?> GetAsync(Guid id);
        Task DeleteAsync(Tax entity);
        Task UpdateAsync(Tax entity);
        Task<Guid> AddAsync(Tax entity);
    }
}