using System.ComponentModel.DataAnnotations;

namespace InnoClinic.Profiles.Business.Models;

public class AccountModel
{
    public Guid Id { get; set; }
    [EmailAddress]
    public string Email { get; set; }
    public string Password { get; set; }
    [Phone]
    public string PhoneNumber { get; set; }
    public bool IsEmailVerified { get; set; }
    public Guid PhotoId { get; set; }
    public Guid CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public Guid UpdatedBy { get; set; }
    public DateTime UpdatedOn { get; set; }
}