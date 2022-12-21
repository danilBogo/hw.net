using TimMovie.SharedKernel.BaseEntities;

namespace TimMovie.Core.Entities;

public class Producer : PersonBaseEntity
{
    public List<Film> Films { get; set; }
}