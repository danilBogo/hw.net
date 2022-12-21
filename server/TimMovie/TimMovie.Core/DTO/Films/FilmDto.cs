using TimMovie.Core.DTO.Actor;
using TimMovie.Core.DTO.Comments;
using TimMovie.Core.DTO.Country;
using TimMovie.Core.DTO.Genre;
using TimMovie.Core.DTO.Producer;

namespace TimMovie.Core.DTO.Films;

public class FilmDto
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public int Year { get; set; }
    public string? Description { get; set; }
    public CountryDto? Country { get; set; }
    public double? Rating { get; set; }
    public int? GradesNumber { get; set; }
    public string? FilmLink { get; set; }
    public List<CommentsDto> Comments { get; set; }
    public List<ProducerDto> Producers { get; set; }
    public List<ActorDto> Actors { get; set; }
    public List<GenreDto> Genres { get; set; }
    public string Image { get; set; } = null!;
    
    public bool IsAvailable { get; set; }
}