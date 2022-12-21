using System.ComponentModel.DataAnnotations;
using TimMovie.SharedKernel.BaseEntities;

namespace TimMovie.Core.Entities;

public class Message: BaseEntity
{
    [Required]
    public bool ToUser { get; set; }
    public User? Sender { get; set; }
    [Required]
    public string Content { get; set; }
    [Required]
    public DateTime Date { get; set; }
    [Required]
    public string GroupName { get; set; }
}