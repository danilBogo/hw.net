using TimMovie.Core.Entities;
using TimMovie.SharedKernel.Specification;

namespace TimMovie.Core.Specifications.InheritedSpecifications.FilmSpec.UserFilmWatchedSpec;

public class WatchedFilmByFilmIdSpec: Specification<UserFilmWatched>
{
    public WatchedFilmByFilmIdSpec(Guid filmId)
    {
        Conditional = watched => watched.Film.Id == filmId;
    }
}