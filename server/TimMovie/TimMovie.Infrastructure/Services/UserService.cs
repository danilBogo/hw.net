using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TimMovie.Core.DTO.Account;
using TimMovie.Core.DTO.Users;
using TimMovie.Core.Entities;
using TimMovie.Core.Interfaces;
using TimMovie.Core.Services.Countries;
using TimMovie.Core.Services.Films;
using TimMovie.Core.Specifications.InheritedSpecifications;
using TimMovie.SharedKernel.Classes;
using TimMovie.SharedKernel.Extensions;
using TimMovie.SharedKernel.Validators;

namespace TimMovie.Infrastructure.Services;

public class UserService : IUserService
{
    private readonly SignInManager<User> signInManager;
    private readonly UserManager<User> userManager;
    private readonly IMapper mapper;
    private readonly IMailService mailService;
    private readonly IUserMessageService userMessageService;
    private readonly IIpService ipService;
    private readonly CountryService countryService;
    private readonly IVkService vkService;
    private readonly FilmService _filmService;
    private readonly IFileService _fileService;

    public UserService(SignInManager<User> signInManager, UserManager<User> userManager, IMapper mapper,
        IMailService mailService, IUserMessageService userMessageService, IIpService ipService,
        CountryService countryService, IVkService vkService, FilmService filmService, IFileService fileService)
    {
        this.signInManager = signInManager;
        this.userManager = userManager;
        this.mapper = mapper;
        this.mailService = mailService;
        this.userMessageService = userMessageService;
        this.ipService = ipService;
        this.countryService = countryService;
        this.vkService = vkService;
        _filmService = filmService;
        _fileService = fileService;
    }

    public async Task<IdentityResult> RegisterUserAsync(UserRegistrationDto userRegistrationDto)
    {
        var user = mapper.Map<User>(userRegistrationDto);
        user.RegistrationDate = DateTime.Now;
        user.DisplayName = userRegistrationDto.UserName;
        user.BirthDate = DateOnly.FromDateTime(DateTime.Today);
        user.PathToPhoto = await _fileService.GetLinkToDefaultUserPhoto();
        if (userRegistrationDto.Ip != null)
            await AddCountryByIpAsync(user, userRegistrationDto.Ip);
        var registerResult = await userManager.CreateAsync(user, userRegistrationDto.Password);
        if (registerResult.Succeeded)
        {
            var userFromDb = await userManager.FindByNameAsync(userRegistrationDto.UserName);
            await UpdateClaimsAsync(userFromDb);
            await userManager.UpdateAsync(userFromDb);
        }

        return registerResult;
    }

    public async Task<Result> SendConfirmEmailAsync(string email, string urlToAction)
    {
        var userFromDb = await userManager.FindByEmailAsync(email);
        if (userFromDb is null)
            return Result.Fail("can`t find user by userName while sending email");
        if (userFromDb.EmailConfirmed)
            return Result.Fail("mail has already been confirmed");

        var token = await userManager.GenerateEmailConfirmationTokenAsync(userFromDb);
        var confirmUrl = CreateUrlToConfirmEmail(urlToAction, userFromDb, token);
        var msg = userMessageService.GenerateConfirmMessage(userFromDb.DisplayName, userFromDb.Email, confirmUrl);
        var result = await mailService.SendMessageAsync(msg);
        return result;
    }

    public async Task<Result> ConfirmEmailAsync(string userId, string code)
    {
        if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(code))
        {
            return Result.Fail("userId or code was empty in confirmEmail");
        }

        var user = await userManager.FindByIdAsync(userId);

        if (user == null)
        {
            return Result.Fail("can`t find user by id in confirmEmail");
        }

        var result = await userManager.ConfirmEmailAsync(user, code);

        if (result.Succeeded)
        {
            await signInManager.SignInAsync(user, false);
            return Result.Ok();
        }

        return Result.Fail("cant confirm user email");
    }

    public AuthenticationProperties GetExternalAuthenticationProperties(string provider, string redirectUrl)
    {
        return signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
    }

    public async Task<SignInResult> ExternalLoginCallback()
    {
        var info = await GetExternalLoginInfoAsync();
        if (info is null)
            return SignInResult.Failed;
        var result = await signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, false, false);
        return result;
    }

    public async Task<Result> RegisterExternalAsync(ExternalLoginDto externalLoginDto)
    {
        if (await IsEmailExistAsync(externalLoginDto.Email))
        {
            return Result.Fail($"почта {externalLoginDto.Email} уже занята");
        }

        var info = await GetExternalLoginInfoAsync();
        if (info is null)
        {
            return Result.Fail("не удалось получить регистрационную информацию из внешнего поставщика");
        }

        var id = info.Principal.FindFirstValue(ClaimTypes.NameIdentifier);
        var vkInfoResult = await vkService.GetUserInfoByIdAsync(id);
        if (vkInfoResult.Succeeded)
        {
            var user = await CreateUserByVkRegistrationAsync(externalLoginDto.Email, vkInfoResult.Value,
                externalLoginDto.Ip);
            var createUserResult = await userManager.CreateAsync(user);
            if (!createUserResult.Succeeded)
            {
                return Result.Fail("ошибка при создании пользователя");
            }

            var addLoginResult = await userManager.AddLoginAsync(user, info);
            if (!addLoginResult.Succeeded)
            {
                return Result.Fail(addLoginResult.Errors.First().Description);
            }

            await UpdateClaimsAsync(user);
            await signInManager.UpdateExternalAuthenticationTokensAsync(info);
            return Result.Ok();
        }

        return Result.Fail(vkInfoResult.Error);
    }

    public async Task<bool> IsEmailExistAsync(string email)
    {
        var userMail = await userManager.FindByEmailAsync(email);
        return userMail is not null;
    }

    public async Task<SignInResult> LoginAsync(LoginDto loginDto)
    {
        var user = await userManager.FindByEmailAsync(loginDto.Login);
        var login = user is null ? loginDto.Login : user.UserName;
        var result = await signInManager.PasswordSignInAsync(login, loginDto.Password, loginDto.RememberMe, false);
        if (result == SignInResult.NotAllowed)
        {
            if (await userManager.CheckPasswordAsync(user ?? await userManager.FindByNameAsync(login),
                    loginDto.Password))
            {
                return result;
            }
            return SignInResult.Failed;
        }

        return result;
    }

    public async Task<ExternalLoginInfo?> GetExternalLoginInfoAsync() =>
        await signInManager.GetExternalLoginInfoAsync();

    public async Task<UserInfoDto> GetInfoAboutUserAsync(Guid userId)
    {
        var user = await userManager.Users
            .Include(u => u.Status)
            .Include(u => u.Country)
            .FirstOrDefaultAsync(new EntityByIdSpec<User>(userId));
        
        var shortInfoAboutUser = mapper.Map<UserInfoDto>(user);
        shortInfoAboutUser.FilmForStatusDto = _filmService.GetCurrentWatchingFilmByUser(userId);
        
        return shortInfoAboutUser;
    }

    public async Task<bool> UserIsExisted(Guid id)
    {
        return await userManager.FindByIdAsync(id.ToString()) is not null;
    }

    public async Task<Result> UpdateUserPhotoAsync(IFormFile photo, Guid userId, string pathToContentDirectory)
    {
        var user = await userManager.FindByIdAsync(userId.ToString());
        if (!await UserIsExisted(userId))
        {
            return Result.Fail("Пользователя с таким id не существует");
        }
        
        try
        {
            user.PathToPhoto = await _fileService.SaveUserPhoto(photo);
            
            await userManager.UpdateAsync(user);
            
            return Result.Ok();
        }
        catch (Exception e)
        {
            return Result.Fail(e.Message);
        }
    }
    
    public async Task UpdateUserInfo(ShortUserInfoDto userInfo, Guid userId)
    {
        var user = await userManager.Users
            .Include(u => u.Country)
            .FirstOrDefaultAsync(new EntityByIdSpec<User>(userId));
        
        if (user is null)
        {
            throw new InvalidOperationException($"В методе {nameof(UpdateUserInfoForUser)} не " +
                                                $"найден пользователь с id: {userId}");
        }

        await UpdateUserInfoForUser(userInfo, user);
    }

    public async Task<UserInfoForChatDto?> GetUserInfoForChat(Guid userId)
    {
        var user = await userManager.FindByIdAsync(userId.ToString());
        return user is null
            ? null
            : mapper.Map<UserInfoForChatDto>(user);
    }

    private async Task UpdateUserInfoForUser(ShortUserInfoDto userInfo, User user)
    {
        user.DisplayName = userInfo.DisplayName;
        user.BirthDate = userInfo.BirthDate is null
            ? null
            : DateOnly.FromDateTime(userInfo.BirthDate.Value);
        var country = countryService.FindByName(userInfo.CountryName);
        user.Country = country;

        await userManager.UpdateAsync(user);
    }

    private string GenerateRandomFileNameWithExtension(string extension)
    {
        ArgumentValidator.ThrowExceptionIfNull(extension, nameof(extension));
        
        return $"{Path.GetFileNameWithoutExtension(Path.GetRandomFileName())}{extension}";
    }

    private async Task UpdateClaimsAsync(User user)
    {
        await userManager.AddClaimAsync(user, new Claim(ClaimTypes.DateOfBirth, user.BirthDate.ToString()));
    }

    private async Task AddCountryByIpAsync(User user, string ip)
    {
        if (string.IsNullOrEmpty(ip)) return;
        var userCountryResult = await ipService.GetCountryByIpAsync(ip);
        if (userCountryResult.Succeeded)
        {
            var countryFromDb = countryService.FindByName(userCountryResult.Value);
            if (countryFromDb != null)
            {
                user.Country = countryFromDb;
            }
        }
    }

    private async Task<User> CreateUserByVkRegistrationAsync(string email, Core.Classes.VkUserInfo vkInfo, string? ip)
    {
        var user = new User
        {
            UserName = email.GetMailName().AddRandomEnd(), Email = email,
            DisplayName = vkInfo.FirstName + " " + vkInfo.LastName,
            BirthDate = vkInfo.Birthday,
            RegistrationDate = DateTime.Now,
            PathToPhoto = await _fileService.GetLinkToDefaultUserPhoto()
        };
        if (ip != null)
            await AddCountryByIpAsync(user, ip);
        return user;
    }

    private string CreateUrlToConfirmEmail(string urlToAction, User user, string token)
    {
        var queryString = System.Web.HttpUtility.ParseQueryString(string.Empty);
        queryString.Add("userId", user.Id.ToString());
        queryString.Add("code", token);
        return urlToAction + "?" + queryString;
    }
}