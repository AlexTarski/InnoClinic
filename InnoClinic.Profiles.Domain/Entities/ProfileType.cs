using System.ComponentModel;

namespace InnoClinic.Profiles.Domain.Entities
{
    public enum ProfileType
    {
        [Description(nameof(Patient))]Patient,
        Doctor,
        Receptionist,
        UnknownProfile
    }
}