using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TimMovie.Core.DTO;
using TimMovie.Core.DTO.Films;
using TimMovie.Core.Enums;
using TimMovie.Core.Services.Countries;
using TimMovie.Core.Services.Films;
using TimMovie.Core.Services.Genres;
using TimMovie.Web.Extensions;
using TimMovie.Web.ViewModels.FilmCard;
using TimMovie.Web.ViewModels.FilmFilter;

namespace TimMovie.Web.Controllers.Navbar;

public class FilmsController : Controller
{
    private readonly CountryService _countryService;
    private readonly GenreService _genreService;
    private readonly FilmCardService _filmCardService;
    private readonly IMapper _mapper;

    public FilmsController(
        FilmCardService filmsFilterService,
        CountryService countryService,
        GenreService genreService,
        IMapper mapper)
    {
        _filmCardService = filmsFilterService;
        _countryService = countryService;
        _genreService = genreService;
        _mapper = mapper;
    }

    [HttpGet]
    public IActionResult Filters(
        HashSet<string> genres,
        HashSet<string> countries, 
        int? minRating = null,
        int? year = null,
        FilmSortingType filmSortingType = FilmSortingType.Popularity,
        bool isDescending = true)
    {
        if (minRating is < 5 or >= 10)
        {
            minRating = null;
        }
        
        if (year < 1900 || year > DateTime.Now.Year)
        {
            year = null;
        }
        
        var filmFilters = new FilmFiltersViewModel
        {
            GenresName = _genreService.GetGenreNames(),
            CountriesName = _countryService.GetCountryNames(),
            Ratings = new[] {9, 8, 7, 6, 5},
            AnnualPeriods = new[]
            {
                new AnnualPeriodDto(2022, 2022),
                new AnnualPeriodDto(2021, 2021),
                new AnnualPeriodDto(2020, 2020),
                new AnnualPeriodDto(2017, 2019),
                new AnnualPeriodDto(2014, 2016),
                new AnnualPeriodDto(2011, 2013),
                new AnnualPeriodDto(2001, 2010),
                new AnnualPeriodDto(1900, 2000)
            },
            SelectedFilters = new CurrentSelectedFilters()
            {
                GenreNames = genres,
                Countries = countries,
                MinRating = minRating,
                Year = year,
                FilmSortingType = filmSortingType,
                IsDescending = isDescending
            }
        };

        return View("/Views/Navbar/Films/Filters.cshtml", filmFilters);
    }

    [HttpPost]
    public IActionResult FilmFilters(GeneralPaginationDto<SelectedFilmFiltersDto> filtersWithPagination)
    {
        var userId = User.GetUserId() is null ? default : User.GetUserId().Value;
        var cardsDto = _filmCardService.GetFilmCardsByFilters(filtersWithPagination, userId);
        if (filtersWithPagination?.DataDto is null)
        {
            return NotFound();
        }

        var cardsViewModel = _mapper.Map<IEnumerable<FilmCardViewModel>>(cardsDto);

        return PartialView("~/Views/Partials/FilmCard/FilmCards.cshtml", cardsViewModel);
    }
}