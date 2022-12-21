namespace TimMovie.WebApi.Tests

open System.Collections.Generic
open System.Security.Claims
open Microsoft.AspNetCore.Identity
open OpenIddict.Abstractions
open TimMovie.Core.Entities

type JwtService() =
    static member GetJwtToken(userName: string, userManager: UserManager<User>) =
        let dbUser =
            userManager.FindByNameAsync(userName)
            |> Async.AwaitTask
            |> Async.RunSynchronously

        let claimId =
            Claim(OpenIddictConstants.Claims.Subject, dbUser.Id.ToString())

        let claims = List<Claim>()
        claims.Add(claimId)
        MockJwtTokens.GenerateJwtToken(claims)
