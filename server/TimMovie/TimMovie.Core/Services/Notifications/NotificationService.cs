using AutoMapper;
using TimMovie.Core.DTO.Notifications;
using TimMovie.Core.Entities;
using TimMovie.Core.Interfaces;
using TimMovie.SharedKernel.Interfaces;

namespace TimMovie.Core.Services.Notifications;

public class NotificationService : INotificationService
{
    private readonly IRepository<Notification> _notificationRepository;
    private readonly IMapper _mapper;

    public NotificationService(IRepository<Notification> notificationRepository, IMapper mapper)
    {
        _notificationRepository = notificationRepository;
        _mapper = mapper;
    }

    public IEnumerable<NotificationDto> GetAllUserNotifications(Guid userId)
    {
        var notifications = _notificationRepository.Query
            .Where(notification => notification.Users.Any(user => user.Id == userId))
            .ToList();
        return _mapper.Map<IEnumerable<NotificationDto>>(notifications);
    }
}