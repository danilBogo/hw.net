namespace TimMovie.WebApi.Tests

open System
open TimMovie.Core.Entities
open TimMovie.Infrastructure.Database

type DatabaseFillerNavbarSearch() =
    member this.Start(dbContext: ApplicationContext) =
        this.AddFilms(dbContext)
        this.AddActors(dbContext)
        this.AddProducers(dbContext)
        this.AddGenres(dbContext)

    member private this.AddFilms(dbContext: ApplicationContext) =
        dbContext.Films.Add(Film(Id = Constants.DefaultFilmGuid, Title = "F1A0P0G0"))
        |> ignore

        dbContext.Films.Add(Film(Title = "F2A0P0G0"))
        |> ignore

        dbContext.Films.Add(Film(Title = "F2A0P0G0"))
        |> ignore

        dbContext.Films.Add(Film(Title = "F3A0P0G0"))
        |> ignore

        dbContext.Films.Add(Film(Title = "F3A0P0G0"))
        |> ignore

        dbContext.Films.Add(Film(Title = "F3A0P0G0"))
        |> ignore

        dbContext.Films.Add(Film(Title = "F4A0P0G0"))
        |> ignore

        dbContext.Films.Add(Film(Title = "F4A0P0G0"))
        |> ignore

        dbContext.Films.Add(Film(Title = "F4A0P0G0"))
        |> ignore

        dbContext.Films.Add(Film(Title = "F4A0P0G0"))
        |> ignore

        dbContext.Films.Add(Film(Title = "F5A0P0G0"))
        |> ignore

        dbContext.Films.Add(Film(Title = "F5A0P0G0"))
        |> ignore

        dbContext.Films.Add(Film(Title = "F5A0P0G0"))
        |> ignore

        dbContext.Films.Add(Film(Title = "F5A0P0G0"))
        |> ignore

        dbContext.Films.Add(Film(Title = "F5A0P0G0"))
        |> ignore

        dbContext.Films.Add(Film(Title = "F1A1P0G0"))
        |> ignore

        dbContext.Films.Add(Film(Title = "F1A1P1G1"))
        |> ignore

        dbContext.Films.Add(Film(Title = "F3A3P3G3"))
        |> ignore

        dbContext.Films.Add(Film(Title = "F3A3P3G3"))
        |> ignore

        dbContext.Films.Add(Film(Title = "F3A3P3G3"))
        |> ignore

        dbContext.Films.Add(Film(Title = "F5A3P3G3"))
        |> ignore

        dbContext.Films.Add(Film(Title = "F5A3P3G3"))
        |> ignore

        dbContext.Films.Add(Film(Title = "F5A3P3G3"))
        |> ignore

        dbContext.Films.Add(Film(Title = "F5A3P3G3"))
        |> ignore

        dbContext.Films.Add(Film(Title = "F5A3P3G3"))
        |> ignore

        dbContext.Films.Add(Film(Title = "Фильм"))
        |> ignore

        dbContext.Films.Add(Film(Title = "фильм"))
        |> ignore

        dbContext.Films.Add(Film(Title = "Film"))
        |> ignore

        dbContext.Films.Add(Film(Title = "film"))
        |> ignore

    member private this.AddActors(dbContext: ApplicationContext) =
        dbContext.Actors.Add(Actor(Name = "F0A1P0G0"))
        |> ignore

        dbContext.Actors.Add(Actor(Name = "F0A2P0G0"))
        |> ignore

        dbContext.Actors.Add(Actor(Name = "F0A2P0G0"))
        |> ignore

        dbContext.Actors.Add(Actor(Name = "F0A3P0G0"))
        |> ignore

        dbContext.Actors.Add(Actor(Name = "F0A3P0G0"))
        |> ignore

        dbContext.Actors.Add(Actor(Name = "F0A3P0G0"))
        |> ignore

        dbContext.Actors.Add(Actor(Name = "F1A1P0G0"))
        |> ignore

        dbContext.Actors.Add(Actor(Name = "F1A1P1G1"))
        |> ignore

        dbContext.Actors.Add(Actor(Name = "F3A3P3G3"))
        |> ignore

        dbContext.Actors.Add(Actor(Name = "F3A3P3G3"))
        |> ignore

        dbContext.Actors.Add(Actor(Name = "F3A3P3G3"))
        |> ignore

        dbContext.Actors.Add(Actor(Name = "F5A3P3G3"))
        |> ignore

        dbContext.Actors.Add(Actor(Name = "F5A3P3G3"))
        |> ignore

        dbContext.Actors.Add(Actor(Name = "F5A3P3G3"))
        |> ignore

        dbContext.Actors.Add(Actor(Name = "Имя", Surname = "Фамилия"))
        |> ignore

        dbContext.Actors.Add(Actor(Name = "имя", Surname = "Фамилия"))
        |> ignore

        dbContext.Actors.Add(Actor(Name = "Имя", Surname = "фамилия"))
        |> ignore

        dbContext.Actors.Add(Actor(Name = "имя", Surname = "фамилия"))
        |> ignore

        dbContext.Actors.Add(Actor(Name = "Name", Surname = "Surname"))
        |> ignore

        dbContext.Actors.Add(Actor(Name = "name", Surname = "Surname"))
        |> ignore

        dbContext.Actors.Add(Actor(Name = "Name", Surname = "surname"))
        |> ignore

        dbContext.Actors.Add(Actor(Name = "name", Surname = "surname"))
        |> ignore



    member private this.AddProducers(dbContext: ApplicationContext) =
        dbContext.Producers.Add(Producer(Name = "F0A0P1G0"))
        |> ignore

        dbContext.Producers.Add(Producer(Name = "F0A0P2G0"))
        |> ignore

        dbContext.Producers.Add(Producer(Name = "F0A0P2G0"))
        |> ignore

        dbContext.Producers.Add(Producer(Name = "F0A0P3G0"))
        |> ignore

        dbContext.Producers.Add(Producer(Name = "F0A0P3G0"))
        |> ignore

        dbContext.Producers.Add(Producer(Name = "F0A0P3G0"))
        |> ignore

        dbContext.Producers.Add(Producer(Name = "F1A1P1G1"))
        |> ignore

        dbContext.Producers.Add(Producer(Name = "F3A3P3G3"))
        |> ignore

        dbContext.Producers.Add(Producer(Name = "F3A3P3G3"))
        |> ignore

        dbContext.Producers.Add(Producer(Name = "F3A3P3G3"))
        |> ignore

        dbContext.Producers.Add(Producer(Name = "F5A3P3G3"))
        |> ignore

        dbContext.Producers.Add(Producer(Name = "F5A3P3G3"))
        |> ignore

        dbContext.Producers.Add(Producer(Name = "F5A3P3G3"))
        |> ignore

        dbContext.Producers.Add(Producer(Name = "Имя", Surname = "Фамилия"))
        |> ignore

        dbContext.Producers.Add(Producer(Name = "имя", Surname = "Фамилия"))
        |> ignore

        dbContext.Producers.Add(Producer(Name = "Имя", Surname = "фамилия"))
        |> ignore

        dbContext.Producers.Add(Producer(Name = "имя", Surname = "фамилия"))
        |> ignore

        dbContext.Producers.Add(Producer(Name = "Name", Surname = "Surname"))
        |> ignore

        dbContext.Producers.Add(Producer(Name = "name", Surname = "Surname"))
        |> ignore

        dbContext.Producers.Add(Producer(Name = "Name", Surname = "surname"))
        |> ignore

        dbContext.Producers.Add(Producer(Name = "name", Surname = "surname"))
        |> ignore

    member private this.AddGenres(dbContext: ApplicationContext) =
        dbContext.Genres.Add(Genre(Name = "F0A0P0G1"))
        |> ignore

        dbContext.Genres.Add(Genre(Name = "F0A0P0G2"))
        |> ignore

        dbContext.Genres.Add(Genre(Name = "F0A0P0G2"))
        |> ignore

        dbContext.Genres.Add(Genre(Name = "F0A0P0G3"))
        |> ignore

        dbContext.Genres.Add(Genre(Name = "F0A0P0G3"))
        |> ignore

        dbContext.Genres.Add(Genre(Name = "F0A0P0G3"))
        |> ignore

        dbContext.Genres.Add(Genre(Name = "F1A1P1G1"))
        |> ignore

        dbContext.Genres.Add(Genre(Name = "F3A3P3G3"))
        |> ignore

        dbContext.Genres.Add(Genre(Name = "F3A3P3G3"))
        |> ignore

        dbContext.Genres.Add(Genre(Name = "F3A3P3G3"))
        |> ignore

        dbContext.Genres.Add(Genre(Name = "F5A3P3G3"))
        |> ignore

        dbContext.Genres.Add(Genre(Name = "F5A3P3G3"))
        |> ignore

        dbContext.Genres.Add(Genre(Name = "F5A3P3G3"))
        |> ignore

        dbContext.Genres.Add(Genre(Name = "Жанр"))
        |> ignore

        dbContext.Genres.Add(Genre(Name = "жанр"))
        |> ignore

        dbContext.Genres.Add(Genre(Name = "Genre"))
        |> ignore

        dbContext.Genres.Add(Genre(Name = "genre"))
        |> ignore
