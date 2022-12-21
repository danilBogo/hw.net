using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TimMovie.Core.Entities;
using TimMovie.Core.Entities.Enums;
using TimMovie.Infrastructure.Database;
using TimMovie.Web.Extensions;

namespace TimMovie.Web.Middleware;

public class UserStatusDeleteServiceMiddleware
{
    private readonly RequestDelegate _next;
    private const int CountMinutes = 5;

    public UserStatusDeleteServiceMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context,  UserManager<User> userManager)
    {
        var users = userManager.Users
            .Include(u => u.Status)
            .Where(u => u.Status != null && u.Status.UserStatusEnum != UserStatusEnum.Offline)
            .ToList();
        foreach (var user in users)
        {
            if (DateTime.Now - user.Status!.DateLastChange <= TimeSpan.FromMinutes(CountMinutes)) continue;
            user.Status.UserStatusEnum = UserStatusEnum.Offline;
            user.Status.DateLastChange = DateTime.Now;
            await userManager.UpdateAsync(user);
        }

        await _next.Invoke(context);
    }
}