using TimMovie.Core.DTO;

namespace TimMovie.Core.Interfaces;

public interface ISearchEntityService
{
    public SearchEntityResultDto GetSearchEntityResultByNamePart(string? namePart);
}