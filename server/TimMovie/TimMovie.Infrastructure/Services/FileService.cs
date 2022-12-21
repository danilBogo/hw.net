using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;
using TimMovie.Core.Interfaces;

namespace TimMovie.Infrastructure.Services;

public class FileService: IFileService
{
    private static readonly Uri PathForAddUserPhoto = new("file-api/image/user-photo", UriKind.Relative);
    private static readonly Regex RequestOnFileService = new("/file-api/");
    
    private readonly Uri _absoluteUriForAddUserPhoto;
    private readonly Uri _serviceUri;

    public FileService(IConfigurationService configurationService)
    {
        _serviceUri = new Uri(configurationService.GetFileServiceUri());
        _absoluteUriForAddUserPhoto = new Uri(_serviceUri, PathForAddUserPhoto);
    }
    
    public async Task<string> SaveUserPhoto(IFormFile photo)
    {
        using var httpClient = new HttpClient();
        using var multipartFormContent = new MultipartFormDataContent();
        using var fileStreamContent = new StreamContent(photo.OpenReadStream());
        fileStreamContent.Headers.ContentType = new MediaTypeHeaderValue("multipart/form-data");

        multipartFormContent.Add(fileStreamContent, "Image", photo.FileName);
        
        var response = await httpClient.PostAsync(_absoluteUriForAddUserPhoto, multipartFormContent);
        var path = response.Headers.Location;
        return path.PathAndQuery;
    }

    public async Task<string> GetLinkToDefaultUserPhoto()
    {
        using var httpClient = new HttpClient();
        var uri = new Uri(_serviceUri, "file-api/image/user-photo/default");
        
        return await httpClient.GetStringAsync(uri);
    }
    
    public string? GetUriToFileServer(string? relativePath)
    {
        return relativePath is not null 
            ? new Uri(_serviceUri, relativePath).AbsoluteUri
            : null;
    }

    public bool IsRequestToFileServer(string path)
    {
        return RequestOnFileService.IsMatch(path);
    }
}