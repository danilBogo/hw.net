using TimMovie.Core.DTO.Films;

namespace TimMovie.Web.ViewModels.FilmFilter;

public class FilmFiltersViewModel
{
    public IEnumerable<string> GenresName { get; set; }
    public IEnumerable<string> CountriesName { get; set; }
    public IEnumerable<int> Ratings { get; set; }
    public IEnumerable<AnnualPeriodDto> AnnualPeriods { get; set; }
    public CurrentSelectedFilters SelectedFilters { get; set; }
}