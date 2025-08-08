using System.ComponentModel.DataAnnotations;

namespace InnoClinic.Authorization.Business.Models;

public class MessageViewModel
{
    [Required]
    [MaxLength(100)]
    public string Title { get; set; } = String.Empty;
    
    [Required]
    [MaxLength(100)]
    public string Header { get; set; } = String.Empty;
    
    [Required]
    [MaxLength(1000)]
    public string Message { get; set; } = String.Empty;
}