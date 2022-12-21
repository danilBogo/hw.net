namespace TimMovie.Auth.Controllers

open System.Collections.Generic
open System.Security.Claims
open Microsoft.AspNetCore.Authentication
open Microsoft.AspNetCore.Cors
open Microsoft.AspNetCore.Identity
open Microsoft.AspNetCore.Mvc
open OpenIddict.Abstractions
open OpenIddict.Server.AspNetCore

open type OpenIddictConstants.Permissions
open type OpenIddictServerAspNetCoreConstants

open TimMovie.Core.Entities
open Microsoft.AspNetCore


[<ApiController>]
[<Route("connect")>]
type AuthController(userManager: UserManager<User>, signInManager: SignInManager<User>) =
    inherit Controller()

    member private this.GetAuthPropertiesErrors() =
        let errors = Dictionary<string, string>()
        errors.Add(Properties.Error, OpenIddictConstants.Errors.InvalidGrant)
        errors.Add(Properties.ErrorDescription, "The username/password couple is invalid.")
        let properties = AuthenticationProperties(errors)
        properties

    member private this.GetUnconfirmedEmailException() =
        let errors = Dictionary<string, string>()
        errors.Add(Properties.Error, OpenIddictConstants.Errors.AccessDenied)
        errors.Add(Properties.ErrorDescription, "email for this user is not confirmed")
        let properties = AuthenticationProperties(errors)
        properties
    
    member private this.GetDestinations(principal: ClaimsPrincipal, claim: Claim) =
        seq {
            match claim.Type with
            | OpenIddictConstants.Claims.Name ->
                yield OpenIddictConstants.Destinations.AccessToken

                if principal.HasScope(Scopes.Profile) then
                    yield OpenIddictConstants.Destinations.IdentityToken
            | OpenIddictConstants.Claims.Role ->
                yield OpenIddictConstants.Destinations.AccessToken

                if principal.HasScope(Scopes.Roles) then
                    yield OpenIddictConstants.Destinations.IdentityToken
            | OpenIddictConstants.Claims.Email ->
                yield OpenIddictConstants.Destinations.AccessToken

                if principal.HasScope(Scopes.Email) then
                    yield OpenIddictConstants.Destinations.IdentityToken
            | "AspNet.Identity.SecurityStamp" -> "" |> ignore
            | _ -> yield OpenIddictConstants.Destinations.AccessToken
        }


    [<HttpPost("token"); Consumes("application/x-www-form-urlencoded") ;Produces("application/json")>]
    member this.Exchange([<FromForm>]username: string,
                         [<FromForm>]password: string,
                         [<FromForm>]grant_type: string) : IActionResult =
        let request =
            this.HttpContext.GetOpenIddictServerRequest()

        if request.IsPasswordGrantType() then
            let user =
                userManager.FindByNameAsync(request.Username)
                |> Async.AwaitTask
                |> Async.RunSynchronously

            if user = null then
                this.Forbid(this.GetAuthPropertiesErrors(), OpenIddictServerAspNetCoreDefaults.AuthenticationScheme)

            else
                let result =
                    signInManager.CheckPasswordSignInAsync(user, request.Password, true)
                    |> Async.AwaitTask
                    |> Async.RunSynchronously

                if result.Succeeded then
                    let principal =
                        signInManager.CreateUserPrincipalAsync(user)
                        |> Async.AwaitTask
                        |> Async.RunSynchronously

                    principal.SetScopes(
                        Set.intersect
                            (set [ OpenIddictConstants.Permissions.Scopes.Email
                                   OpenIddictConstants.Permissions.Scopes.Profile
                                   OpenIddictConstants.Permissions.Scopes.Roles ])
                            (set (request.GetScopes() |> List.ofSeq))
                    )
                    |> ignore
                    if System.String.IsNullOrEmpty(principal.FindFirstValue(OpenIddictConstants.Claims.Subject)) then
                        principal.SetClaim(OpenIddictConstants.Claims.Subject, user.Id |> string)
                        |> ignore

                    for claim in principal.Claims do
                        claim.SetDestinations(this.GetDestinations(principal, claim))
                        |> ignore

                    this.SignIn(principal, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme)
                
                
                else
                    if result.IsNotAllowed && userManager.CheckPasswordAsync(user,request.Password)
                                              |> Async.AwaitTask
                                              |> Async.RunSynchronously
                                              then
                        this.Forbid(this.GetUnconfirmedEmailException(), OpenIddictServerAspNetCoreDefaults.AuthenticationScheme)
                    else
                        this.Forbid(this.GetAuthPropertiesErrors(), OpenIddictServerAspNetCoreDefaults.AuthenticationScheme)

        else
            this.BadRequest()