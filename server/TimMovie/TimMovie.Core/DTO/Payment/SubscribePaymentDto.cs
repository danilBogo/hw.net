using TimMovie.Core.Entities;

namespace TimMovie.Core.DTO.Payment;

public class SubscribePaymentDto
{
    public Subscribe Subscribe { get; set; } = null!;

    public User User { get; set; } = null!;
}