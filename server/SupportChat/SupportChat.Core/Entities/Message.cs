namespace SupportChat.Core.Entities;

using SharedKernel.BaseEntities;

public class Message : BaseEntity
{
    public string Content { get; set; } = null!;
    public DateTime Time { get; set; }
}