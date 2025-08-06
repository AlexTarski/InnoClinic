using InnoClinic.Authorization.Domain.Entities.Users;

namespace InnoClinic.Authorization.Business.Models.UserModels;

public class YourEntityModel : UserModel
{
    public DateTime Birth { get; set; }
    public Guid RefId { get; set; }
    public ushort Year { get; set; }
    public YourEntityStatus EntityStatus { get; set; }
}