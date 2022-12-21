module TimMovie.WebApi.Helpers.HttpClient

open System.Collections.Generic
open System.Net
open System.Net.Http

type HttpHelper public () =
    static member GetToken(url: string, username: string, password: string) =
        async {
            let httpClient = new HttpClient()
            let parameters = Dictionary<string, string>()
            parameters.Add("username", username)
            parameters.Add("password", password)
            parameters.Add("grant_type", "password")

            let! response =
                httpClient.PostAsync(url, new FormUrlEncodedContent(parameters))
                |> Async.AwaitTask

            let mutable result = "invalid logic or password"

            if response.StatusCode = HttpStatusCode.OK then
                let! content =
                    response.Content.ReadAsStringAsync()
                    |> Async.AwaitTask
                result <- content.Split("\"")[3]

            return result
        }
