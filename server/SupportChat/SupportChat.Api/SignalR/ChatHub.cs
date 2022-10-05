using MassTransit;
using Microsoft.AspNetCore.SignalR;
using SupportChat.Core.Entities;

namespace SupportChat.Api.SignalR;

public class ChatHub : Hub
{
    private readonly IBus _bus;

    public ChatHub(IBus bus)
    {
        _bus = bus;
    }

    public async Task Send(string message)
    {
        await _bus.Publish(new Message {Content = message, Time = DateTime.Now});
        await Clients.All.SendAsync("Send", message);
    }
}