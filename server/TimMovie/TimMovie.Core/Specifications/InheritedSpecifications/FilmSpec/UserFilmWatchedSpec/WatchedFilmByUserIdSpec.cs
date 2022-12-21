using TimMovie.Core.Entities;
using TimMovie.SharedKernel.Specification;

namespace TimMovie.Core.Specifications.InheritedSpecifications.FilmSpec.UserFilmWatchedSpec;

public class WatchedFilmByUserIdSpec: Specification<UserFilmWatched>
{
    public WatchedFilmByUserIdSpec(Guid userId)
    {
        Conditional = watched => watched.WatchedUser.Id == userId;
    }
}