using Microsoft.Extensions.DependencyInjection;

namespace TimMovie.Core.Classes;

public class Lazier<T> : Lazy<T> where T : class
{
    public Lazier(IServiceProvider provider)
        : base(provider.GetRequiredService<T>)
    {
    }
}