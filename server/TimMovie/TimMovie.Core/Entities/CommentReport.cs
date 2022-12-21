using System.ComponentModel.DataAnnotations;
using TimMovie.SharedKernel.BaseEntities;

namespace TimMovie.Core.Entities;

public class CommentReport : BaseEntity
{
    [Required]
    public Comment Comment { get; set; }
    
    [Required]
    public User User { get; set; }
}