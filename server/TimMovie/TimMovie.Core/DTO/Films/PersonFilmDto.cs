namespace TimMovie.Core.DTO.Films;

public class PersonFilmDto
{
    public Guid Id { get; set; }
    public string Image { get; set; }
    public int Year { get; set; }
    public string Title { get; set; }
    public double Rating { get; set; }
}