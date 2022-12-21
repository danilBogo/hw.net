namespace TimMovie.WebApi.Tests.Tests

open System.Collections.Generic
open System.Net
open System.Net.Http
open Newtonsoft.Json
open TimMovie.Core.DTO
open TimMovie.WebApi.Tests
open Xunit
open TimMovie.WebApi

type SearchTests(factory: BaseApplicationFactory<Program>) =
    interface IClassFixture<BaseApplicationFactory<Program>>

    [<Theory>]
    [<MemberData("TestProperty")>]
    member this.``Test search``
        (
            value: string,
            filmsCount: int,
            actorsCount: int,
            producersCount: int,
            genresCount: int
        ) =
        let client = factory.CreateClient()
        let data = List<KeyValuePair<string, string>>()
        data.Add(KeyValuePair<string, string>("namePart", value))

        task {
            let! response = client.PostAsync(Constants.NavbarSearch, new FormUrlEncodedContent(data))
            client.Dispose()
            Assert.True(response.StatusCode = HttpStatusCode.OK)
            let! content = response.Content.ReadAsStringAsync()

            let result =
                JsonConvert.DeserializeObject<SearchEntityResultDto> content

            Assert.True(
                result <> null
                && result <> SearchEntityResultDto()
                && result.Films <> null
                && result.Films |> Seq.length = filmsCount
                && result.Actors <> null
                && result.Actors |> Seq.length = actorsCount
                && result.Producers <> null
                && result.Producers |> Seq.length = producersCount
                && result.Genres <> null
                && result.Genres |> Seq.length = genresCount
            )
        }

    static member TestProperty: obj [] list =
        [ [| "F1A0P0G0"; 1; 0; 0; 0 |]
          [| "F2A0P0G0"; 2; 0; 0; 0 |]
          [| "F3A0P0G0"; 3; 0; 0; 0 |]
          [| "F4A0P0G0"; 4; 0; 0; 0 |]
          [| "F5A0P0G0"; 4; 0; 0; 0 |]
          [| "F1A1P0G0"; 1; 1; 0; 0 |]
          [| "F1A1P0G0"; 1; 1; 0; 0 |]
          [| "F1A1P1G1"; 1; 1; 1; 1 |]
          [| "F3A3P3G3"; 3; 2; 2; 2 |]
          [| "F5A3P3G3"; 4; 2; 2; 2 |]
          [| "F0A1P0G0"; 0; 1; 0; 0 |]
          [| "F0A2P0G0"; 0; 2; 0; 0 |]
          [| "F0A3P0G0"; 0; 2; 0; 0 |]
          [| "F0A0P1G0"; 0; 0; 1; 0 |]
          [| "F0A0P2G0"; 0; 0; 2; 0 |]
          [| "F0A0P3G0"; 0; 0; 2; 0 |]
          [| "F0A0P0G1"; 0; 0; 0; 1 |]
          [| "F0A0P0G2"; 0; 0; 0; 2 |]
          [| "F0A0P0G3"; 0; 0; 0; 2 |]
          [| "F1"; 3; 2; 1; 1 |]
          [| "Фильм"; 2; 0; 0; 0 |]
          [| "фильм"; 2; 0; 0; 0 |]
          [| "Film"; 3; 1; 1; 0 |]
          [| "film"; 3; 1; 1; 0 |]
          [| "Имя Фамилия"; 0; 2; 2; 0 |]
          [| "имя Фамилия"; 0; 2; 2; 0 |]
          [| "Имя фамилия"; 0; 2; 2; 0 |]
          [| "имя фамилия"; 0; 2; 2; 0 |]
          [| "имяфамилия"; 0; 0; 0; 0 |]
          [| "Name Surname"; 0; 2; 2; 0 |]
          [| "name Surname"; 0; 2; 2; 0 |]
          [| "Name surname"; 0; 2; 2; 0 |]
          [| "name surname"; 0; 2; 2; 0 |]
          [| "namesurname"; 0; 0; 0; 0 |]
          [| "Жанр"; 0; 0; 0; 2 |]
          [| "жанр"; 0; 0; 0; 2 |]
          [| "Genre"; 0; 0; 0; 2 |]
          [| "genre"; 0; 0; 0; 2 |] ]
