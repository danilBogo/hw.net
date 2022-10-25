using MassTransit;
using Microsoft.AspNetCore.SignalR;
using SupportChat.Domain.Dto;

namespace SupportChat.WebHost.SignalR;

public class ChatHub : Hub
{
    private readonly IBus _bus;

    public ChatHub(IBus bus)
    {
        _bus = bus;
    }

    public async Task Send(string message, IFormFile formFile, string jsonMetadata)
    {
        await _bus.Publish(new MessageDto
        {
            Content = message,
            Time = DateTime.Now,
            FormFile = formFile,
            JsonMetadata = jsonMetadata
        });
        await Clients.All.SendAsync("Send", message);
    }
}