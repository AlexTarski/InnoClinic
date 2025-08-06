namespace InnoClinic.Authorization.Domain.Entities.Users
{
    public enum YourEntityStatus
    {
        Status1,
        Status2
    }

    public class YourEntity : User
    {
        public DateTime Birth { get; set; }
        public Guid RefId { get; set; }
        public ushort Year { get; set; }
        public YourEntityStatus EntityStatus { get; set; }
    }
}
