using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using TimMovie.Core.DTO.Account;
using TimMovie.Core.DTO.Users;
using TimMovie.SharedKernel.Classes;

namespace TimMovie.Core.Interfaces;

public interface IUserService
{
    //
    public Task<IdentityResult> RegisterUserAsync(UserRegistrationDto userRegistrationDto);

    //
    public Task<Result> SendConfirmEmailAsync(string email, string urlToAction);

    //
    public Task<Result> ConfirmEmailAsync(string userId, string code);

    public AuthenticationProperties GetExternalAuthenticationProperties(string provider, string redirectUrl);
    public Task<SignInResult> ExternalLoginCallback();
    public Task<Result> RegisterExternalAsync(ExternalLoginDto externalLoginDto);
    
    public Task<bool> IsEmailExistAsync(string email);

    public Task<SignInResult> LoginAsync(LoginDto loginDto);
    
    public Task<ExternalLoginInfo?> GetExternalLoginInfoAsync();

    public Task<UserInfoDto> GetInfoAboutUserAsync(Guid userId);
    public Task<bool> UserIsExisted(Guid id);
    public Task<Result> UpdateUserPhotoAsync(IFormFile photo, Guid userId, string pathToContentDirectory);
    public Task UpdateUserInfo(ShortUserInfoDto userInfo, Guid userId);
    public Task<UserInfoForChatDto?> GetUserInfoForChat(Guid userId);
}