namespace TimMovie.WebApi.Tests.Tests

open System.Collections.Generic
open System.Net
open System.Net.Http
open Newtonsoft.Json
open TimMovie.Core.DTO.Comments
open TimMovie.Core.DTO.Films
open TimMovie.WebApi
open TimMovie.WebApi.Tests
open Xunit

type FilmsTests(factory: BaseApplicationFactory<Program>) =
    interface IClassFixture<BaseApplicationFactory<Program>>

    [<Fact>]
    member this.``Test getting film by id``() =
        let client = factory.CreateClient()
        let data = List<KeyValuePair<string, string>>()
        data.Add(KeyValuePair<string, string>("filmId", Constants.DefaultFilmGuid.ToString()))

        let response =
            client.PostAsync(Constants.GetFilmById, new FormUrlEncodedContent(data))
            |> Async.AwaitTask
            |> Async.RunSynchronously

        Assert.True(response.StatusCode = HttpStatusCode.OK)

        let content =
            response.Content.ReadAsStringAsync()
            |> Async.AwaitTask
            |> Async.RunSynchronously

        let result =
            JsonConvert.DeserializeObject<FilmDto> content

        Assert.True(
            result <> null
            && result.Id = Constants.DefaultFilmGuid
        )

        client.Dispose()

    [<Theory>]
    [<InlineData("1234", true, true)>]
    [<InlineData("1", false, false)>]
    [<InlineData("1234", false, false)>]
    [<InlineData("1234", true, true)>]
    [<InlineData("1234", true, false)>]
    [<InlineData("зелибоба", true, true)>]
    [<InlineData("keyboard", true, true)>]
    [<InlineData("kEyboard3412", true, true)>]
    member this.``Test adding comment to film``(content: string, isSucceeded: bool, isRequestWithJWT: bool) =
        let client = factory.CreateClient()
        let userManager = factory.GetUserManager
        let data = List<KeyValuePair<string, string>>()
        data.Add(KeyValuePair<string, string>("filmId", Constants.DefaultFilmGuid.ToString()))
        data.Add(KeyValuePair<string, string>("content", content))

        if isRequestWithJWT then
            let jwtToken =
                JwtService.GetJwtToken(Constants.DefaultUserName, userManager)

            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwtToken}")

        let response =
            client.PostAsync(Constants.AddCommentToFilm, new FormUrlEncodedContent(data))
            |> Async.AwaitTask
            |> Async.RunSynchronously

        if isSucceeded = false || isRequestWithJWT = false then
            Assert.True(response.StatusCode = HttpStatusCode.BadRequest)
        else
            Assert.True(response.StatusCode = HttpStatusCode.OK)

            let responseContent =
                response.Content.ReadAsStringAsync()
                |> Async.AwaitTask
                |> Async.RunSynchronously

            let result =
                JsonConvert.DeserializeObject<CommentsDto> responseContent

            Assert.True(result <> null && result.Content = content)

        client.Dispose()
