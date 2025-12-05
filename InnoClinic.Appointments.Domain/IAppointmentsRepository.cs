using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using InnoClinic.Appointments.Domain.Entities;
using InnoClinic.Shared.Pagination;

namespace InnoClinic.Appointments.Domain
{
    public interface IAppointmentsRepository
    {
        Task<IEnumerable<Appointment>> GetAllAsync();
        Task<PagedList<Appointment>> GetAllAsync(IQueryable<Appointment> query, QueryStringParameters queryParams);
        Task<Appointment> GetByIdAsync(Guid id);
        IQueryable<Appointment> GetEntityQuery();
        Task<bool> SaveAllAsync();
    }
}