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

    public async Task Send(string message, FileMetadata fileMetadata)
    {
        await _bus.Publish(new MessageFileMetadataDto
        {
            Content = message,
            Time = DateTime.Now,
            FileMetadata = fileMetadata
        });
        await Clients.All.SendAsync("Send", message, fileMetadata);
    }
}