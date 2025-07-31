using InnoClinic.Profiles.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace InnoClinic.Profiles.Infrastructure;

public class DoctorsRepository : BaseCrudRepository<Doctor>
{
    public DoctorsRepository(ProfilesContext context) 
    : base(context) { }
}