using System.ComponentModel.DataAnnotations;
using TimMovie.SharedKernel.BaseEntities;

namespace TimMovie.Core.Entities;

public class Genre : BaseEntity
{
    [Required]
    [MaxLength(50)]
    public string Name { get; set; }
    
    public List<Film> Films { get; set; }
    
    public List<Subscribe> Subscribes { get; set; }
}