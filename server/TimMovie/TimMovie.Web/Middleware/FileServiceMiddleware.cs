using TimMovie.Core.Interfaces;

namespace TimMovie.Web.Middleware;

public class FileServiceMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IFileService _fileService;

    public FileServiceMiddleware(RequestDelegate next, IFileService fileService)
    {
        _next = next;
        _fileService = fileService;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var path = context.Request.Path.Add(context.Request.QueryString);
        
        if (_fileService.IsRequestToFileServer(path))
        {
            var uri = _fileService.GetUriToFileServer(path);
            context.Response.Redirect(uri);
            return;
        }
        
        await _next.Invoke(context);
    }
}