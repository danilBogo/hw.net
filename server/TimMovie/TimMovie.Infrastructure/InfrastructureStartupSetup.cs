using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TimMovie.Core.Entities;
using TimMovie.Infrastructure.Database;
using TimMovie.Infrastructure.Identity;

namespace TimMovie.Infrastructure;

public static class InfrastructureStartupSetup
{
    public static void AddDbContext(this IServiceCollection services, string connectionString) =>
        services.AddDbContext<ApplicationContext>(options =>
        {
            options.UseNpgsql(connectionString);
            options.UseOpenIddict();
        });
    
    public static IServiceCollection AddIdentity(this IServiceCollection services)
    {
        services.AddIdentity<User, IdentityRole<Guid>>(opt =>
            {
                opt.Password.RequireUppercase = false;
                opt.Password.RequireDigit = true;
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequiredLength = 8;
                opt.User.RequireUniqueEmail = true;
                opt.SignIn.RequireConfirmedEmail = true;
            })
            .AddErrorDescriber<RussianErrorDescriber>()
            .AddSignInManager()
            .AddEntityFrameworkStores<ApplicationContext>()
            .AddDefaultTokenProviders();
        return services;
    } 
}