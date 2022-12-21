using System.ComponentModel.DataAnnotations;

namespace TimMovie.Web.ViewModels.Account;

public class LoginViewModel
{
    [Required(ErrorMessage = "Это обязательное поле")]
    public string UserName { get; set; }

    [Required(ErrorMessage = "Это обязательное поле")]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [Display(Name = "Запомнить меня")]
    public bool RememberMe { get; set; }
}