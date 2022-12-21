using System.ComponentModel.DataAnnotations;

namespace TimMovie.Core.DTO.Account;

public class UserRegistrationDto
{
    [Required(ErrorMessage = "Это обязательно поле")]
    [EmailAddress(ErrorMessage = "Неверный тип почты")]
    [Display(Name = "Почта")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Это обязательно поле")]
    [Display(Name = "Логин")]
    public string UserName { get; set; }
    
    [Required(ErrorMessage = "Это обязательно поле")]
    [Display(Name = "Пароль")]
    [DataType(DataType.Password)]
    public string Password { get; set; }
    
    public string? Ip { get; set; }
}