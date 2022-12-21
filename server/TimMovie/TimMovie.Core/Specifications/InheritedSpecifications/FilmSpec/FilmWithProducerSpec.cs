using TimMovie.Core.Entities;
using TimMovie.SharedKernel.Specification;

namespace TimMovie.Core.Specifications.InheritedSpecifications.FilmSpec;

public class FilmWithProducerSpec: Specification<Film>
{
    public FilmWithProducerSpec(Guid producerId)
    {
        Conditional = film => film.Producers.Any(producer => producer.Id == producerId);
    }
}