using MassTransit;
using Microsoft.AspNetCore.SignalR;
using SupportChat.Domain.Dto;
using SupportChat.Domain.Models;
using SupportChat.Domain.Models.Files;
using SupportChat.Infrastructure.Services;

namespace SupportChat.WebHost.SignalR;

public class ChatHub : Hub
{
    private readonly IBus _bus;

    public ChatHub(IBus bus)
    {
        _bus = bus;
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var admin = UserService.HashSet.FirstOrDefault(e => e.AdminId == Context.ConnectionId);
        if (admin is not null)
        {
            admin.AdminId = null;
            admin.AdminName = null;
            var freeAdmin = UserService.HashSet.FirstOrDefault(e => e.AdminId is not null && e.UserId is null);
            var freeUser = UserService.HashSet.FirstOrDefault(e => e.UserId is not null && e.AdminId is null);
            if (freeAdmin is not null && freeUser is not null)
            {
                freeAdmin.UserId = freeUser.UserId;
                freeAdmin.UserName = freeUser.UserName;
                freeUser.AdminId = freeAdmin.AdminId;
                freeUser.AdminName = freeAdmin.AdminName;
                await Clients.Client(freeUser.UserId!)
                    .SendAsync("NotificateAdminUser", "К вам подключился администратор");
                await Clients.Client(freeUser.AdminId!)
                    .SendAsync("NotificateAdminUser", "Вы подключились к пользователю");
            }
            return;
        }
        
        var user = UserService.HashSet.FirstOrDefault(e => e.UserId == Context.ConnectionId);
        if (user is not null)
        {
            user.UserId = null;
            user.UserName = null;
            var freeAdmin = UserService.HashSet.FirstOrDefault(e => e.AdminId is not null && e.UserId is null);
            var freeUser = UserService.HashSet.FirstOrDefault(e => e.UserId is not null && e.AdminId is null);
            if (freeAdmin is not null && freeUser is not null)
            {
                freeAdmin.UserId = freeUser.UserId;
                freeAdmin.UserName = freeUser.UserName;
                freeUser.AdminId = freeAdmin.AdminId;
                freeUser.AdminName = freeAdmin.AdminName;
                await Clients.Client(freeUser.UserId!)
                    .SendAsync("NotificateAdminUser", "К вам подключился администратор");
                await Clients.Client(freeUser.AdminId!)
                    .SendAsync("NotificateAdminUser", "Вы подключились к пользователю");
            }
        }
    }

    public async Task Send(string message, string userName, string userId, Metadata metadata)
    {
        var interlocutor = userName;
        
        var value1 = UserService.HashSet.FirstOrDefault(e => e.AdminId == userId);
        if (value1?.UserId != null)
        {
            interlocutor = value1.UserName;
            await Clients.Client(value1.UserId).SendAsync("Send", message, metadata);
        }

        var value2 = UserService.HashSet.FirstOrDefault(e => e.UserId == userId);
        if (value2?.AdminId != null)
        {
            interlocutor = value2.AdminName;
            await Clients.Client(value2.AdminId).SendAsync("Send", message, metadata);
        }
        
        await Clients.Client(userId).SendAsync("Send", message, metadata);
        
        if (metadata.FileId != null)
        {
            await _bus.Publish(new MessageMetadataDto
            {
                Content = message,
                UserName = userName,
                InterlocutorName = interlocutor,
                Time = DateTime.Now,
                Metadata = metadata
            });
        }
    }

    public async Task RegisterUser(string name, string userId)
    {
        if (name.Contains("admin"))
        {
            var admin = UserService.HashSet.FirstOrDefault(e => e.AdminName == name);
            if (admin is not null && admin.AdminId != userId)
                admin.AdminId = userId;
            if (UserService.HashSet.FirstOrDefault(e => e.AdminId == userId) is null)
            {
                var freeUser = UserService.HashSet.FirstOrDefault(e => e.UserId is not null && e.AdminId is null);
                if (freeUser is not null)
                {
                    freeUser.AdminId = userId;
                    freeUser.AdminName = name;
                    await Clients.Client(freeUser.UserId!)
                        .SendAsync("NotificateAdminUser", "К вам подключился администратор");
                    await Clients.Client(freeUser.AdminId)
                        .SendAsync("NotificateAdminUser", "Вы подключились к пользователю");
                }
                else
                {
                    UserService.HashSet.Add(new AdminUser { AdminId = userId, AdminName = name });
                }
            }
        }
        else
        {
            var user = UserService.HashSet.FirstOrDefault(e => e.UserName == name);
            if (user is not null && user.UserId != userId)
                user.AdminId = userId;
            if (UserService.HashSet.FirstOrDefault(e => e.UserId == userId) is null)
            {
                var value = UserService.HashSet.FirstOrDefault(e => e.AdminId is not null && e.UserId is null);
                if (value is not null)
                {
                    value.UserId = userId;
                    value.UserName = name;
                    await Clients.Client(userId).SendAsync("NotificateAdminUser", "К вам подключился администратор");
                    await Clients.Client(value.AdminId!)
                        .SendAsync("NotificateAdminUser", "Вы подключились к пользователю");
                }
                else
                {
                    UserService.HashSet.Add(new AdminUser { UserId = userId, UserName = name});
                }
            }
        }
    }
}