using System.ComponentModel.DataAnnotations;
using TimMovie.SharedKernel.BaseEntities;

namespace TimMovie.Core.Entities;

public class Country : BaseEntity
{
    [Required]
    [MaxLength(100)]
    public string Name { get; set; }

    [System.Text.Json.Serialization.JsonIgnore]
    public List<User> Users { get; set; }
    
    [System.Text.Json.Serialization.JsonIgnore]
    public List<Film> Films { get; set; }
}