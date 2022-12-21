using FilesApi.Extensions;

namespace FilesApi.Services;

public class FileService
{
    private readonly string _pathToContent;
    
    public FileService(IHostEnvironment webHostEnvironment, IConfiguration configuration)
    {
        var relativePathToContents = configuration.GetRelativePathToFileContents();
        _pathToContent = Path.Combine(webHostEnvironment.ContentRootPath, relativePathToContents);
    }

    public bool ContentFileIsExisted(string relativePath)
    {
        var pathToFile = Path.Combine(_pathToContent, relativePath);

        return File.Exists(pathToFile);
    }

    public string GetAbsolutePathToFile(string relativePath)
    {
        return Path.Combine(_pathToContent, relativePath);
    }
}