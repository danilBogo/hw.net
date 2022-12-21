using TimMovie.Core.Entities;
using TimMovie.Core.Specifications.InheritedSpecifications.FilmSpec;
using TimMovie.Core.Specifications.StaticSpecification;
using TimMovie.SharedKernel.Interfaces;
using TimMovie.SharedKernel.Specification;

namespace TimMovie.Core.Query.Films;

public class FilmFiltersBuilder: FilmQueryBuilder
{
    public FilmFiltersBuilder(FilmQueryBuilder builder) : base(builder)
    {
    }
    
    public FilmFiltersBuilder(IRepository<Film> query) : base(query)
    {
    }
    
    public FilmFiltersBuilder(IRepository<Film> filmRepository, IQueryable<Film> filmQuery) : base(filmRepository, filmQuery)
    {
    }

    public FilmFiltersBuilder AddFilterByGenre(IEnumerable<string>? genreNames)
    {
        if (genreNames is null)
        {
            return this;
        }
        
        Query = Query.Where(new FilmByGenreNamesSpec(genreNames.ToArray()));
        return this;
    }
    
    public FilmFiltersBuilder AddFilterByYear(int year)
    {
        Query = Query.Where(new FilmDateSpec(year));
        return this;
    }
    
    public FilmFiltersBuilder AddFilterByYear(int firstYear, int lastYear)
    {
        Query = Query.Where(new FilmDateSpec(firstYear, lastYear));
        return this;
    }
    
    public FilmFiltersBuilder AddFilterByCountry(IEnumerable<string>? countryNames)
    {
        if (countryNames is null || !countryNames.Any())
        {
            return this;
        }

        Specification<Film> filterOnCountry = new FilmByNameCountrySpec(countryNames.First());
        foreach (var name in countryNames.Skip(1))
        {
            filterOnCountry = filterOnCountry || new FilmByNameCountrySpec(name);
        }

        Query = Query.Where(filterOnCountry);
        
        return this;
    }
    
    public FilmFiltersBuilder AddFilterOnMinimumRating(double rating)
    {
        Query = rating > 0 
            ? Query.Where(new FilmWithMinimumRatingSpec(rating)) 
            : Query;

        return this;
    }
}