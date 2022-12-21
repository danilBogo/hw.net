using TimMovie.Core.Entities;
using TimMovie.SharedKernel.Interfaces;
using TimMovie.SharedKernel.Validators;

namespace TimMovie.Core.Query.Films;

public abstract class FilmQueryBuilder
{
    private readonly IRepository<Film> _filmRepository;
    protected IQueryable<Film> Query;

    protected FilmQueryBuilder(FilmQueryBuilder builder)
    {
        ArgumentValidator.ThrowExceptionIfNull(builder, nameof(builder));

        _filmRepository = builder._filmRepository;
        Query = builder.Query;
    }

    protected FilmQueryBuilder(IRepository<Film> filmRepository)
    {
        ArgumentValidator.ThrowExceptionIfNull(filmRepository, nameof(filmRepository));

        _filmRepository = filmRepository;
        Query = filmRepository.Query;
    }
    
    protected FilmQueryBuilder(IRepository<Film> filmRepository, IQueryable<Film> filmQuery)
    {
        ArgumentValidator.ThrowExceptionIfNull(filmRepository, nameof(filmRepository));
        ArgumentValidator.ThrowExceptionIfNull(filmQuery, nameof(filmQuery));

        _filmRepository = filmRepository;
        Query = filmQuery;
    }
    
    public QueryExecutor<Film> Build()
    {
        return new QueryExecutor<Film>(Query, _filmRepository);
    }
}