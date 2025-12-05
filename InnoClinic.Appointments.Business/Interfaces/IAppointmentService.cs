using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InnoClinic.Appointments.Domain.Entities;
using InnoClinic.Shared.Pagination;

namespace InnoClinic.Appointments.Business.Interfaces
{
    public interface IAppointmentService
    {
        Task<IEnumerable<Appointment>> GetAllAsync();
        Task<PagedList<Appointment>> GetAllFilteredAsync(QueryStringParameters queryParams);
        Task<Appointment> GetByIdAsync(Guid id);
        void ApplyFilters(ref IQueryable<Appointment> query, QueryStringParameters queryParams);
        Task<bool> SaveAllAsync();
    }
}