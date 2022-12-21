using AutoMapper;
using TimMovie.Core.DTO.Messages;
using TimMovie.Core.Entities;
using TimMovie.SharedKernel.Interfaces;
using TimMovie.SharedKernel.Validators;

namespace TimMovie.Core.Services.Messages;

public class MessageService
{
    private readonly IRepository<Message> _messageRepository;
    private readonly IMapper _mapper;

    public MessageService(IRepository<Message> messageRepository, IMapper mapper)
    {
        _messageRepository = messageRepository;
        _mapper = mapper;
    }

    public IEnumerable<MessageDto> GetAllMessagesByGroup(string groupName)
    {
        var messages = _messageRepository.Query
            .Where(message => message.GroupName == groupName)
            .OrderBy(message => message.Date)
            .ToList();
        return _mapper.Map<IEnumerable<MessageDto>>(messages);
    }
    
    public async Task<MessageDto> CreateNewMessageToUserAsync(NewMessageDto newMessage)
    {
        return await CreateNewMessageAsync(newMessage, true);
    }

    public async Task<MessageDto> CreateNewMessageToSupportAsync(NewMessageDto newMessage)
    {
        return await CreateNewMessageAsync(newMessage, false);
    }
    
    private async Task<MessageDto> CreateNewMessageAsync(NewMessageDto newMessage, bool toUser)
    {
        ArgumentValidator.ThrowExceptionIfNull(newMessage, nameof(newMessage));

        var message = _mapper.Map<Message>(newMessage);
        message.Date = DateTime.UtcNow;
        message.ToUser = toUser;

        await _messageRepository.AddAsync(message);
        await _messageRepository.SaveChangesAsync();

        return _mapper.Map<MessageDto>(message);
    }
}