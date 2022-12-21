namespace TimMovie.Web.ViewModels.FilmCard;

public class FilmCardViewModel
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public int Year { get; set; }
    public string CountryName { get; set; } = null!;
    public string Image { get; set; } = null!;
    public string FirstGenreName { get; set; } = null!;
    public double? Rating { get; set; }
    public bool IsFree { get; set; }
    public bool IsGradeSet { get; set; }
    public bool IsAddedToWatchLater { get; set; }
}