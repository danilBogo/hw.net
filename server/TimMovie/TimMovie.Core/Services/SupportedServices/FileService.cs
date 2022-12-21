using Microsoft.AspNetCore.Http;

namespace TimMovie.Core.Services.SupportedServices;

public class FileService
{
    private static readonly HashSet<string> ImageExtensions = new() {".jpeg", ".jpg", ".pjpeg", ".png"};
    private const long ByteInOmeMb = 1024 * 1024;
    
    public bool UserImageHasCorrectExtension(IFormFile photo)
    {
        return ImageExtensions.Contains(Path.GetExtension(photo.FileName));
    }

    public bool UserPhotoHasCorrectSize(IFormFile photo)
    {
        return photo.Length < ByteInOmeMb;
    }
}