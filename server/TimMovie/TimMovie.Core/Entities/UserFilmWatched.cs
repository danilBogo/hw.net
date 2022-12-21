using System.ComponentModel.DataAnnotations;
using TimMovie.SharedKernel.BaseEntities;

namespace TimMovie.Core.Entities;

public class UserFilmWatched : BaseEntity
{
    [Required]
    public User WatchedUser { get; set; }
    
    [Required]
    public Film Film { get; set; }
    
    [Range(1,10)]
    public int? Grade { get; set; }
    
    [Required]
    public DateTime Date { get; set; }
}