using System.ComponentModel.DataAnnotations;

namespace InnoClinic.Authorization.Business.Models;

public class LoginViewModel
{
    [Required]
    public string FullName { get; set; }
    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }
    public string ReturnUrl { get; set; }
}