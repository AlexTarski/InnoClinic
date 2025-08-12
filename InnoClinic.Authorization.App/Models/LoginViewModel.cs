using System.ComponentModel.DataAnnotations;

namespace InnoClinic.Authorization.Business.Models;

public class LoginViewModel
{
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email fromat")]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; }

    [Required(ErrorMessage = "Password is required")]
    [MinLength(6, ErrorMessage = "Password must be minimum 6 symbols long")]
    [MaxLength(15, ErrorMessage = "Password must be no longer than 15 symbols")]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    public string ReturnUrl { get; set; }

    public string ClientId { get; set; } = string.Empty;
}