using InnoClinic.Profiles.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace InnoClinic.Profiles.Infrastructure;

public class ReceptionistsRepository : BaseCrudRepository<Receptionist>
{
    public ReceptionistsRepository(ProfilesContext context) 
        : base(context) { }
}