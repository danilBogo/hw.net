namespace TimMovie.WebApi.Tests

open System.Collections.Generic
open System.Net.Http
open System.Security.Claims
open System.Threading.Tasks
open Microsoft.AspNetCore.Authentication
open Microsoft.AspNetCore.Authorization.Policy

type FakePolicyEvaluator() =
    interface IPolicyEvaluator with
        member this.AuthenticateAsync(policy, context) =
            let testScheme = "FakeScheme"
            let principal = ClaimsPrincipal()
            let claimsList = List<Claim>()
            claimsList.Add(Claim("Permission", "CanViewPage"))
            claimsList.Add(Claim("Manager", "yes"))
            claimsList.Add(Claim(ClaimTypes.Role, "Administrator"))
            claimsList.Add(Claim(ClaimTypes.NameIdentifier, "John"))
            let claimsIdentity = ClaimsIdentity(claimsList, testScheme)
            principal.AddIdentity(claimsIdentity)

            Task.FromResult(
                AuthenticateResult.Success(AuthenticationTicket(principal, AuthenticationProperties(), testScheme))
            )

        member this.AuthorizeAsync(policy, authenticationResult, context, resource) =
            Task.FromResult(PolicyAuthorizationResult.Success())
