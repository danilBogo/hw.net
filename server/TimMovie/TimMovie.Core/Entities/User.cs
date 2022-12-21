using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using TimMovie.SharedKernel.Classes;
using TimMovie.SharedKernel.Interfaces;

namespace TimMovie.Core.Entities;

public class User : IdentityUser<Guid>, IIdHolder<Guid>
{
    [DateOnly] public DateOnly? BirthDate { get; set; }
    public UserStatus? Status { get; set; }
    public Film? WatchingFilm { get; set; }

    [Required]
    [MaxLength(100)]
    public string DisplayName { get; set; }
    public string PathToPhoto { get; set; }
    
    public Country? Country { get; set; }
    public DateTime RegistrationDate { get; set; }
    public List<UserFilmWatched> WatchedFilms { get; set; }
    public List<Notification> Notifications { get; set; }
    public List<Film> FilmsWatchLater { get; set; }
}