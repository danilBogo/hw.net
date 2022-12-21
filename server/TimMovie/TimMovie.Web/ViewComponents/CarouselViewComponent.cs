using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TimMovie.Core.Classes;
using TimMovie.Core.Entities;
using TimMovie.Core.Enums;
using TimMovie.Core.Services.Films;
using TimMovie.SharedKernel.Interfaces;
using TimMovie.Web.ViewModels.FilmCard;
using TimMovie.Web.Extensions;

namespace TimMovie.Web.ViewComponents;

public class CarouselViewComponent : ViewComponent
{
    private readonly FilmCardService _filmCardService;
    private readonly IMapper _mapper;
    private readonly UserManager<User> _userManager;

    public CarouselViewComponent(FilmCardService filmCardService, IMapper mapper, UserManager<User> userManager)
    {
        _filmCardService = filmCardService;
        _mapper = mapper;
        _userManager = userManager;
    }

    public IViewComponentResult Invoke(string genreName, Guid? filmIdToRemove = null)
    {
        var userIdString = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        var userId = userIdString is null ? default : Guid.Parse(userIdString);
        var cards = _mapper.Map<List<FilmCardViewModel>>(
            _filmCardService.GetFilmCardsByGenre(genreName, 15, filmIdToRemove, userId));
        return View(cards);
    }
}