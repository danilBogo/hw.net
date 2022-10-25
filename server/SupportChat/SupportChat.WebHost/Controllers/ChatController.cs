using Microsoft.AspNetCore.Mvc;
using SupportChat.Domain.Models;
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
        return await _messageService.GetMessageHistoryAsync();
    }
    
    // [HttpPost]
    // public async Task<IActionResult> Message([FromForm]string content)
    // {
    //     var message = new Message
    //     {
    //         Content = content,
    //         Time = DateTime.Now
    //     };
    //     await _messageService.AddMessageAsync(message);
    //     return Ok();
    // }
}

