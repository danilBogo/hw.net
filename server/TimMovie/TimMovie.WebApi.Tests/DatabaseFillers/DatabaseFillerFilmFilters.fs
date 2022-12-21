namespace TimMovie.WebApi.Tests

open System
open System.Collections.Generic
open Microsoft.AspNetCore.Identity
open TimMovie.Core.Entities
open TimMovie.Infrastructure.Database

type DatabaseFillerFilmFilters() =
    member this.Start(dbContext: ApplicationContext, userManager: UserManager<User>) =
        this.AddFilms(dbContext, userManager)

    member private this.AddFilms(dbContext: ApplicationContext, userManager: UserManager<User>) =
        let genres1 = List<Genre>()
        let genre = Genre(Name = "Комедия")
        genres1.Add(genre)
        dbContext.Genres.Add(genre) |> ignore
        let genre = Genre(Name = "Боевик")
        genres1.Add(genre)
        dbContext.Genres.Add(genre) |> ignore

        let genres2 = List<Genre>()
        let genre = Genre(Name = "Драма")
        genres2.Add(genre)
        dbContext.Genres.Add(genre) |> ignore
        let genre = Genre(Name = "По комиксам")
        genres2.Add(genre)
        dbContext.Genres.Add(genre) |> ignore

        let genres3 = List<Genre>()
        let genre = Genre(Name = "Триллер")
        genres3.Add(genre)
        dbContext.Genres.Add(genre) |> ignore

        let film1 =
            Film(Title = "FilterFi1", Year = 2010, Genres = genres1, Country = Country(Name = "Казахстан"))

        let film2 =
            Film(Title = "FilterFi2", Year = 1998, Genres = genres2, Country = Country(Name = "Россия"))

        let film3 =
            Film(Title = "FilterFi3", Year = 2020, Genres = genres3, Country = Country(Name = "Узбекистан"))

        dbContext.Films.Add(film1) |> ignore
        dbContext.Films.Add(film2) |> ignore
        dbContext.Films.Add(film3) |> ignore

        task {
            let! user = userManager.FindByNameAsync(Constants.DefaultUserName)

            if user.WatchedFilms = null then
                user.WatchedFilms <- List<UserFilmWatched>()

            user.WatchedFilms.Add(UserFilmWatched(WatchedUser = user, Film = film1, Grade = 9, Date = DateTime.Now))
            user.WatchedFilms.Add(UserFilmWatched(WatchedUser = user, Film = film2, Grade = 5, Date = DateTime.Now))
            user.WatchedFilms.Add(UserFilmWatched(WatchedUser = user, Film = film2, Grade = 7, Date = DateTime.Now))
            let! _ = userManager.UpdateAsync(user)
            ()
        }
        |> ignore
