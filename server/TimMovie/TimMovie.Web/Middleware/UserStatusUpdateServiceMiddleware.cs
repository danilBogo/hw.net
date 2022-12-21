using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TimMovie.Core.Entities;
using TimMovie.Core.Entities.Enums;
using TimMovie.Core.Services.Films;
using TimMovie.Web.Extensions;

namespace TimMovie.Web.Middleware;

public class UserStatusUpdateServiceMiddleware
{
    private readonly RequestDelegate _next;

    public UserStatusUpdateServiceMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, 
        UserManager<User> userManager,
        FilmService filmService)
    {
        var path = context.Request.Path.Value;
        var userId = context.User.GetUserId();
        if (userId is not null)
        {
            if (path is not null)
            {
                Guid filmId;
                if (path.Contains("Film/") && 
                    Guid.TryParse(path.Split("/")[^1], out filmId) ||
                    path.Contains("UpdateUserStatusWatchingFilm") && 
                    Guid.TryParse(context.Request.Query.FirstOrDefault().Value, out filmId))
                {
                    var film = filmService.GetDbFilmById(filmId);
                    if (film is not null)
                        await UpdateUserStatus(userId.Value, UserStatusEnum.Watching, userManager, film);
                }
                else
                    await UpdateUserStatus(userId.Value, UserStatusEnum.Online, userManager);
            }

            if (path is null)
                await UpdateUserStatus(userId.Value, UserStatusEnum.Online, userManager);
        }

        await _next.Invoke(context);
    }

    private async Task UpdateUserStatus(Guid userId, UserStatusEnum userStatusEnum, 
        UserManager<User> userManager, Film? film = null)
    {
        var dbUser = await userManager.Users.Include(u => u.Status).FirstOrDefaultAsync(u => u.Id == userId);
        if (dbUser is not null)
        {
            dbUser.Status ??= new UserStatus();
            if (film is not null)
                dbUser.WatchingFilm = film;
            dbUser.Status.UserStatusEnum = userStatusEnum;
            dbUser.Status.DateLastChange = DateTime.Now;
            await userManager.UpdateAsync(dbUser);
        }
    }
}