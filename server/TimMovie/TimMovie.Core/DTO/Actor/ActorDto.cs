namespace TimMovie.Core.DTO.Actor;

public class ActorDto
{
    public Guid Id { get; set; }
    
    public string Name { get; set; }
    
    public string? Surname { get; set; }
    
    public string? Photo { get; set; }
}