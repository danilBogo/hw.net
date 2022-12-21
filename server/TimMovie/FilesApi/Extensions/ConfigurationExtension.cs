namespace FilesApi.Extensions;

public static class ConfigurationExtension
{
    public static string GetRelativePathToFileContents(this IConfiguration configuration)
    {
        IfNullThrowException(configuration, nameof(configuration));

        return configuration["RelativePathToFileContents"];
    }
    
    public static string GetRelativePathToFilms(this IConfiguration configuration)
    {
        IfNullThrowException(configuration, nameof(configuration));

        return configuration["RelativePathToFilms"];
    }
    
    public static string GetRelativePathToUserPhoto(this IConfiguration configuration)
    {
        IfNullThrowException(configuration, nameof(configuration));

        return configuration["RelativePathToUserPhoto"];
    }
    
    public static string GetRelativePathToActors(this IConfiguration configuration)
    {
        IfNullThrowException(configuration, nameof(configuration));

        return configuration["RelativePathToActors"];
    }
    
    public static string GetRelativePathToBanners(this IConfiguration configuration)
    {
        IfNullThrowException(configuration, nameof(configuration));

        return configuration["RelativePathToBanners"];
    }
    
    public static string GetRelativePathToProducers(this IConfiguration configuration)
    {
        IfNullThrowException(configuration, nameof(configuration));

        return configuration["RelativePathToProducers"];
    }

    private static void IfNullThrowException(object obj, string paramName)
    {
        if (obj is null)
        {
            throw new ArgumentNullException(paramName);
        }
    }
}