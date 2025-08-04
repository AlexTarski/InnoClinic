using System.ComponentModel.DataAnnotations;

namespace InnoClinic.Authorization.Business.Models.UserModels;

public abstract class UserModel
{
    public Guid Id { get; set; }
    [Required]
    public string Name { get; set; }
    public Guid Ref2Id { get; set; }
}