namespace TimMovie.WebApi.Tests.Tests

open System.Collections.Generic
open System.Linq
open System.Net
open System.Net.Http
open Newtonsoft.Json
open TimMovie.Core.DTO.Films
open TimMovie.Core.Enums
open TimMovie.WebApi.Tests
open Xunit
open TimMovie.WebApi

type FilmFiltersTests(factory: BaseApplicationFactory<Program>) =
    interface IClassFixture<BaseApplicationFactory<Program>>

    [<Theory>]
    [<MemberData("TestProperty")>]
    member this.``Test film filters``
        (
            filmSortingType: FilmSortingType,
            genresName: IEnumerable<string>,
            countriesName: IEnumerable<string>,
            firstYear: int,
            lastYear: int,
            isDescending: bool,
            rating: int,
            amountSkip: int,
            amountTake: int,
            requiredCountFilms: int
        ) =
        let client = factory.CreateClient()

        let data = List<KeyValuePair<string, string>>()
        data.Add(KeyValuePair<string, string>("filmSortingType", filmSortingType.ToString()))

        if genresName <> null then
            for i in genresName do
                data.Add(KeyValuePair<string, string>("genresName", i))

        if countriesName <> null then
            for i in countriesName do
                data.Add(KeyValuePair<string, string>("countriesName", i))

        data.Add(KeyValuePair<string, string>("firstYear", firstYear.ToString()))
        data.Add(KeyValuePair<string, string>("lastYear", lastYear.ToString()))
        data.Add(KeyValuePair<string, string>("isDescending", isDescending.ToString()))
        data.Add(KeyValuePair<string, string>("rating", rating.ToString()))
        data.Add(KeyValuePair<string, string>("amountSkip", amountSkip.ToString()))
        data.Add(KeyValuePair<string, string>("amountTake", amountTake.ToString()))

        task {
            let! response = client.PostAsync(Constants.FilmByFilters, new FormUrlEncodedContent(data))
            client.Dispose()
            Assert.True(response.StatusCode = HttpStatusCode.OK)
            let! content = response.Content.ReadAsStringAsync()

            let result =
                JsonConvert.DeserializeObject<List<FilmCardDto>> content

            if result = null then
                Assert.True(
                    requiredCountFilms = 0
                    || amountSkip <> 0
                    || amountTake = 0
                )
            else
                Assert.True(result.Count = requiredCountFilms)

                if genresName <> null then
                    Assert.True(
                        Enumerable.All(result, (fun element -> Enumerable.Contains(genresName, element.FirstGenreName)))
                    )

                if countriesName <> null then
                    Assert.True(
                        Enumerable.All(result, (fun element -> Enumerable.Contains(countriesName, element.CountryName)))
                    )

                Assert.True(
                    Enumerable.All(
                        result,
                        fun element ->
                            firstYear <= element.Year
                            && element.Year <= lastYear
                    )
                )

                Assert.True(
                    Enumerable.All(
                        result,
                        fun element ->
                            element.Rating.HasValue
                            && element.Rating.Value >= rating
                    )
                )
        }

    static member TestProperty: obj [] list =
        [ [| FilmSortingType.Title
             null
             null
             1900
             2200
             true
             1
             0
             0
             0 |]
          [| FilmSortingType.Title
             null
             null
             1900
             2200
             true
             1
             100
             0
             0 |]
          [| FilmSortingType.Title
             null
             null
             2020
             2019
             true
             1
             100
             100
             0 |]
          [| FilmSortingType.Title
             [| "Комедия" |]
             null
             1900
             2200
             true
             1
             0
             100
             1 |]
          [| FilmSortingType.Title
             [| "Комедия" |]
             null
             1900
             2200
             false
             1
             0
             100
             1 |]
          [| FilmSortingType.Title
             [| "Комедия" |]
             null
             1900
             2200
             true
             9
             0
             100
             1 |]
          [| FilmSortingType.Title
             [| "Комедия" |]
             null
             1900
             2200
             false
             9
             0
             100
             1 |]
          [| FilmSortingType.Title
             [| "Комедия" |]
             null
             2010
             2010
             true
             9
             0
             100
             1 |]
          [| FilmSortingType.Title
             [| "Драма" |]
             null
             1998
             1998
             true
             5
             0
             100
             1 |]
          [| FilmSortingType.Title
             [| "Драма" |]
             null
             1998
             1998
             false
             5
             0
             100
             1 |]
          [| FilmSortingType.Title
             [| "По комиксам"; "Драма" |]
             null
             1998
             1998
             true
             5
             0
             100
             1 |]
          [| FilmSortingType.Title
             [| "По комиксам"; "Драма" |]
             null
             1998
             1998
             false
             5
             0
             100
             1 |]
          [| FilmSortingType.Title
             [| "По комиксам"; "Драма" |]
             [| "Россия" |]
             1998
             1998
             false
             10
             0
             100
             0 |]
          [| FilmSortingType.Title
             [| "По комиксам"; "Драма" |]
             [| "Россия" |]
             1998
             1998
             false
             5
             0
             100
             1 |]
          [| FilmSortingType.Title
             [| "По комиксам"; "Драма" |]
             [| "Россия" |]
             1998
             1998
             true
             10
             0
             100
             0 |]
          [| FilmSortingType.Title
             [| "По комиксам"; "Драма" |]
             [| "Россия" |]
             1998
             1998
             true
             5
             0
             100
             1 |]
          [| FilmSortingType.Title
             [| "По комиксам"; "Драма" |]
             [| "Узбекистан" |]
             1998
             1998
             false
             10
             0
             100
             0 |]
          [| FilmSortingType.Title
             [| "Комедия"; "Боевик"; "Триллер" |]
             null
             1900
             2200
             true
             1
             0
             100
             0 |] ]
