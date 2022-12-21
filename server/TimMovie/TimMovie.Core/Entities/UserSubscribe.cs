using System.ComponentModel.DataAnnotations;
using TimMovie.SharedKernel.BaseEntities;

namespace TimMovie.Core.Entities;

public class UserSubscribe : BaseEntity
{
    [Required]
    public User SubscribedUser { get; set; }
    
    [Required]
    public Subscribe Subscribe { get; set; }

    [Required]
    public DateTime StartDay { get; set; }
    
    [Required]
    public DateTime EndDate { get; set; }
}