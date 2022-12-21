using System.ComponentModel.DataAnnotations;

namespace TimMovie.Web.ViewModels.Account;

public class ExternalLoginViewModel
{
    [Required(ErrorMessage = "Это обязательно поле")]
    [EmailAddress(ErrorMessage = "Неверный тип почты")]
    [Display(Name = "Почта")]
    public string Email { get; set; }
    
    public string? ReturnUrl { get; set; }
}