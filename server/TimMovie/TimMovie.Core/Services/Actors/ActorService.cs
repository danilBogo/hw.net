using TimMovie.Core.Entities;
using TimMovie.Core.Specifications.InheritedSpecifications.ActorSpec;
using TimMovie.SharedKernel.Interfaces;

namespace TimMovie.Core.Services.Actors;

public class ActorService
{
    private readonly IRepository<Actor> _actorRepository;

    public ActorService(IRepository<Actor> actorRepository)
    {
        _actorRepository = actorRepository;
    }

    public IEnumerable<Actor> GetActorsByNamePart(string? namePart, int count = int.MaxValue) =>
        _actorRepository.Query
            .Where(new ActorByNamePartSpec(namePart))
            .Take(count);
}