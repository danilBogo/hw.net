namespace TimMovie.Core.DTO.Films;

public class BigFilmCardDto
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public int Year { get; set; }
    public string? Description { get; set; }
    public string? CountryName { get; set; }
    public string? Image { get; set; }
    public List<Entities.Genre> Genres { get; set; }
    public List<Entities.Actor> Actors { get; set; }
    public Entities.Producer? Producer { get; set; }
    public double? Rating { get; set; }
}