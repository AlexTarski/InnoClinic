using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using InnoClinic.Appointments.Domain.Entities;
using InnoClinic.Shared.Pagination;

namespace InnoClinic.Appointments.Domain
{
    public interface ICrudRepository<T>
        where T : Entity
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<PagedList<T>> GetAllAsync(IQueryable<T> query, QueryStringParameters queryParams);
        Task<T> GetByIdAsync(Guid id);
        IQueryable<T> GetEntityQuery();
        Task<bool> SaveAllAsync();
    }
}