namespace TimMovie.Auth.Controllers

open Microsoft.AspNetCore.Authorization
open Microsoft.AspNetCore.Mvc
open OpenIddict.Validation.AspNetCore

type CheckController () =
    inherit Controller()

    [<HttpGet("check")>]
    [<Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)>]
    member _.Get():StatusCodeResult =
        OkResult()