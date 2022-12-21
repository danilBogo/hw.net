using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using SupportChat.Domain.Models;
using SupportChat.Domain.Models.Files;
using SupportChat.Infrastructure.Services;

namespace SupportChat.WebHost.Controllers;

[ApiController]
[Route("[controller]")]
public class ChatController : ControllerBase
{
    private readonly MessageService _messageService;

    public ChatController(MessageService messageService)
    {
        _messageService = messageService;
    }

    [HttpGet]
    public async Task<IEnumerable<Message>> Message(string userName)
    {
        var interlocutor = userName.Contains("admin")
            ? UserService.HashSet
                .FirstOrDefault(e => e.AdminName == userName && e.UserName is not null)?.UserName
            : UserService.HashSet
                .FirstOrDefault(e => e.UserName == userName && e.AdminName is not null)?.AdminName;
        return await _messageService.GetMessageHistoryByUserNameAndInterlocutorNameAsync(userName, interlocutor);
    }
}