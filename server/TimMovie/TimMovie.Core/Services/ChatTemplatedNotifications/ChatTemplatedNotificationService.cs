using TimMovie.Core.Entities;
using TimMovie.Core.Specifications.InheritedSpecifications.ChatTemplatedNotificationSpec;
using TimMovie.SharedKernel.Interfaces;

namespace TimMovie.Core.Services.ChatTemplatedNotifications;

public class ChatTemplatedNotificationService
{
    private readonly IRepository<ChatTemplatedNotification> _templateRepository;

    public ChatTemplatedNotificationService(IRepository<ChatTemplatedNotification> templateRepository)
    {
        _templateRepository = templateRepository;
    }

    public string? GetValueTemplateByName(string name)
    {
        return _templateRepository.Query
            .FirstOrDefault(new TemplatedNotificationByNameSpec(name))?
            .Value;
    }
}