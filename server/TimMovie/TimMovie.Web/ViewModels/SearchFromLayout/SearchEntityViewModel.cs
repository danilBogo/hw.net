using TimMovie.Core.DTO.Search;

namespace TimMovie.Web.ViewModels.SearchFromLayout;

public class SearchEntityViewModel
{
    public IEnumerable<SearchFilmDto>? Films { get; set; }
    
    public IEnumerable<SearchGenreDto>? Genres { get; set; }
    
    public IEnumerable<SearchActorDto>? Actors { get; set; }
    
    public IEnumerable<SearchProducerDto>? Producers { get; set; }
}