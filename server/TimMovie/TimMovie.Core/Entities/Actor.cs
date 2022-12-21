using TimMovie.SharedKernel.BaseEntities;

namespace TimMovie.Core.Entities;

public class Actor : PersonBaseEntity
{
    public List<Film> Films { get; set; }
}