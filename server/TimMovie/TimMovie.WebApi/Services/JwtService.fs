namespace TimMovie.WebApi.Services.JwtService

open System.IdentityModel.Tokens.Jwt
open System.Linq

type JwtService() =
    member this.GetUserJwtToken(headers: Microsoft.AspNetCore.Http.IHeaderDictionary) =
        if (headers.ContainsKey("Authorization")
            && headers["Authorization"].Count <> 0) then
            Some(headers["Authorization"])
        else
            None

    member this.GetUserGuid(headerRecord: string) =
        if (headerRecord = null) then
            None
        else
            let parts = headerRecord.Split [| ' ' |]

            if (parts.Length <> 2) then
                None
            else
                let token = parts[1]

                if (token = null) then
                    None
                else
                    let handler = JwtSecurityTokenHandler()
                    let jwtSecurityToken = handler.ReadJwtToken(token)

                    if (jwtSecurityToken = null) then
                        None
                    else
                        Some(
                            Enumerable
                                .FirstOrDefault(
                                    jwtSecurityToken.Claims,
                                    (fun claim -> claim.Type = "sub")
                                )
                                .Value
                        )
