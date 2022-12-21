using System.ComponentModel.DataAnnotations;

namespace TimMovie.Core.DTO.Payment;

public class CardDto
{
    [Required(ErrorMessage = "это обязательное поле")]
    [MinLength(13, ErrorMessage = "неверная длина карты")]
    [MaxLength(19, ErrorMessage = "неверная длина карты")]
    public string CardNumber { get; set; } = null!;
    
    [DataType(DataType.Password)]
    [Required(ErrorMessage = "это обязательное поле")]
    [MinLength(3,ErrorMessage = "неверная длина cvv")]
    [MaxLength(7,ErrorMessage = "неверная длина cvv")]
    public string CCID { get; set; } = null!;
    
    [Required(ErrorMessage = "это обязательное поле")]
    [Range(1,12,ErrorMessage = "недопустимое значение для месяца")]
    public int ExpirationMonth { get; set; }
    
    [Required(ErrorMessage = "это обязательное поле")]
    [Range(2022,int.MaxValue,ErrorMessage = "срок годности вашей карты истек")]
    public int ExpirationYear { get; set; }
}