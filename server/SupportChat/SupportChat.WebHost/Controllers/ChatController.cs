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
    public async Task<IEnumerable<Message>> Message()
    {
        var result = await _messageService.GetMessageHistoryAsync();
        return await _messageService.GetMessageHistoryAsync();
    }
}

