namespace TimMovie.Core.DTO.Comments;

public class CommentsDto
{
    public Guid AuthorId { get; set; }
    
    public string AuthorPathToPhoto { get; set; }
    
    public string AuthorDisplayName { get; set; }
    
    public DateTime Date { get; set; }
    
    public string Content { get; set; }
}