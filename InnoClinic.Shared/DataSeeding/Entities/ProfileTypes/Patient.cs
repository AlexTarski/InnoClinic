namespace InnoClinic.Shared.DataSeeding.Entities.ProfileTypes
{
    public class Patient : User
    {
        public DateTime DateOfBirth { get; set; }
        public bool IsLinkedToAccount { get; set; }
    }
}
