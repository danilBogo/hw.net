using System.ComponentModel.DataAnnotations;
using TimMovie.SharedKernel.BaseEntities;

namespace TimMovie.Core.Entities;

public class Notification : BaseEntity
{
    [Required]
    public string Content { get; set; }
    
    public List<User> Users { get; set; }
    
    public DateTime? Date { get; set; }

}