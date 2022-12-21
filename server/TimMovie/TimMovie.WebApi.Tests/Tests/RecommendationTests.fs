namespace TimMovie.WebApi.Tests.Tests

open System.Collections.Generic
open System.Net
open System.Net.Http
open Newtonsoft.Json
open TimMovie.Core.DTO.Films
open TimMovie.Core.DTO.Notifications
open TimMovie.WebApi.Tests
open Xunit
open TimMovie.WebApi

type RecommendationTests(factory: BaseApplicationFactory<Program>) =
    interface IClassFixture<BaseApplicationFactory<Program>>

    [<Theory>]
    [<InlineData(true, 10)>]
    [<InlineData(false, 10)>]
    [<InlineData(true, 1)>]
    [<InlineData(false, 1)>]
    member this.``Test recommendations``(isRequestWithJWT: bool, amount : int) =
        let client = factory.CreateClient()
        let userManager = factory.GetUserManager

        let data = List<KeyValuePair<string, string>>()
        data.Add(KeyValuePair<string, string>("amount", amount.ToString()))
        
        if isRequestWithJWT then
            let jwtToken =
                JwtService.GetJwtToken(Constants.DefaultUserName, userManager)

            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwtToken}")

        let response =
            client.PostAsync(Constants.FilmRecommendations, new FormUrlEncodedContent(data))
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
                JsonConvert.DeserializeObject<List<FilmCardDto>> responseContent

            Assert.True(result <> null)

        client.Dispose()
