namespace TimMovie.WebApi.Controllers.AccountController

open Microsoft.AspNetCore.Authorization
open Microsoft.AspNetCore.Http
open Microsoft.AspNetCore.Mvc
open TimMovie.Core.DTO.Account
open TimMovie.Core.Interfaces
open TimMovie.SharedKernel.Classes

[<ApiController>]
[<Route("[controller]/[action]")>]
[<System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage>]
type AccountController(userService: IUserService) as this =
    inherit ControllerBase()

    member private _.UrlToConfirmEmail =
        this.Url.Action("ConfirmEmail", "Account", null, this.HttpContext.Request.Scheme)

    [<HttpPost>]
    [<AllowAnonymous>]
    [<Consumes("application/x-www-form-urlencoded")>]
    member _.Register([<FromForm>] email: string, [<FromForm>] username: string, [<FromForm>] password: string) =
        let userRegistrationDto = UserRegistrationDto()
        userRegistrationDto.Email <- email
        userRegistrationDto.UserName <- username
        userRegistrationDto.Password <- password

        let registerResult =
            userService.RegisterUserAsync(userRegistrationDto)
            |> Async.AwaitTask
            |> Async.RunSynchronously

        let result =
            if registerResult.Succeeded then
                userService.SendConfirmEmailAsync(email, this.UrlToConfirmEmail)
                |> Async.AwaitTask
                |> Async.RunSynchronously
            else
                let sb = System.Text.StringBuilder()

                for error in registerResult.Errors do
                    sb.Append($" {error.Description}") |> ignore

                Result.Fail($"{sb.ToString()}")

        result

    [<HttpPost>]
    [<AllowAnonymous>]
    [<Consumes("application/x-www-form-urlencoded")>]
    member _.SendConfirmEmail([<FromForm>] email: string) =
        userService.SendConfirmEmailAsync(email, this.UrlToConfirmEmail)

    [<HttpGet>]
    [<AllowAnonymous>]
    [<ApiExplorerSettings(IgnoreApi = true)>]
    member _.ConfirmEmail(userId: string, code: string) =
        userService.ConfirmEmailAsync(userId, code)

    [<HttpGet>]
    [<AllowAnonymous>]
    [<ApiExplorerSettings(IgnoreApi = true)>]
    member _.GetUrlToConfirmEmail() = this.UrlToConfirmEmail
