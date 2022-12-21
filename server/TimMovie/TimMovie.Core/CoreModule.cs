using Autofac;
using TimMovie.Core.Interfaces;
using TimMovie.Core.Services.Actors;
using TimMovie.Core.Services.Banners;
using TimMovie.Core.Services.ChatTemplatedNotifications;
using TimMovie.Core.Services.Countries;
using TimMovie.Core.Services.Films;
using TimMovie.Core.Services.Genres;
using TimMovie.Core.Services.Messages;
using TimMovie.Core.Services.Notifications;
using TimMovie.Core.Services.Person;
using TimMovie.Core.Services.Producers;
using TimMovie.Core.Services.Subscribes;
using TimMovie.Core.Services.SupportedServices;
using TimMovie.Core.Services.WatchedFilms;
using TimMovie.Core.ValidatorServices;

namespace TimMovie.Core;

public class CoreModule: Module
{
    protected override void Load(ContainerBuilder builder)
    {
        RegisterServiceOnSelf<FilmService>(builder);
        RegisterServiceOnSelf<CountryService>(builder);
        RegisterServiceOnSelf<GenreService>(builder);
        RegisterServiceOnSelf<FilmCardService>(builder);
        RegisterServiceOnSelf<BannerService>(builder);
        RegisterServiceOnSelf<ActorService>(builder);
        RegisterServiceOnSelf<ProducerService>(builder);
        builder.RegisterType<SubscribeService>().As<ISubscribeService>().InstancePerLifetimeScope();
        builder.RegisterType<NotificationService>().As<INotificationService>().InstancePerLifetimeScope();
        RegisterServiceOnSelf<FileService>(builder);
        RegisterServiceOnSelf<UserValidator>(builder);
        RegisterServiceOnSelf<WatchedFilmService>(builder);
        RegisterServiceOnSelf<ChatTemplatedNotificationService>(builder);
        RegisterServiceOnSelf<MessageService>(builder);
        RegisterServiceOnSelf<PersonService>(builder);
        RegisterServiceOnSelf<WatchLaterService>(builder);
    }

    private void RegisterServiceOnSelf<T>(ContainerBuilder builder) 
        where T : notnull
    {
        builder.RegisterType<T>().AsSelf().InstancePerLifetimeScope();
    }
}