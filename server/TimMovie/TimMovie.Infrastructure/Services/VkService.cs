using System.Globalization;
using System.Text.Json;
using TimMovie.Core.Classes;
using TimMovie.Core.Interfaces;
using TimMovie.SharedKernel.Classes;

namespace TimMovie.Infrastructure.Services;

public class VkService : IVkService
{
    private readonly HttpClient client;
    public VkService(string accessToken, HttpClient client)
    {
        AccessToken = accessToken;
        this.client = client;
    }

    public string AccessToken { get; set; }
    public async Task<Result<VkUserInfo>>  GetUserInfoByIdAsync(string id)
    {
        try
        {
            client.DefaultRequestHeaders.Add("accept-language", "ru-RU,ru;q=0.9,en-US;q=0.8,en;q=0.7");
            var resp = await client.SendAsync(new HttpRequestMessage(HttpMethod.Get,
                $"https://api.vk.com/method/users.get?user_ids={id}&fields=bdate&access_token={AccessToken}&v=5.131"));
            if (!resp.IsSuccessStatusCode)
                return Result.Fail<VkUserInfo>("error" + resp.StatusCode + " " +  resp.ReasonPhrase);
            var json = await resp.Content.ReadAsStringAsync();
            var result = ParseVkInfo(json);
            return result.Succeeded ? Result.Ok<VkUserInfo>(result.Value) : Result.Fail<VkUserInfo>(result.Error);
        }
        catch (Exception e)
        {
            return Result.Fail<VkUserInfo>(e.Message);
        }
    }

    private Result<VkUserInfo> ParseVkInfo(string json)
    {
        try
        {
            var jsonElement = JsonDocument.Parse(json).RootElement.GetProperty("response")[0];
            var id = jsonElement.GetProperty("id").ToString();
            var firstName = jsonElement.GetProperty("first_name").ToString();
            var lastName = jsonElement.GetProperty("last_name").ToString();
            var isBirthday = jsonElement.TryGetProperty("bdate", out var birthday);
            return Result.Ok(new VkUserInfo(id, firstName, lastName,
                isBirthday ? DateOnly.Parse(birthday.ToString(),CultureInfo.GetCultureInfo("ru-RU")) : DateOnly.FromDateTime(DateTime.Today)));
        }
        catch (Exception e)
        {
            return Result.Fail<VkUserInfo>(e.Message);
        }
    }
    
}