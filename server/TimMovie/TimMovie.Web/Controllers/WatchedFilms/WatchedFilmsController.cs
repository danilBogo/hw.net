using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TimMovie.Core.Classes;
using TimMovie.Core.Entities;
using TimMovie.Core.Services.WatchedFilms;
using TimMovie.Web.Extensions;
using TimMovie.Web.ViewModels.WatchedFilms;

namespace TimMovie.Web.Controllers.WatchedFilms;

[ApiExplorerSettings(IgnoreApi=true)]
public class WatchedFilmsController : Controller
{
    private readonly UserManager<User> userManager;
    private readonly WatchedFilmService watchedFilmService;
    private readonly IMapper mapper;

    private const int PageSize = 9;

    public WatchedFilmsController(UserManager<User> userManager,
        WatchedFilmService watchedFilmService,
        IMapper mapper)
    {
        this.userManager = userManager;
        this.watchedFilmService = watchedFilmService;
        this.mapper = mapper;
    }


    [HttpGet("[controller]/{id:guid}")]
    public async Task<IActionResult> WatchedFilms(Guid id, int page = 1)
    {
        var user = await userManager.FindByIdAsync(id.ToString());

        if (user is null)
        {
            return View("~/Views/Errors/UserNotExisting.cshtml");
        }

        var watchedFilmsDto = watchedFilmService.GetPaginatedUserWatchedFilmsByUserId(id, page, PageSize);

        if (!watchedFilmsDto.Any())
        {
            return NotFound();
        }
        
        var isOwner = User.Identity.IsAuthenticated && User.GetUserId() == user.Id;

        var watchedFilmsViewModel = mapper.Map<PaginatedList<WatchedFilmViewModel>>(watchedFilmsDto);
        watchedFilmsViewModel.PageIndex = watchedFilmsDto.PageIndex;
        watchedFilmsViewModel.TotalPages = watchedFilmsDto.TotalPages;
        watchedFilmsViewModel.PageSize = watchedFilmsDto.PageSize;
        return View((watchedFilmsViewModel, isOwner));
    }
}