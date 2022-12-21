namespace TimMovie.WebApi.Controllers.MainPageController

open System
open System.Collections.Generic
open Microsoft.AspNetCore.Authorization
open Microsoft.AspNetCore.Mvc
open Microsoft.FSharp.Core
open Newtonsoft.Json
open OpenIddict.Validation.AspNetCore
open TimMovie.Core.DTO
open TimMovie.Core.DTO.Films
open TimMovie.Core.Enums
open TimMovie.Core.Interfaces
open TimMovie.Core.Services.Films
open TimMovie.WebApi.Services.JwtService

[<ApiController>]
[<Route("[controller]/[action]")>]
type MainPageController
    (
        searchEntityService: ISearchEntityService,
        subscribeService: ISubscribeService,
        notificationService: INotificationService,
        filmCardService: FilmCardService,
        watchLaterService: WatchLaterService
    ) as this =
    inherit ControllerBase()

    member private _.jwtService = JwtService()

    [<HttpPost>]
    [<AllowAnonymous>]
    [<Consumes("application/x-www-form-urlencoded")>]
    member _.SearchEntityByNamePart([<FromForm>] namePart: string) =
        searchEntityService.GetSearchEntityResultByNamePart(namePart)

    [<HttpGet>]
    [<Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)>]
    [<Consumes("application/x-www-form-urlencoded")>]
    member _.GetAllUserNotifications() =
        let jwtTokenOption =
            this.jwtService.GetUserJwtToken(this.HttpContext.Request.Headers)

        if jwtTokenOption.IsSome then
            let userGuidOption =
                this.jwtService.GetUserGuid(jwtTokenOption.Value)

            if userGuidOption.IsSome then
                let notifications =
                    notificationService.GetAllUserNotifications(Guid(userGuidOption.Value.ToString()))


                let json =
                    JsonConvert.SerializeObject(notifications, Formatting.Indented)

                ContentResult(Content = json, StatusCode = 200)
            else
                ContentResult(Content = "Error occurred while decoding the jwt token", StatusCode = 400)
        else
            ContentResult(Content = "Error occurred while getting user jwt token", StatusCode = 400)

    [<HttpGet>]
    [<Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)>]
    [<Consumes("application/x-www-form-urlencoded")>]
    member _.GetUserSubscribes() =
        let jwtTokenOption =
            this.jwtService.GetUserJwtToken(this.HttpContext.Request.Headers)

        if jwtTokenOption.IsSome then
            let userGuidOption =
                this.jwtService.GetUserGuid(jwtTokenOption.Value)

            if userGuidOption.IsSome then
                let subscribes =
                    subscribeService.GetAllActiveUserSubscribes(Guid(userGuidOption.Value.ToString()))

                let json =
                    JsonConvert.SerializeObject(subscribes, Formatting.Indented)

                ContentResult(Content = json, StatusCode = 200)
            else
                ContentResult(Content = "Error occurred while decoding the jwt token", StatusCode = 400)
        else
            ContentResult(Content = "Error occurred while getting user jwt token", StatusCode = 400)

    [<HttpPost>]
    [<AllowAnonymous>]
    [<Consumes("application/x-www-form-urlencoded")>]
    member _.GetAllSubscribesByNamePart([<FromForm>] namePart: string) =
        subscribeService.GetSubscribesByNamePart(namePart)

    [<HttpPost>]
    [<AllowAnonymous>]
    [<Consumes("application/x-www-form-urlencoded")>]
    member _.GetSubscribesByNamePartWithPagination
        (
            [<FromForm>] namePart: string,
            [<FromForm>] take: int,
            [<FromForm>] skip: int
        ) =
        subscribeService.GetSubscribesByNamePart(namePart, take, skip)

    [<HttpPost>]
    [<AllowAnonymous>]
    [<Consumes("application/x-www-form-urlencoded")>]
    member _.GetFilmByFilters
        (
            [<FromForm>] filmSortingType: FilmSortingType,
            [<FromForm>] genresName: IEnumerable<string>,
            [<FromForm>] countriesName: IEnumerable<string>,
            [<FromForm>] firstYear: int,
            [<FromForm>] lastYear: int,
            [<FromForm>] rating: int,
            [<FromForm>] isDescending: bool,
            [<FromForm>] amountSkip: int,
            [<FromForm>] amountTake: int
        ) =

        let selectedFilmFiltersDto =
            SelectedFilmFiltersDto(
                SortingType = filmSortingType,
                GenresName = genresName,
                CountriesName = countriesName,
                Rating = rating,
                AnnualPeriod = AnnualPeriodDto(firstYear, lastYear),
                IsDescending = isDescending
            )

        let generalPaginationDto =
            GeneralPaginationDto<SelectedFilmFiltersDto>(
                DataDto = selectedFilmFiltersDto,
                AmountSkip = amountSkip,
                AmountTake = amountTake
            )

        filmCardService.GetFilmCardsByFilters(generalPaginationDto)

    [<HttpPost>]
    [<Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)>]
    [<Consumes("application/x-www-form-urlencoded")>]
    member _.GetWatchLaterFilms([<FromForm>] take: int, [<FromForm>] skip: int) =
        let jwtTokenOption =
            this.jwtService.GetUserJwtToken(this.HttpContext.Request.Headers)

        if jwtTokenOption.IsSome then
            let userGuidOption =
                this.jwtService.GetUserGuid(jwtTokenOption.Value)

            if userGuidOption.IsSome then
                let films =
                    watchLaterService.GetWatchLaterFilms(Guid(userGuidOption.Value.ToString()), take, skip)

                let json =
                    JsonConvert.SerializeObject(films, Formatting.Indented)

                ContentResult(Content = json, StatusCode = 200)
            else
                ContentResult(Content = "Error occurred while decoding the jwt token", StatusCode = 400)
        else
            ContentResult(Content = "Error occurred while getting user jwt token", StatusCode = 400)
