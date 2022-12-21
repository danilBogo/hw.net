using TimMovie.Core.DTO.Notifications;

namespace TimMovie.Core.Interfaces;

public interface INotificationService 
{
    public IEnumerable<NotificationDto> GetAllUserNotifications(Guid userId);
}