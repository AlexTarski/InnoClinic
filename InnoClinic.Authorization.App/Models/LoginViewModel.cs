using System.ComponentModel.DataAnnotations;

namespace InnoClinic.Authorization.Business.Models;

public class LoginViewModel
{
    [Required(ErrorMessage = "Please, enter the email")]
    [EmailAddress(ErrorMessage = "You've entered an invalid email")]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; }

    [Required(ErrorMessage = "Please, enter the password")]
    [MinLength(6, ErrorMessage = "Password must be minimum 6 symbols long")]
    [MaxLength(15, ErrorMessage = "Password must be no longer than 15 symbols")]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    public string ReturnUrl { get; set; }
}