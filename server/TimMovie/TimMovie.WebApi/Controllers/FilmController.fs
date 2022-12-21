namespace TimMovie.WebApi.Controllers.FilmController

open System
open Microsoft.AspNetCore.Authorization
open Microsoft.AspNetCore.Mvc
open Microsoft.FSharp.Core
open Newtonsoft.Json
open OpenIddict.Validation.AspNetCore
open TimMovie.Core.Services.Films
open TimMovie.WebApi.Services.JwtService

[<ApiController>]
[<Route("[controller]/[action]")>]
type FilmController(filmService: FilmService, filmCardService: FilmCardService) as this =
    inherit ControllerBase()

    member private _.jwtService = JwtService()

    [<HttpPost>]
    [<AllowAnonymous>]
    [<Consumes("application/x-www-form-urlencoded")>]
    member _.GetFilmById([<FromForm>] filmId: Guid) = filmService.GetFilmById(filmId)

    [<HttpPost>]
    [<Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)>]
    [<Consumes("application/x-www-form-urlencoded")>]
    member _.AddCommentToFilm([<FromForm>] filmId: Guid, [<FromForm>] content: string) =
        let jwtTokenOption =
            this.jwtService.GetUserJwtToken(this.HttpContext.Request.Headers)

        if jwtTokenOption.IsSome then
            let userGuidOption =
                this.jwtService.GetUserGuid(jwtTokenOption.Value)

            if userGuidOption.IsSome then
                let result =
                    filmService.TryAddCommentToFilm(Guid(userGuidOption.Value.ToString()), filmId, content)
                    |> Async.AwaitTask
                    |> Async.RunSynchronously

                if result.Succeeded then
                    let json =
                        JsonConvert.SerializeObject(result.Value, Formatting.Indented)

                    ContentResult(Content = json, StatusCode = 200)
                else
                    let json =
                        JsonConvert.SerializeObject(result.Error, Formatting.Indented)

                    ContentResult(Content = json, StatusCode = 400)
            else
                ContentResult(Content = "Error occurred while decoding the jwt token", StatusCode = 400)
        else
            ContentResult(Content = "Error occurred while getting user jwt token", StatusCode = 400)

    [<HttpPost>]
    [<Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)>]
    [<Consumes("application/x-www-form-urlencoded")>]
    member _.GetFilmRecommendations([<FromForm>] amount: int) =
        let jwtTokenOption =
            this.jwtService.GetUserJwtToken(this.HttpContext.Request.Headers)

        if jwtTokenOption.IsSome then
            let userGuidOption =
                this.jwtService.GetUserGuid(jwtTokenOption.Value)

            if userGuidOption.IsSome then
                let result =
                    filmCardService.GetFilmRecommendationsByUserId(Guid(userGuidOption.Value.ToString()), amount)

                if result.Succeeded then
                    let json =
                        JsonConvert.SerializeObject(result.Value, Formatting.Indented)

                    ContentResult(Content = json, StatusCode = 200)
                else
                    let json =
                        JsonConvert.SerializeObject(result.Error, Formatting.Indented)

                    ContentResult(Content = json, StatusCode = 400)
            else
                ContentResult(Content = "Error occurred while decoding the jwt token", StatusCode = 400)
        else
            ContentResult(Content = "Error occurred while getting user jwt token", StatusCode = 400)
