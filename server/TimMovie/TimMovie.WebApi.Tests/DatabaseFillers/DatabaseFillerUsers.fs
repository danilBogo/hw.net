namespace TimMovie.WebApi.Tests

open Microsoft.AspNetCore.Identity
open TimMovie.Core.Entities

type DatabaseFillerUsers() =
    member this.Start(userManager: UserManager<User>) = this.AddUsers(userManager)

    member private this.AddUsers(userManager: UserManager<User>) =
        let user =
            User(
                DisplayName = Constants.DefaultDisplayName,
                UserName = Constants.DefaultUserName,
                Email = Constants.DefaultEmail,
                PathToPhoto = Constants.DefaultPathToPhoto
            )

        userManager.CreateAsync(user, Constants.DefaultPassword)
        |> Async.AwaitTask
        |> Async.RunSynchronously
        |> ignore
