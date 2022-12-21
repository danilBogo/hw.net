using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TimMovie.Core.DTO.Subscribes;
using TimMovie.Core.DTO.Users;
using TimMovie.Core.Entities;
using TimMovie.Core.Entities.Enums;
using TimMovie.Core.Interfaces;
using TimMovie.Core.Services.Countries;
using TimMovie.Core.Services.Films;
using TimMovie.Core.Services.Subscribes;
using TimMovie.Core.Services.SupportedServices;
using TimMovie.Core.ValidatorServices;
using TimMovie.SharedKernel.Classes;
using TimMovie.SharedKernel.Interfaces;
using TimMovie.Web.Extensions;
using TimMovie.Web.ViewModels.FilmCard;
using TimMovie.Web.ViewModels.User;
using TimMovie.Web.ViewModels.UserSubscribes;

namespace TimMovie.Web.Controllers.Profile;

[ApiExplorerSettings(IgnoreApi=true)]
public class UserProfileController : Controller
{
    private readonly IUserService _userService;
    private readonly FilmCardService _filmCardService;
    private readonly ISubscribeService _subscribeService;
    private readonly CountryService _countryService;
    private readonly FileService _fileService;
    private readonly UserValidator _userValidator;
    private readonly UserManager<User> _userManager;
    private readonly FilmService _filmService;
    private readonly IRepository<Subscribe> _repository;
    private readonly IMapper _mapper;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public UserProfileController(
        IUserService userService,
        IMapper mapper,
        FilmCardService filmCardService,
        ISubscribeService subscribeService,
        FileService fileService,
        IWebHostEnvironment webHostEnvironment, 
        CountryService countryService, 
        UserValidator userValidator,
        UserManager<User> userManager,
        FilmService filmService,
        IRepository<Subscribe> repository)
    {
        _userService = userService;
        _mapper = mapper;
        _filmCardService = filmCardService;
        _subscribeService = subscribeService;
        _fileService = fileService;
        _webHostEnvironment = webHostEnvironment;
        _countryService = countryService;
        _userValidator = userValidator;
        _userManager = userManager;
        _filmService = filmService;
        _repository = repository;
    }

    [HttpGet("[controller]/{id:guid}")]
    public async Task<IActionResult> Profile(Guid id)
    {
        if (!await _userService.UserIsExisted(id))
        {
            return View("~/Views/Errors/UserNotExisting.cshtml");
        }

        var currentUser = User.GetUserId();

        var userInfo = await _userService.GetInfoAboutUserAsync(id);
        var filmCards = _filmCardService.GetLatestFilmsViewedByUser(id, 6, currentUser);
        var subscribes = _subscribeService.GetAllActiveUserSubscribes(id);

        var userProfile = new UserProfileViewModel
        {
            IsOwner = User.Identity.IsAuthenticated && User.GetUserId() == id,
            UserInfo = _mapper.Map<UserInfoViewModel>(userInfo),
            FilmCards = _mapper.Map<IEnumerable<FilmCardViewModel>>(filmCards),
            UserSubscribes = _mapper.Map<IEnumerable<UserSubscribeViewModel>>(subscribes),
            CountryNames = _countryService.GetCountryNames()
        };
        userProfile.UserInfo.Id = id;
        
        return View("~/Views/Profile/UserProfile.cshtml", userProfile);
    }
    
    [Authorize]
    [HttpPost]
    public async Task<JsonResult> SaveUserPhotoAsync(IFormFile file)
    {
        if (!_fileService.UserImageHasCorrectExtension(file))
        {
            return Json(Result.Fail($"Некорректное расшрение файла: {Path.GetExtension(file.Name)}"));
        }

        if (!_fileService.UserPhotoHasCorrectSize(file))
        {
            return Json(Result.Fail("Размер файла должен быть меньше 1 Мб"));
        }

        var userId = HttpContext.User.GetUserId();
        var resultUpdate = await _userService.UpdateUserPhotoAsync(file, userId.Value, _webHostEnvironment.WebRootPath);

        return Json(resultUpdate);
    }
    
    [Authorize]
    [HttpPost]
    public async Task<JsonResult> SaveUserInfo(ShortUserInfoDto userInfo)
    {
        var validationResult = _userValidator.ValidateUserInfo(userInfo);
        if (!validationResult.Succeeded)
        {
            return Json(validationResult);
        }

        var userId = HttpContext.User.GetUserId();
        await _userService.UpdateUserInfo(userInfo, userId.Value);

        return Json(Result.Ok());
    }

    [HttpGet]
    public async Task<UserInfoForChatDto?> GetUserInfoForChat(Guid userId)
    {
        return await _userService.GetUserInfoForChat(userId);
    }
    
    [HttpGet]
    public void UpdateUserStatusWatchingFilm(Guid filmId) { }
}