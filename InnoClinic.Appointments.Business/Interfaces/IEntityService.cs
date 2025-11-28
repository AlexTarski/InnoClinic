using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using InnoClinic.Services.Domain.Entities;
using InnoClinic.Shared.Pagination;

namespace InnoClinic.Appointments.Business.Interfaces
{
    public interface IEntityService<T, TParams>
    where T : Entity
    where TParams : QueryStringParameters
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<PagedList<T>> GetAllFilteredAsync(TParams queryParams);
        Task<T> GetByIdAsync(Guid id);
        void ApplyFilters(ref IQueryable<T> query, TParams queryParams);
        Task<bool> SaveAllAsync();
    }
}