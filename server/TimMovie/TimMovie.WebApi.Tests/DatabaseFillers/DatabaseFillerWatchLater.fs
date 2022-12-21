namespace TimMovie.WebApi.Tests

open System
open System.Collections.Generic
open System.Linq
open Microsoft.AspNetCore.Identity
open TimMovie.Core.Entities
open TimMovie.Infrastructure.Database

type DatabaseFillerWatchLater() =
    member this.Start(dbContext: ApplicationContext, userManager: UserManager<User>) =
        this.AddWatchLater(dbContext, userManager)

    member private this.AddWatchLater(dbContext: ApplicationContext, userManager: UserManager<User>) =
        let user =
            userManager.FindByNameAsync(Constants.DefaultUserName)
            |> Async.AwaitTask
            |> Async.RunSynchronously

        let dbFilm =
            Queryable.FirstOrDefault(dbContext.Films, (fun f -> f.Title = "RecommendationFilm"))

        if dbFilm.Comments = null then
            dbFilm.Comments <- List<Comment>()

        dbFilm.Comments.Add(Comment(Author = user, Film = dbFilm, Content = "13252", Date = DateTime.Now))
        dbFilm.IsFree <- true
        dbFilm.Description <- "132532"
        dbFilm.FilmLink <- "123424"
        dbContext.Update(dbFilm) |> ignore

        if user.FilmsWatchLater = null then
            user.FilmsWatchLater <- List<Film>()

        user.FilmsWatchLater.Add(dbFilm)

        userManager.UpdateAsync(user)
        |> Async.AwaitTask
        |> Async.RunSynchronously
