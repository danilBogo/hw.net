using TimMovie.Core.Entities;
using TimMovie.SharedKernel.Specification;

namespace TimMovie.Core.Specifications.StaticSpecification;

public class FilmStaticSpec
{
    public static readonly Specification<Film> FilmIsIncludedAnySubscriptionSpec = 
        new(film => film.Subscribes.Any());
    public static readonly Specification<Film> FilmWithoutRatings = 
        new(film => film.UserFilmWatcheds.All(watched => watched.Grade == null));
}