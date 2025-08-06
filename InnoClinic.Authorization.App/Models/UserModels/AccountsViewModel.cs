using InnoClinic.Authorization.Domain.Entities.Users;
using System.ComponentModel.DataAnnotations;

namespace InnoClinic.Authorization.Business.Models.UserModels;

public class AccountsViewModel
{
    public Guid Id { get; set; }

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email fromat")]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is required")]
    [MinLength(6, ErrorMessage = "Password must be minimum 6 symbols long")]
    [MaxLength(15, ErrorMessage = "Password must be no longer than 15 symbols")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "Phone number is required")]
    [Phone(ErrorMessage = "Invalid phone number format")]
    [DataType(DataType.PhoneNumber)]
    public string PhoneNumber { get; set; } = string.Empty;

    public bool IsEmailVerified { get; set; } = false;
    public Guid PhotoId { get; set; } = Guid.Empty;
    public Guid CreatedBy { get; set; } = Guid.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public Guid? UpdatedBy { get; set; } = Guid.Empty;
    public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;
}