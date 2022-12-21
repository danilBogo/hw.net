using System.ComponentModel.DataAnnotations;
using TimMovie.SharedKernel.BaseEntities;

namespace TimMovie.Core.Entities;

public class Comment : BaseEntity
{
    [Required]
    public Film Film { get; set; }
    
    [Required]
    public User Author { get; set; }

    [Required]
    public string Content { get; set; }
    
    [Required]
    public DateTime Date { get; set; }
}