namespace TimMovie.Core.DTO.WatchedFilms;

public class WatchedFilmDto
{
    public Guid FilmId { get; set; }
    public string Title { get; set; } = null!;
    public int Grade { get; set; }
    public double Rating { get; set; }
    public string Image { get; set; } = null!;
    public DateOnly WatchedDate { get; set; }
}