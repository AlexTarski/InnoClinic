using System.ComponentModel.DataAnnotations;

namespace InnoClinic.Profiles.Business.Models;

public abstract class UserModel
{
    public Guid Id { get; set; }
    [Required]
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string MiddleName { get; set; }
    public Guid AccountId { get; set; }
}