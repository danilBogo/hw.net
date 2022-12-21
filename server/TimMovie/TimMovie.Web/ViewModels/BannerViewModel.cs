using TimMovie.Core.Entities;

namespace TimMovie.Web.ViewModels;

public class BannerViewModel
{
    public string? Description { get; set; }

    public string Image { get; set; }

    public Film Film { get; set; }

    public BannerViewModel(string? description, string image, Film film)
    {
        Description = description;
        Image = image;
        Film = film;
    }
}