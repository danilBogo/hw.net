using FilesApi.Extensions;

namespace FilesApi.Services;

public class ImageService
{
    private readonly string _pathToContentRelativeProject;
    private readonly string _pathToFilmsRelativeContentDirectory;
    private readonly string _pathToUserPhotoRelativeContentDirectory;
    private readonly string _pathToBannersRelativeContentDirectory;
    private readonly string _pathToActorsRelativeContentDirectory;
    private readonly string _pathToProducersRelativeContentDirectory;
    

    public ImageService(IHostEnvironment webHostEnvironment, IConfiguration configuration)
    {
        var relativePathToContents = configuration.GetRelativePathToFileContents();
        _pathToContentRelativeProject = Path.Combine(webHostEnvironment.ContentRootPath, relativePathToContents);
        
        _pathToFilmsRelativeContentDirectory = configuration.GetRelativePathToFilms();
        _pathToUserPhotoRelativeContentDirectory = configuration.GetRelativePathToUserPhoto();
        _pathToBannersRelativeContentDirectory = configuration.GetRelativePathToBanners();
        _pathToActorsRelativeContentDirectory = configuration.GetRelativePathToActors();
        _pathToProducersRelativeContentDirectory = configuration.GetRelativePathToProducers();
    }

    public string GetLinkToDefaultUserPhoto()
    {
        return "user_photo/default.jpg";
    }

    public async Task<string> SaveUserPhotoAndReturnRelativePathAsync(IFormFile image)
    {
        return await SaveImageAsync(image, _pathToUserPhotoRelativeContentDirectory);
    }
    
    public async Task<string> SaveImageForFilmAndReturnRelativePathAsync(IFormFile image)
    {
        return await SaveImageAsync(image, _pathToFilmsRelativeContentDirectory);
    }
    
    public async Task<string> SaveImageForActorAndReturnRelativePathAsync(IFormFile image)
    {
        return await SaveImageAsync(image, _pathToActorsRelativeContentDirectory);
    }
    
    public async Task<string> SaveImageForBannerAndReturnRelativePathAsync(IFormFile image)
    {
        return await SaveImageAsync(image, _pathToBannersRelativeContentDirectory);
    }
    
    public async Task<string> SaveImageProducerAndReturnRelativePathAsync(IFormFile image)
    {
        return await SaveImageAsync(image, _pathToProducersRelativeContentDirectory);
    }

    private async Task<string> SaveImageAsync(IFormFile image, string pathToImageRelativeContentDirectory)
    {
        var fileName = GenerateRandomFileNameWithExtension(Path.GetExtension(image.FileName));
        var pathToFileRelativeContentDir = Path.Combine(
            pathToImageRelativeContentDirectory, 
            fileName);
        var absolutePathToFile = Path.Combine(
            _pathToContentRelativeProject,
            pathToFileRelativeContentDir);

        await using var fs = File.OpenWrite(absolutePathToFile);
        await image.CopyToAsync(fs);

        return pathToFileRelativeContentDir;
    }

    private string GenerateRandomFileNameWithExtension(string extension)
    {
        return $"{Path.GetFileNameWithoutExtension(Path.GetRandomFileName())}{extension}";
    }
}