using System.ComponentModel.DataAnnotations;
using TimMovie.Core.Entities;

namespace TimMovie.Core.DTO.Messages;

public class NewMessageDto
{
    public User? Sender { get; set; }
    public string Content { get; set; }
    public string GroupName { get; set; }
}