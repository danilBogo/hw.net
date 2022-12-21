using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using TimMovie.Core.DTO.Payment;
using TimMovie.Core.Entities;
using TimMovie.Core.Interfaces;
using TimMovie.SharedKernel.Classes;

namespace TimMovie.Infrastructure.Services;

public class PaymentService : IPaymentService
{
    private readonly ISubscribeService _userSubscribeService;
    private readonly UserManager<User> _userManager;

    public PaymentService(ISubscribeService userSubscribeService, UserManager<User> userManager)
    {
        this._userSubscribeService = userSubscribeService;
        _userManager = userManager;
    }

    public bool TryValidateCard(CardDto card, out ValidationResult validationResult)
    {
        validationResult = null;
        var results = new List<ValidationResult>();
        var vc = new ValidationContext(card);
        var isValid = Validator.TryValidateObject(card, vc, results, true);
        var date = DateTime.Now;
        if (isValid)
            if (card.ExpirationYear >= date.Year &&
                (card.ExpirationYear != date.Year || card.ExpirationMonth >= date.Month))
                return true;
            else
            {
                validationResult = new ValidationResult("срок годности вашей карты истек");
                return false;
            }

        validationResult = results.First();
        return false;
    }

    public async Task<Result> PaySubscribeAsync(Guid? userId, Guid subscribeId, CardDto cardDto)
    {
        var userFromDb = await _userManager.FindByIdAsync(userId.ToString());
        if (userFromDb is null)
            return Result.Fail("данного пользователя не существует");
        var subscribeFromDb = _userSubscribeService.GetSubscribeById(subscribeId);
        if (subscribeFromDb is null)
            return Result.Fail("данной подписки не существует");
        if (!TryValidateCard(cardDto, out var validationResult))
            return Result.Fail(validationResult.ErrorMessage!);
        if (new Random().NextDouble() < 0.15)
            return Result.Fail("банк отклонил вашу покупку");
        await _userSubscribeService.AddUserToSubscribeAsync(userFromDb, subscribeFromDb);
        return Result.Ok();
    }
}