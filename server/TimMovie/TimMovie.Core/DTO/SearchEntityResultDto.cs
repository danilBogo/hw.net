using TimMovie.Core.Entities;

namespace TimMovie.Core.DTO;

public class SearchEntityResultDto
{
    public IEnumerable<Film> Films { get; set; }
    
    public IEnumerable<Entities.Genre> Genres { get; set; }
    
    public IEnumerable<Entities.Actor> Actors { get; set;  }
    
    public IEnumerable<Entities.Producer> Producers { get; set; }
}