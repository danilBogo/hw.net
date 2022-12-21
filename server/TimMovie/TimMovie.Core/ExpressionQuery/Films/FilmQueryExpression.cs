using System.Linq.Expressions;
using TimMovie.Core.Entities;

namespace TimMovie.Core.ExpressionQuery.Films;

public static class FilmQueryExpression
{
    public static readonly Expression<Func<Film, double>> Rating = film =>
        film.UserFilmWatcheds.Select(watched => watched.Grade).Average() ?? 0;

    public static readonly Expression<Func<Film, int>> AmountViews = film =>
        film.UserFilmWatcheds.Count;
}