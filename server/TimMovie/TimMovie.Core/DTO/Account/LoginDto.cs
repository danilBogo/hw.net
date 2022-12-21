using System.ComponentModel.DataAnnotations;
namespace TimMovie.Core.DTO.Account;

public class LoginDto
{
    [Required]
    public string Login { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }
    public bool RememberMe { get; set; }
}