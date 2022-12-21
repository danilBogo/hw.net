using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TimMovie.Core.DTO.Account;
using TimMovie.Core.Entities;
using TimMovie.Core.Interfaces;
using TimMovie.Core.Services.Banners;
using TimMovie.Web.ViewModels;
using TimMovie.Web.ViewModels.Account;

namespace TimMovie.Web.Controllers;

[AllowAnonymous]
public class AccountController : Controller
{
    private readonly ILogger<AccountController> logger;
    private readonly UserManager<User> userManager;
    private readonly IMapper mapper;
    private readonly IUserService userService;
    private readonly BannerService bannerService;
    private string? UserIp => HttpContext.Connection.RemoteIpAddress?.ToString();
    private string UrlToConfirmEmail => Url.Action("ConfirmEmail", "Account", null, HttpContext.Request.Scheme)!;

    public AccountController(UserManager<User> userManager,
        IMapper mapper,
        ILogger<AccountController> logger,
        IUserService userService)
    {
        this.userManager = userManager;
        this.mapper = mapper;
        this.logger = logger;
        this.userService = userService;
    }

    [HttpGet]
    public IActionResult Registration(string? returnUrl)
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> RegistrationAsync(RegistrationViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var userDto = mapper.Map<UserRegistrationDto>(model);
        userDto.Ip = UserIp;
        var registerUserResult = await userService.RegisterUserAsync(userDto);

        if (!registerUserResult.Succeeded)
        {
            AddErrors(registerUserResult);
            return View(model);
        }

        var sendResult = await userService.SendConfirmEmailAsync(userDto.Email, UrlToConfirmEmail);
        if (sendResult.Succeeded)
        {
            logger.LogInformation($"send email to {userDto.Email}");
            return PartialView("MailSend", (userDto.Email, userDto.UserName));
        }

        logger.LogError(sendResult.Error);
        return View();
    }


    [HttpGet]
    public async Task<IActionResult> ConfirmEmailAsync(string userId, string code)
    {
        var confirmResult = await userService.ConfirmEmailAsync(userId, code);
        if (confirmResult.Succeeded)
        {
            logger.LogInformation($"user with id {userId} confirmed email");
            return RedirectToAction("MainPage", "MainPage");
        }

        logger.LogError(confirmResult.Error);
        return View("Error");
    }

    [HttpPost]
    public IActionResult RegisterByVk(string provider, string returnUrl)
    {
        var redirectUrl = Url.Action("ExternalLoginCallback", new { returnUrl });
        var properties = userService.GetExternalAuthenticationProperties(provider, redirectUrl!);
        return Challenge(properties, provider);
    }

    [AllowAnonymous]
    public async Task<IActionResult> ExternalLoginCallback(string returnUrl)
    {
        var result = await userService.ExternalLoginCallback();
        if (result.Succeeded)
        {
            return RedirectToAction("MainPage", "MainPage");
        }

        var info = await userService.GetExternalLoginInfoAsync();

        if (info is null)
        {
            return RedirectToAction("Registration");
        }

        if (result.IsNotAllowed)
        {
            var userFromDb = await userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);
            return PartialView("MailSend", (userFromDb.Email, userFromDb.DisplayName));
        }

        return RedirectToAction("RegisterExternal",
            new { ReturnUrl = returnUrl, Email = info.Principal.FindFirstValue(ClaimTypes.Email) });
    }

    [AllowAnonymous]
    public IActionResult RegisterExternal(ExternalLoginViewModel model)
    {
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> RegisterExternalAsync(ExternalLoginViewModel model)
    {
        if (await userService.IsEmailExistAsync(model.Email))
        {
            ModelState.AddModelError(string.Empty, $"почта {model.Email} уже занята");
            return View(model);
        }

        var externalLoginDto = mapper.Map<ExternalLoginDto>(model);
        externalLoginDto.Ip = UserIp;

        var registrationResult = await userService.RegisterExternalAsync(externalLoginDto);
        if (registrationResult.Succeeded)
        {
            var sendResult = await userService.SendConfirmEmailAsync(model.Email, UrlToConfirmEmail);
            if (sendResult.Succeeded)
            {
                var userFromDb = await userManager.FindByEmailAsync(model.Email);
                logger.LogInformation($"send email to {model.Email}");
                return PartialView("MailSend", (userFromDb.Email, userFromDb.DisplayName));
            }
        }

        logger.LogError(registrationResult.Error);
        return View("Registration");
    }

    [HttpPost]
    [ActionName("SendConfirmEmail")]
    public async Task<IActionResult> SendConfirmEmailAction(string email)
    {
        if (!await userService.IsEmailExistAsync(email))
        {
            logger.LogError($"пользователя с почтой {email} не существует");
            return BadRequest();
        }

        var sendResult = await userService.SendConfirmEmailAsync(email, UrlToConfirmEmail);
        if (sendResult.Succeeded)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user is not null)
            {
                logger.LogInformation($"письмо на почту {email} отправлено еще раз");
                return PartialView("MailSend", (user.Email, user.DisplayName));
            }

            logger.LogError($"пользователь с почтой {email} не найден для повторной отправки сообщения");

            return View("Error");
        }

        logger.LogError($"не удалось отправить повторное сообщение на почту: {sendResult.Error}");
        return View("Error");
    }

    [HttpPost]
    
    public async Task<IActionResult> LoginAsync(string login, string password, bool rememberMe)
    {
        var loginDto = new LoginDto
        {
            Login = login,
            Password = password,
            RememberMe = rememberMe
        };


        var loginResult = await userService.LoginAsync(loginDto);

        if (loginResult.Succeeded)
        {
            logger.LogInformation($"пользователь {login} вошел в свой акканут");
            return Ok();
        }

        if (loginResult.IsNotAllowed)
        {
            var userFromDb = await userManager.FindByNameAsync(login) ?? await userManager.FindByEmailAsync(login);
            return PartialView("MailSend", (userFromDb.Email, userFromDb.DisplayName));
        }

        logger.LogInformation($"неудачная попытка входа с использованием логина {login}");
        return Ok("Неверный(е) логин/почта или пароль");
    }

    [HttpGet]
    public IActionResult Denied()
    {
        return View("Denied");
    }

    private void AddErrors(IdentityResult result)
    {
        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }
    }
}