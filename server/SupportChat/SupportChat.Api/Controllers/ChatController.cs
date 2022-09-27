using Microsoft.AspNetCore.Mvc;
using SupportChat.Core.Entities;
using SupportChat.Core.Services;

namespace SupportChat.Api.Controllers;

[ApiController]
public class ChatController : ControllerBase
{
    private readonly MessageService messageService;

    public ChatController(MessageService messageService)
    {
        this.messageService = messageService;
    }

    [HttpGet]
    public async Task<IEnumerable<Message>> GetMessage()
    {
        return await messageService.GetMessageHistoryAsync();
    }
}