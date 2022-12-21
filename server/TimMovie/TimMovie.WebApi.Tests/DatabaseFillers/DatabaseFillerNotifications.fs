namespace TimMovie.WebApi.Tests

open System.Collections.Generic
open Microsoft.AspNetCore.Identity
open TimMovie.Core.Entities
open TimMovie.Infrastructure.Database

type DatabaseFillerNotifications() =
    member this.Start(dbContext: ApplicationContext, userManager: UserManager<User>) =
        this.AddNotifications(dbContext, userManager)

    member private this.AddNotifications(dbContext: ApplicationContext, userManager: UserManager<User>) =
        let user =
            userManager.FindByNameAsync(Constants.DefaultUserName)
            |> Async.AwaitTask
            |> Async.RunSynchronously

        let users = List<User>()
        users.Add(user)

        dbContext.Notifications.Add(Notification(Content = "Notification1", Users = users))
        |> ignore

        dbContext.Notifications.Add(Notification(Content = "Notification2", Users = users))
        |> ignore
        
        
