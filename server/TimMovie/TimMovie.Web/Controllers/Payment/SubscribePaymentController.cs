using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TimMovie.Core.DTO.Payment;
using TimMovie.Core.Entities;
using TimMovie.Core.Interfaces;
using TimMovie.SharedKernel.Classes;
using TimMovie.Web.Extensions;
using TimMovie.Web.ViewModels.Payment;

namespace TimMovie.Web.Controllers.Payment;

[Authorize]
public class SubscribePaymentController : Controller
{
    private readonly IPaymentService _paymentService;
    private readonly IMapper _mapper;
    private readonly ISubscribeService _subscribeService;
    private readonly UserManager<User> _userManager;

    public SubscribePaymentController(IPaymentService paymentService, IMapper mapper,
        ISubscribeService subscribeService, UserManager<User> userManager
    )
    {
        _paymentService = paymentService;
        _mapper = mapper;
        _subscribeService = subscribeService;
        _userManager = userManager;
    }


    [HttpGet]
    public async Task<IActionResult> Payment(Guid subscribeId, string? returnUrl)
    {
        var subscribe = _subscribeService.GetSubscribeById(subscribeId);
        if (subscribe is null)
            return BadRequest();
        var user = await _userManager.FindByNameAsync(User.Identity!.Name);
        var payment = new SubscribePaymentViewModel
        {
            User = user,
            Subscribe = subscribe
        };
        return View("~/Views/Payment/Payment.cshtml",
            new SubscribePaymentWithCardViewModel
                {SubscribePaymentViewModel = payment, CardViewModel = new CardViewModel()});
    }


    [HttpPost]
    public async Task<JsonResult> Payment(CardViewModel cardViewModel, Guid subscribeId, string? returnUrl)
    {
        if (!ModelState.IsValid)
            return new JsonResult(Result.Fail("неверные данные карты"));

        var cardDto = _mapper.Map<CardDto>(cardViewModel);
        var paymentResult = await _paymentService.PaySubscribeAsync(User.GetUserId(), subscribeId, cardDto);

        return paymentResult.IsFailure 
            ? new JsonResult(Result.Fail(paymentResult.Error)) 
            : new JsonResult(Result.Ok());
    }
}