namespace TimMovie.WebApi.Tests

open System
open System.Collections.Generic
open System.IdentityModel.Tokens.Jwt
open System.Security.Claims
open System.Security.Cryptography
open Microsoft.IdentityModel.Tokens

type MockJwtTokens() =
    static let mutable issuer = Guid.NewGuid().ToString()
    static let mutable securityKey = Unchecked.defaultof<SecurityKey>
    static let mutable signingCredentials = Unchecked.defaultof<SigningCredentials>
    static let s_tokenHandler = JwtSecurityTokenHandler()
    static let s_rng = RandomNumberGenerator.Create()
    static let s_key = Array.zeroCreate 32

    do
        s_rng.GetBytes(s_key)
        securityKey <- SymmetricSecurityKey(s_key, KeyId = Guid.NewGuid().ToString())
        signingCredentials <- SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256)

    member this.GetIssuer = issuer
    member this.SecurityKey = securityKey
    member this.GetSigningCredentials = securityKey

    static member GenerateJwtToken(claims: IEnumerable<Claim>) =
        s_tokenHandler.WriteToken(
            JwtSecurityToken(issuer, null, claims, DateTime(), DateTime.UtcNow.AddMinutes(20), signingCredentials)
        )
