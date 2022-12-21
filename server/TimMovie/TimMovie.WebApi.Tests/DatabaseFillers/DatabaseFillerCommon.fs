namespace TimMovie.WebApi.Tests

open Microsoft.AspNetCore.Identity
open TimMovie.Core.Entities
open TimMovie.Infrastructure.Database

type DatabaseFillerCommon() =

    static member Start(dbContext: ApplicationContext, userManager: UserManager<User>) =
        let dbFillerUsers = DatabaseFillerUsers()
        dbFillerUsers.Start(userManager)
        let dbFillerNavbarSearch = DatabaseFillerNavbarSearch()
        dbFillerNavbarSearch.Start(dbContext)
        let dbFillerNotifications = DatabaseFillerNotifications()
        dbFillerNotifications.Start(dbContext, userManager)
        let dbFillerFilmFilters = DatabaseFillerFilmFilters()
        dbFillerFilmFilters.Start(dbContext, userManager)
        let dbFillerSubscribes = DatabaseFillerSubscribes()
        dbFillerSubscribes.Start(dbContext, userManager)
        let dbFillerRecommendations = DatabaseFillerRecommendations()
        dbFillerRecommendations.Start(dbContext, userManager)
        let dbFillerWatchLater = DatabaseFillerWatchLater()
        dbFillerWatchLater.Start(dbContext, userManager)
