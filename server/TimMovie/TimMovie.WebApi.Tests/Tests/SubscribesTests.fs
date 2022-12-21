namespace TimMovie.WebApi.Tests.Tests

open System.Collections.Generic
open System.Net
open System.Net.Http
open Newtonsoft.Json
open TimMovie.Core.DTO.Subscribes
open TimMovie.WebApi
open TimMovie.WebApi.Tests
open Xunit

type SubscribesTests(factory: BaseApplicationFactory<Program>) =
    interface IClassFixture<BaseApplicationFactory<Program>>

    [<Theory>]
    [<InlineData(true)>]
    [<InlineData(false)>]
    member this.``Test getting all user notifications``(isRequestWithJWT: bool) =
        let client = factory.CreateClient()
        let userManager = factory.GetUserManager

        if isRequestWithJWT then
            let jwtToken =
                JwtService.GetJwtToken(Constants.DefaultUserName, userManager)

            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwtToken}")

        let response =
            client.GetAsync(Constants.AllUserSubscribes)
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
                JsonConvert.DeserializeObject<List<UserSubscribeDto>> responseContent

            Assert.True(result <> null && result.Count = 1)

        client.Dispose()

    [<Theory>]
    [<InlineData("kek", 0)>]
    [<InlineData("TestSubscribe2", 1)>]
    [<InlineData("TestSubscribe", 2)>]
    [<InlineData("testSubscribe", 2)>]
    [<InlineData("testsubscribe", 2)>]
    member this.``Test getting subscribes by name part``(namePart: string, requiredCount: int) =
        let client = factory.CreateClient()
        let data = List<KeyValuePair<string, string>>()
        data.Add(KeyValuePair<string, string>("namePart", namePart))

        let response =
            client.PostAsync(Constants.AllSubscribesByNamePart, new FormUrlEncodedContent(data))
            |> Async.AwaitTask
            |> Async.RunSynchronously

        Assert.True(response.StatusCode = HttpStatusCode.OK)

        let content =
            response.Content.ReadAsStringAsync()
            |> Async.AwaitTask
            |> Async.RunSynchronously

        let result =
            JsonConvert.DeserializeObject<List<SubscribeDto>> content

        Assert.True(result <> null && result.Count = requiredCount)

        client.Dispose()

    [<Theory>]
    [<InlineData("kek", 0, 1, 0)>]
    [<InlineData("kek", 0, 0, 0)>]
    [<InlineData("TestSubscribe2", 1, 1, 0)>]
    [<InlineData("TestSubscribe2", 0, 1, 1)>]
    [<InlineData("TestSubscribe2", 0, 0, 1)>]
    [<InlineData("TestSubscribe", 2, 2, 0)>]
    [<InlineData("TestSubscribe", 1, 2, 1)>]
    [<InlineData("TestSubscribe", 0, 2, 2)>]
    [<InlineData("TestSubscribe", 1, 1, 1)>]
    member this.``Test getting subscribes by name part with pagination``
        (
            namePart: string,
            requiredCount: int,
            take: int,
            skip: int
        ) =
        let client = factory.CreateClient()
        let data = List<KeyValuePair<string, string>>()
        data.Add(KeyValuePair<string, string>("namePart", namePart))
        data.Add(KeyValuePair<string, string>("take", take.ToString()))
        data.Add(KeyValuePair<string, string>("skip", skip.ToString()))

        let response =
            client.PostAsync(Constants.AllSubscribesByNamePartWithPagination, new FormUrlEncodedContent(data))
            |> Async.AwaitTask
            |> Async.RunSynchronously

        Assert.True(response.StatusCode = HttpStatusCode.OK)

        let content =
            response.Content.ReadAsStringAsync()
            |> Async.AwaitTask
            |> Async.RunSynchronously

        let result =
            JsonConvert.DeserializeObject<List<SubscribeDto>> content

        Assert.True(result <> null && result.Count = requiredCount)

        client.Dispose()
