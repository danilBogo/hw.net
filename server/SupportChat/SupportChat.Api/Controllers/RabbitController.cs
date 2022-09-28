using Microsoft.AspNetCore.Mvc;
using SupportChat.Core.Entities;
using SupportChat.Core.Interfaces;
using SupportChat.Core.Services;

namespace SupportChat.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class RabbitController : ControllerBase
{
    private readonly IRabbitMqService _mqService;

    public RabbitController(IRabbitMqService mqService)
    {
        _mqService = mqService;
    }
    // /rabbit?msg=123
    [HttpGet]
    public IActionResult Message(string msg)
    {
        _mqService.SendMessage(msg);
        return Ok($"Сообщение {msg} отправлено");
    }
}