using TimMovie.Core.Classes;
using TimMovie.SharedKernel.Classes;

namespace TimMovie.Core.Interfaces;

public interface IVkService
{
    public string AccessToken { get; set; }
    public Task<Result<VkUserInfo>>  GetUserInfoByIdAsync(string id);
}