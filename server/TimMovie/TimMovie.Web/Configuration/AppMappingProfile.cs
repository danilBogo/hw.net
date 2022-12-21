using AutoMapper;
using TimMovie.Core.DTO.Account;
using TimMovie.Core.DTO.Actor;
using TimMovie.Core.DTO.Comments;
using TimMovie.Core.DTO.Country;
using TimMovie.Core.DTO.Films;
using TimMovie.Core.DTO.Genre;
using TimMovie.Core.DTO.Payment;
using TimMovie.Core.DTO.Person;
using TimMovie.Core.DTO.Producer;
using TimMovie.Core.DTO.Subscribes;
using TimMovie.Core.DTO.Users;
using TimMovie.Core.DTO.WatchedFilms;
using TimMovie.Core.Entities;
using TimMovie.Web.ViewModels;
using TimMovie.Web.ViewModels.Account;
using TimMovie.Web.ViewModels.FilmCard;
using TimMovie.Web.ViewModels.Films;
using TimMovie.Web.ViewModels.Payment;
using TimMovie.Web.ViewModels.Person;
using TimMovie.Web.ViewModels.User;
using TimMovie.Web.ViewModels.UserSubscribes;
using TimMovie.Web.ViewModels.WatchedFilms;

namespace TimMovie.Web.Configuration;

public class AppMappingProfile : Profile
{
    public AppMappingProfile()
    {
        CreateMap<RegistrationViewModel, UserRegistrationDto>();
        CreateMap<UserRegistrationDto, User>();
        CreateMap<Banner, BannerViewModel>();
        CreateMap<ExternalLoginViewModel, ExternalLoginDto>();
        CreateMap<FilmCardDto, FilmCardViewModel>();
        CreateMap<Film, FilmCardViewModel>();
        CreateMap<LoginViewModel, LoginDto>().ReverseMap();
        CreateMap<UserInfoDto, UserInfoViewModel>();
        CreateMap<SubscribePaymentViewModel, SubscribePaymentDto>().ReverseMap();
        CreateMap<CardViewModel, CardDto>().ForMember(x => x.ExpirationYear, opt =>
                opt.MapFrom(src => 2000 + src.ExpirationYear))
            .ForMember(x => x.CardNumber, opt =>
                opt.MapFrom(src =>
                    src.CardNumber.Trim())).ReverseMap();
        CreateMap<UserSubscribeDto, UserSubscribeViewModel>();
        CreateMap<FilmDto, FilmViewModel>();
        CreateMap<PersonDto, PersonViewModel>();
        CreateMap<WatchedFilmDto, WatchedFilmViewModel>();
        CreateMap<PersonFilmDto, PersonFilmViewModel>();
        CreateMap<BigFilmCardDto, BigFilmCardViewModel>();
    }
}