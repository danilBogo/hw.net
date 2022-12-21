using TimMovie.Core.Entities;
using TimMovie.SharedKernel.Specification;

namespace TimMovie.Core.Specifications.InheritedSpecifications.FilmSpec;

public class FilmWithActorSpec: Specification<Film>
{
    public FilmWithActorSpec(Guid actorId)
    {
        Conditional = film => film.Actors.Any(actor => actor.Id == actorId);
    }
}