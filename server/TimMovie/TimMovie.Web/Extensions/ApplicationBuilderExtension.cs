using TimMovie.Web.Middleware;

namespace TimMovie.Web.Extensions;

public static class ApplicationBuilderExtension
{
    public static IApplicationBuilder UseFileService(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<FileServiceMiddleware>();
    } 
    
    public static IApplicationBuilder UseUserStatusUpdateService(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<UserStatusUpdateServiceMiddleware>();
    }
    
    public static IApplicationBuilder UseUserStatusDeleteService(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<UserStatusDeleteServiceMiddleware>();
    }
    
    public static IApplicationBuilder UseBannedUserService(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<BannedUserMiddleware>();
    }
}