using TimMovie.Core.Enums;

namespace TimMovie.Core.DTO.Films;

public class SelectedFilmFiltersDto
{
    public FilmSortingType SortingType { get; set; }
    public IEnumerable<string>? GenresName { get; set; }
    public IEnumerable<string>? CountriesName { get; set; }
    public int? Rating { get; set; }
    public AnnualPeriodDto AnnualPeriod { get; set; } = null!;
    public bool IsDescending { get; set; }
}