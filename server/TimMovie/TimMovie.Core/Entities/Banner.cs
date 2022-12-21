using System.ComponentModel.DataAnnotations;
using TimMovie.SharedKernel.BaseEntities;

namespace TimMovie.Core.Entities;

public class Banner : BaseEntity
{
    public string? Description { get; set; }

    [Required] public string Image { get; set; }

    [Required] public Film Film { get; set; }
}