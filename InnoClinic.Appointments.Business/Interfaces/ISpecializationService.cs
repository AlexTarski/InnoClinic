using InnoClinic.Appointments.Business.Filters;
using InnoClinic.Services.Domain.Entities;

namespace InnoClinic.Appointments.Business.Interfaces
{
    public interface ISpecializationService : IEntityService<Specialization, SpecializationParameters>
    {
    }
}