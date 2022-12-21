namespace TimMovie.WebApi.Tests.Tests

open System.Net
open Newtonsoft.Json
open TimMovie.Core.DTO.Notifications
open TimMovie.WebApi.Tests
open Xunit
open TimMovie.WebApi

type NotificationsTests(factory: BaseApplicationFactory<Program>) =
    interface IClassFixture<BaseApplicationFactory<Program>>

    [<Theory>]
    [<InlineData(true)>]
    [<InlineData(false)>]
    member this.``Test notifications``(isRequestWithJWT: bool) =
        let client = factory.CreateClient()
        let userManager = factory.GetUserManager

        if isRequestWithJWT then
            let jwtToken =
                JwtService.GetJwtToken(Constants.DefaultUserName, userManager)

            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwtToken}")

        let response =
            client.GetAsync(Constants.Notifications)
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
                JsonConvert.DeserializeObject<List<NotificationDto>> responseContent

            Assert.True(result.Length = 2)

        client.Dispose()
