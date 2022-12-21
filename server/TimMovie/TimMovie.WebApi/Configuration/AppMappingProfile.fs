module TimMovie.WebApi.Configuration.AppMappingProfile

open AutoMapper
open TimMovie.Core.DTO.Account
open TimMovie.Core.DTO.Films
open TimMovie.Core.Entities
open TimMovie.Core.Interfaces
open TimMovie.Infrastructure.Services

type AppMappingProfile() as this =
    inherit Profile()

    do
        this.CreateMap<UserRegistrationDto, User>()
        |> ignore
