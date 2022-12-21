namespace TimMovie.Core.DTO.Producer;

public class ProducerDto
{
    public Guid Id { get; set; }
    
    public string Name { get; set; }
    
    public string? Surname { get; set; }
    
    public string? Photo { get; set; }
}