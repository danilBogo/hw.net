using TimMovie.Web.ViewModels.FilmCard;
using TimMovie.Web.ViewModels.UserSubscribes;

namespace TimMovie.Web.ViewModels.User;

public class UserProfileViewModel
{
    public bool IsOwner { get; set; }
    public UserInfoViewModel UserInfo { get; set; }
    public IEnumerable<FilmCardViewModel> FilmCards { get; set; }
    public IEnumerable<UserSubscribeViewModel> UserSubscribes { get; set; }
    public IEnumerable<string> CountryNames { get; set; } 
}