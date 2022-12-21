using TimMovie.SharedKernel.BaseEntities;

namespace TimMovie.Core.Entities;

public class ChatTemplatedNotification: BaseEntity
{
    public string Name { get; set; }
    public string Value { get; set; }
}