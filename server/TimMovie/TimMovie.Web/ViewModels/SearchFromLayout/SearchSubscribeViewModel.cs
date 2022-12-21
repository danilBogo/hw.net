using TimMovie.Core.DTO.Subscribes;

namespace TimMovie.Web.ViewModels.SearchFromLayout;

public class SearchSubscribeViewModel
{
    public IEnumerable<SubscribeDto>? Subscribes { get; set; }
    
    public IEnumerable<UserSubscribeDto>? UserSubscribes { get; set; }
}