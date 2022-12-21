using TimMovie.Core.DTO.Genre;

namespace TimMovie.Core.DTO.Subscribes;

public class SubscribeDto
{
    public Guid Id { get; set; }
    
    public string Name { get; set; }

    public decimal Price { get; set; }

    public string? Description { get; set; }
    
    public List<SubscribeFilmDto> Films { get; set; }
    
    public List<GenreDto> Genres { get; set; }
}