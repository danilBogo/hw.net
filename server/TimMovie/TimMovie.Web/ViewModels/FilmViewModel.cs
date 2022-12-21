using TimMovie.Core.DTO.Comments;
using TimMovie.Core.Entities;

namespace TimMovie.Web.ViewModels;

public class FilmViewModel
{
    public Guid Id { get; set; }
    public string Title { get; set; }

    public int Year { get; set; }

    public string? Description { get; set; }
    public Country? Country { get; set; }

    public double? Rating { get; set; }
    public int? GradesNumber { get; set; }
    public string? FilmLink { get; set; }
    public List<Producer> Producers { get; set; }
    public List<Actor> Actors { get; set; }
    public List<Genre> Genres { get; set; }
    public List<CommentsDto> Comments { get; set; }
    public bool IsGradeSet { get; set; }
    public bool IsAddedToWatchLater { get; set; }
    public string? PathToUserPhoto { get; set; }
    public bool IsAvailable { get; set; }
}