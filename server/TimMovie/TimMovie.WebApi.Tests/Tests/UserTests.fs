namespace TimMovie.WebApi.Tests.Tests

open System.Collections.Generic
open System.Net
open System.Net.Http
open Newtonsoft.Json
open TimMovie.Core.DTO.Films
open TimMovie.Core.DTO.Notifications
open TimMovie.Core.DTO.Users
open TimMovie.WebApi
open TimMovie.WebApi.Tests
open Xunit

type UserTests(factory: BaseApplicationFactory<Program>) =
    interface IClassFixture<BaseApplicationFactory<Program>>

    [<Theory>]
    [<InlineData(true)>]
    [<InlineData(false)>]
    member this.``Test getting user information``(isRequestWithJWT: bool) =
        let client = factory.CreateClient()
        let userManager = factory.GetUserManager

        if isRequestWithJWT then
            let jwtToken =
                JwtService.GetJwtToken(Constants.DefaultUserName, userManager)

            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwtToken}")

        let response =
            client.GetAsync(Constants.UserInformation)
            |> Async.AwaitTask
            |> Async.RunSynchronously

        if isRequestWithJWT = false then
            Assert.True(response.StatusCode = HttpStatusCode.BadRequest)
        else
            Assert.True(response.StatusCode = HttpStatusCode.OK)

            let responseContent =
                response.Content.ReadAsStringAsync()
                |> Async.AwaitTask
                |> Async.RunSynchronously

            let result =
                JsonConvert.DeserializeObject<UserInfoDto> responseContent

            Assert.True(
                result <> null
                && result.DisplayName = Constants.DefaultDisplayName
                && result.PathToPhoto = Constants.DefaultPathToPhoto
            )

        client.Dispose()

    [<Theory>]
    [<InlineData(true)>]
    [<InlineData(false)>]
    member this.``Test pay for subscription``(isRequestWithJWT: bool) =
        let client = factory.CreateClient()
        let userManager = factory.GetUserManager

        let data = List<KeyValuePair<string, string>>()
        data.Add(KeyValuePair<string, string>("subscribeId", Constants.DefaultSubscribeForPaymentGuid.ToString()))
        data.Add(KeyValuePair<string, string>("cardNumber", "1234567898765432"))
        data.Add(KeyValuePair<string, string>("ccid", "123"))
        data.Add(KeyValuePair<string, string>("expirationMonth", "10"))
        data.Add(KeyValuePair<string, string>("expirationYear", "2023"))

        if isRequestWithJWT then
            let jwtToken =
                JwtService.GetJwtToken(Constants.DefaultUserName, userManager)

            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwtToken}")

        let response =
            client.PostAsync(Constants.PayForSubscription, new FormUrlEncodedContent(data))
            |> Async.AwaitTask
            |> Async.RunSynchronously

        if isRequestWithJWT = false then
            Assert.True(response.StatusCode = HttpStatusCode.BadRequest)
        else
            let responseContent =
                response.Content.ReadAsStringAsync()
                |> Async.AwaitTask
                |> Async.RunSynchronously

            if responseContent.Trim() = "" then
                Assert.True(response.StatusCode = HttpStatusCode.OK)
            else
                Assert.True(responseContent.Contains("банк отклонил вашу покупку"))

        client.Dispose()

    [<Theory>]
    [<InlineData(true, 100, 0, 1)>]
    [<InlineData(true, 0, 0, 0)>]
    [<InlineData(false, 0, 0, 0)>]
    member this.``Test getting watch later films``(isRequestWithJWT: bool, take: int, skip: int, requiredCount: int) =
        let client = factory.CreateClient()
        let userManager = factory.GetUserManager

        let data = List<KeyValuePair<string, string>>()
        data.Add(KeyValuePair<string, string>("take", take.ToString()))
        data.Add(KeyValuePair<string, string>("skip", skip.ToString()))

        if isRequestWithJWT then
            let jwtToken =
                JwtService.GetJwtToken(Constants.DefaultUserName, userManager)

            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwtToken}")

        let response =
            client.PostAsync(Constants.WatchLaterFilms, new FormUrlEncodedContent(data))
            |> Async.AwaitTask
            |> Async.RunSynchronously

        if isRequestWithJWT = false then
            Assert.True(response.StatusCode = HttpStatusCode.BadRequest)
        else
            Assert.True(response.StatusCode = HttpStatusCode.OK)

            let responseContent =
                response.Content.ReadAsStringAsync()
                |> Async.AwaitTask
                |> Async.RunSynchronously

            let result =
                JsonConvert.DeserializeObject<List<BigFilmCardDto>> responseContent

            Assert.True(result <> null && result.Count = requiredCount)

        client.Dispose()
