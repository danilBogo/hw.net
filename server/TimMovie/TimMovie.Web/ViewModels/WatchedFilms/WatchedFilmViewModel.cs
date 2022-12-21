namespace TimMovie.Web.ViewModels.WatchedFilms;

public class WatchedFilmViewModel
{
    public Guid FilmId { get; set; }
    public string Title { get; set; } = null!;
    public int Grade { get; set; }
    public double Rating { get; set; }
    public string Image { get; set; } = null!;
    public DateOnly WatchedDate { get; set; }
}