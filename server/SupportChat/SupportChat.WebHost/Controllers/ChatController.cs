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
    private readonly FileService _fileService;

    public ChatController(MessageService messageService, FileService fileService)
    {
        _messageService = messageService;
        _fileService = fileService;
    }

    [HttpGet]
    public async Task<IEnumerable<Message>> Message()
    {
        return await _messageService.GetMessageHistoryAsync();
    }
}

