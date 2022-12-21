using TimMovie.Core.Enums;

namespace TimMovie.Web.ViewModels.FilmFilter;

public class CurrentSelectedFilters
{
    public HashSet<string> GenreNames { get; set; }
    public HashSet<string> Countries { get; set; }
    public int? MinRating { get; set; }
    public int? Year { get; set; }
    public FilmSortingType FilmSortingType { get; set; }
    public bool IsDescending { get; set; }
}