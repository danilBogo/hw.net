using Microsoft.AspNetCore.Http;

namespace TimMovie.Core.Interfaces;

public interface IFileService
{
    Task<string> SaveUserPhoto(IFormFile photo);
    Task<string> GetLinkToDefaultUserPhoto();
    string? GetUriToFileServer(string? relativePath);
    bool IsRequestToFileServer(string path);
    
}