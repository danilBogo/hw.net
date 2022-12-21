using Microsoft.AspNetCore.Mvc;
using TimMovie.Core.DTO.Search;
using TimMovie.Core.Interfaces;
using TimMovie.Web.ViewModels.SearchFromLayout;

namespace TimMovie.Web.ViewComponents;

public class SearchEntityViewComponent : ViewComponent
{
    private readonly ISearchEntityService _searchEntityService;

    public SearchEntityViewComponent(ISearchEntityService searchEntityService)
    {
        _searchEntityService = searchEntityService;
    }


    public IViewComponentResult Invoke(string? namePart)
    {
        var searchResult = _searchEntityService.GetSearchEntityResultByNamePart(namePart);

        var viewModel = new SearchEntityViewModel
        {
            Films = searchResult.Films.Select(f => new SearchFilmDto
                {Id = f.Id, Title = f.Title, Year = f.Year}),
            Genres = searchResult.Genres.Select(g => new SearchGenreDto {Name = g.Name}),
            Actors = searchResult.Actors.Select(a => new SearchActorDto
            {
                Id = a.Id,
                Name = a.Name,
                Surname = a.Surname
            }),
            Producers = searchResult.Producers.Select(p => new SearchProducerDto
            {
                Id = p.Id,
                Name = p.Name,
                Surname = p.Surname
            })
        };
        return View("~/Views/Shared/Components/SearchEntity/Default.cshtml", viewModel);
    }
}