namespace TimMovie.WebApi.Tests

open System
open System.Collections.Generic
open System.Linq
open Microsoft.AspNetCore.Identity
open TimMovie.Core.Entities
open TimMovie.Infrastructure.Database

type DatabaseFillerRecommendations() =
    member this.Start(dbContext: ApplicationContext, userManager: UserManager<User>) =
        this.AddRecommendations(dbContext, userManager)

    member private this.AddRecommendations(dbContext: ApplicationContext, userManager: UserManager<User>) =
        let listGenres = List<Genre>()
        listGenres.Add(Genre(Name = "1234"))

        let film =
            Film(
                Title = "RecommendationFilm",
                Country = Country(Name = "Россия"),
                Image = "123",
                Genres = listGenres,
                Year = 2006
            )

        dbContext.Films.Add(film) |> ignore
        dbContext.SaveChanges() |> ignore

        let dbFilm =
            Queryable.First(dbContext.Films, (fun f -> f.Title = film.Title))

        if dbFilm.Producers = null then
            dbFilm.Producers = List<Producer>() |> ignore

        let films = List<Film>()
        films.Add(dbFilm)

        let producer =
            Producer(
                Name = "RecommendationFilmProducerName",
                Surname = "RecommendationFilmProducerSurname",
                Films = films,
                Photo = "12354"
            )

        dbContext.Producers.Add(producer) |> ignore

        let actor =
            Actor(
                Name = "RecommendationFilmActorName",
                Surname = "RecommendationFilmActorSurname",
                Films = films,
                Photo = "12352351"
            )

        dbContext.Actors.Add(actor) |> ignore

        let user =
            userManager.FindByNameAsync(Constants.DefaultUserName)
            |> Async.AwaitTask
            |> Async.RunSynchronously

        dbContext.WatchedFilms.Add(UserFilmWatched(WatchedUser = user, Film = dbFilm, Grade = 10, Date = DateTime.Now))
        |> ignore
