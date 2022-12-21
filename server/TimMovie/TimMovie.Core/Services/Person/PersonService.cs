using AutoMapper;
using TimMovie.Core.DTO.Films;
using TimMovie.Core.DTO.Person;
using TimMovie.Core.Entities;
using TimMovie.Core.Services.Films;
using TimMovie.Core.Specifications.InheritedSpecifications;
using TimMovie.Core.Specifications.InheritedSpecifications.FilmSpec;
using TimMovie.SharedKernel.BaseEntities;
using TimMovie.SharedKernel.Interfaces;
using TimMovie.SharedKernel.Specification;

namespace TimMovie.Core.Services.Person;

public class PersonService
{
    private readonly IRepository<Producer> _producerRepository;
    private readonly IRepository<Actor> _actorRepository;
    private readonly IRepository<Film> _filmRepository;
    private readonly IMapper _mapper;
    private readonly FilmService _filmService;

    public PersonService(IRepository<Producer> producerRepository, IRepository<Actor> actorRepository,
        IMapper mapper, FilmService filmService, IRepository<Film> filmRepository)
    {
        _producerRepository = producerRepository;
        _actorRepository = actorRepository;
        _mapper = mapper;
        _filmService = filmService;
        _filmRepository = filmRepository;
    }

    public PersonDto? GetActorById(Guid id)
    {
        return GetPersonById(id, _actorRepository);
    }
    
    public PersonDto? GetProducerById(Guid id)
    {
        return GetPersonById(id, _producerRepository);
    }
    
    public int GetAmountFilmsForActor(Guid id)
    {
        var count = _actorRepository.Query
            .Where(new EntityByIdSpec<Actor>(id))
            .Select(actor => actor.Films.Count)
            .FirstOrDefault();

        return count;
    }

    public int GetAmountFilmsForProducer(Guid id)
    {
        var count = _producerRepository.Query
            .Where(new EntityByIdSpec<Producer>(id))
            .Select(producer => producer.Films.Count)
            .FirstOrDefault();

        return count;
    }

    public IEnumerable<PersonFilmDto> GetFilmsByActor(Guid actorId, int skip, int take)
    {
        return GetFilmByPerson(new FilmWithActorSpec(actorId), skip, take);
    }
    
    public IEnumerable<PersonFilmDto> GetFilmsByProducer(Guid producerId, int skip, int take)
    {
        return GetFilmByPerson(new FilmWithProducerSpec(producerId), skip, take);
    }

    private IEnumerable<PersonFilmDto> GetFilmByPerson<TSpec>(TSpec conditionalByPerson, int skip, int take)
        where TSpec: Specification<Film>
    {
        var films = _filmRepository.Query
            .Where(conditionalByPerson)
            .Skip(skip)
            .Take(take)
            .ToList();

        var filmsDto = films
            .Select(film =>
            {
                var filmDto = _mapper.Map<PersonFilmDto>(film);
                filmDto.Rating = _filmService.GetRating(film);
                return filmDto;
            });

        return filmsDto;
    }

    private PersonDto? GetPersonById<TPerson>(Guid id, IRepository<TPerson> personRep)
        where TPerson : PersonBaseEntity, IIdHolder<Guid>
    {
        var person =  personRep.Query.FirstOrDefault(new EntityByIdSpec<TPerson>(id));
        return person is not null
            ? _mapper.Map<PersonDto>(person)
            : null;
    }
}