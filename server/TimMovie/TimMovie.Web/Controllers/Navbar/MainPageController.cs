using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TimMovie.Core.Services.Banners;
using TimMovie.Core.Services.Films;
using TimMovie.Web.ViewModels;

namespace TimMovie.Web.Controllers.Navbar;

public class MainPageController : Controller
{
    private readonly BannerService _bannerService;
    private readonly IMapper _mapper;

    public MainPageController(IMapper mapper, FilmCardService filmCardService, BannerService bannerService)
    {
        _mapper = mapper;
        _bannerService = bannerService;
    }


    [HttpGet]
    public IActionResult MainPage()
    {
        var banners = _mapper.Map<List<BannerViewModel>>(_bannerService.GetBanners());
        return View("~/Views/Navbar/MainPage/MainPage.cshtml", banners);
    }

    [HttpPost]
    public string[] GetImagesForBanners(Guid[] filmIds, bool isSmall) =>
        isSmall
            ? _bannerService.GetSmallBannerImages(filmIds)
            : _bannerService.GetBigBannerImages(filmIds);
}