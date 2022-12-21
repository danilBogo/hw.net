namespace TimMovie.WebApi.Tests

open System

type Constants() =
    static member NavbarSearch = "/MainPage/SearchEntityByNamePart"
    static member Notifications = "/MainPage/GetAllUserNotifications"
    static member FilmByFilters = "/MainPage/GetFilmByFilters"
    static member AllUserSubscribes = "/MainPage/GetUserSubscribes"
    static member AllSubscribesByNamePart = "/MainPage/GetAllSubscribesByNamePart"

    static member AllSubscribesByNamePartWithPagination =
        "/MainPage/GetSubscribesByNamePartWithPagination"

    static member UserInformation = "/Profile/GetUserInformation"
    static member PayForSubscription = "/Profile/PayForSubscription"
    static member GetFilmById = "/Film/GetFilmById"
    static member AddCommentToFilm = "/Film/AddCommentToFilm"
    static member FilmRecommendations = "/Film/GetFilmRecommendations"
    static member WatchLaterFilms = "/MainPage/GetWatchLaterFilms"

    static member DefaultUserName = "testUser"
    static member DefaultDisplayName = Constants.DefaultUserName
    static member DefaultEmail = "testEmail@testEmail.testEmail"
    static member DefaultPathToPhoto = "123456"
    static member DefaultPassword = "testPassword111"

    static member DefaultSubscribeForPaymentGuid =
        Guid("07ead1e4-e588-4e73-87ee-e68f08352393")

    static member DefaultFilmGuid =
        Guid("cd1cca53-1a3d-46ca-ad28-50c863c98078")
