using System.ComponentModel.DataAnnotations;
using TimMovie.Core.DTO.Payment;
using TimMovie.SharedKernel.Classes;

namespace TimMovie.Core.Interfaces;

public interface IPaymentService
{
    public bool TryValidateCard(CardDto card, out ValidationResult validationResult);

    public Task<Result> PaySubscribeAsync(Guid? userId, Guid subscribeId, CardDto cardDto);
}