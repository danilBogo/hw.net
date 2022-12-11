using System.Text;
using MassTransit;
using Microsoft.AspNetCore.SignalR;
using SupportChat.Domain.Dto;
using SupportChat.Domain.Models.Files;

namespace SupportChat.WebHost.SignalR;

public class ChatHub : Hub
{
    private readonly IBus _bus;

    public ChatHub(IBus bus)
    {
        _bus = bus;
    }

    public async Task Send(string message, Metadata metadata)
    {
        if (metadata.FileId != null)
        {
            await _bus.Publish(new MessageMetadataDto
            {
                Content = message,
                Time = DateTime.Now,
                Metadata = metadata
            });
        }
        await Clients.All.SendAsync("Send", message, metadata);
    }
}