using FilesApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace FilesApi.Controllers;

[ApiController]
[Route("file-api/image")]
public class ImageController : ControllerBase
{
    private const string DefaultImgName = "default.jpg";
    private readonly ImageService _imageService;
    private readonly FileService _fileService;
    private readonly ILogger<ImageController> _logger;

    public ImageController(ImageService imageService, FileService fileService, ILogger<ImageController> logger)
    {
        _imageService = imageService;
        _fileService = fileService;
        _logger = logger;
    }

    [HttpGet]
    public IActionResult GetImage([FromQuery] string relativePath)
    {
        if (!_fileService.ContentFileIsExisted(relativePath))
        {
            _logger.LogWarning($"Изображение не найдено");
            if (!TryGetAbsolutePathDefaultImage(relativePath, out var absolutePathDefaultImg))
                return NotFound();
            return PhysicalFile(absolutePathDefaultImg, "image/jpeg");
        }

        var absolutePath = _fileService.GetAbsolutePathToFile(relativePath);

        return PhysicalFile(absolutePath, "image/jpeg");
    }

    private bool TryGetAbsolutePathDefaultImage(string relativePath, out string absolutePathDefaultImg)
    {
        _logger.LogInformation($"Relative path: {relativePath}");
        absolutePathDefaultImg = null!;
        var parts = relativePath.Split("/");
        _logger.LogInformation($"Split count: {parts.Length}");
        if (parts.Length != 3)
            return false;
        var newPath = $"{parts[0]}/{parts[1]}/{DefaultImgName}";
        _logger.LogInformation($"New path: {newPath}");
        if (!_fileService.ContentFileIsExisted(newPath))
            return false;
        absolutePathDefaultImg = _fileService.GetAbsolutePathToFile(newPath);
        return true;
    }

    [HttpGet("user-photo/default")]
    public async Task<string> GetLinkToDefaultUserPhoto()
    {
        return Url.Action(
            "GetImage",
            "Image",
            new {relativePath = _imageService.GetLinkToDefaultUserPhoto()})!;
    }


    [HttpPost("user-photo")]
    public async Task<IActionResult> SaveUserPhoto([FromForm] IFormFile image)
    {
        var relativePath = await _imageService.SaveUserPhotoAndReturnRelativePathAsync(image);

        var urlToImage = GetUrlToImage(relativePath);
        return Created(urlToImage, null);
    }

    [HttpPost("film")]
    public async Task<IActionResult> SaveFilmImage([FromForm] IFormFile image)
    {
        var relativePath = await _imageService.SaveImageForFilmAndReturnRelativePathAsync(image);

        var urlToImage = GetUrlToImage(relativePath);
        return Created(urlToImage, null);
    }

    [HttpPost("actor")]
    public async Task<IActionResult> SaveActorImage([FromForm] IFormFile image)
    {
        var relativePath = await _imageService.SaveImageForActorAndReturnRelativePathAsync(image);

        var urlToImage = GetUrlToImage(relativePath);
        return Created(urlToImage, null);
    }

    [HttpPost("banner")]
    public async Task<IActionResult> SaveBannerImage([FromForm] IFormFile image)
    {
        var relativePath = await _imageService.SaveImageForBannerAndReturnRelativePathAsync(image);

        var urlToImage = GetUrlToImage(relativePath);
        return Created(urlToImage, null);
    }

    [HttpPost("producer")]
    public async Task<IActionResult> SaveProducerImage([FromForm] IFormFile image)
    {
        var relativePath = await _imageService.SaveImageProducerAndReturnRelativePathAsync(image);

        var urlToImage = GetUrlToImage(relativePath);
        return Created(urlToImage, null);
    }

    private string GetUrlToImage(string relativePath)
    {
        return Url.Action(
            "GetImage",
            "Image",
            new {relativePath = relativePath},
            Uri.UriSchemeHttps)!;
    }
}