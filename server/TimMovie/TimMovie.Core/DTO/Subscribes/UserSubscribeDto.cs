namespace TimMovie.Core.DTO.Subscribes;

public class UserSubscribeDto
{
    public Guid SubscribeId { get; set; }
    public string SubscribeName { get; set; }
    public string SubscribeDescription { get; set; }
    public DateTime StartDay { get; set; }
    public DateTime EndDate { get; set; }
}