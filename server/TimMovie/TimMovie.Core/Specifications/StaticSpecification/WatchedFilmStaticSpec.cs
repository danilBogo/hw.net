using TimMovie.Core.Entities;
using TimMovie.SharedKernel.Specification;

namespace TimMovie.Core.Specifications.StaticSpecification;

public class WatchedFilmStaticSpec
{
    public static readonly Specification<UserFilmWatched> HaveGrade =
        new(watched => watched.Grade != null);
}