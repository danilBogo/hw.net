using TimMovie.Core.Entities;

namespace TimMovie.Web.ViewModels.Payment;

public class SubscribePaymentViewModel
{
    public string? ReturnUrl { get; set; }

    public Subscribe Subscribe { get; set; } = null!;

    public Core.Entities.User User { get; set; } = null!;
}