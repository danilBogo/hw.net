using System.ComponentModel.DataAnnotations;

namespace TimMovie.Core.DTO.Account;

public class ExternalLoginDto
{
    [Required(ErrorMessage = "Это обязательно поле")]
    [EmailAddress(ErrorMessage = "Неверный тип почты")]
    [Display(Name = "Почта")]
    public string Email { get; set; }
    
    public string ReurnUrl { get; set; }
    
    public string? Ip { get; set; }
}