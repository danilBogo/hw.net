using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using TimMovie.Core.Const;
using TimMovie.Core.DTO.Messages;
using TimMovie.Core.Entities;
using TimMovie.Core.Interfaces;
using TimMovie.Core.Services.ChatTemplatedNotifications;
using TimMovie.Core.Services.Messages;
using TimMovie.Core.SupportChat;
using TimMovie.SharedKernel.Extensions;
using TimMovie.Web.Extensions;

namespace TimMovie.Web.Hubs;

public class ChatHub : Hub
{
    private readonly IUserService _userService;
    private readonly MessageService _messageService;
    private readonly UserManager<User> _userManager;
    private readonly ChatTemplatedNotificationService _templatedNotificationService;

    public ChatHub(
        UserManager<User> userManager,
        IUserService userService,
        ChatTemplatedNotificationService templatedNotificationService,
        MessageService messageService)
    {
        _userManager = userManager;
        _userService = userService;
        _templatedNotificationService = templatedNotificationService;
        _messageService = messageService;
    }

    public async Task SendMessageToUser(string content)
    {
        var support = await _userManager.GetUserAsync(Context.User);
        var groupName = StoresForSupport.GroupNameByConnectionId[Context.ConnectionId];

        var messageDto = await _messageService.CreateNewMessageToUserAsync(new NewMessageDto
        {
           Content = content,
           Sender = support,
           GroupName = groupName
        });

        await Clients.Group(groupName).SendAsync("ReceiveNewMessage", messageDto);
    }

    public async Task SendMessageToSupport(string content)
    {
        var groupName = StoresForSupport.GroupNameByConnectionId[Context.ConnectionId];
        await TryFindSupportIfDisconnectedGroup(groupName);
        
        var user = Context.User.Identity.IsAuthenticated 
            ? await _userManager.GetUserAsync(Context.User) 
            : null;

        var messageDto = await _messageService.CreateNewMessageToSupportAsync(new NewMessageDto
        {
           Content = content,
           Sender = user,
           GroupName = groupName
        });
        
        await Clients.Group(groupName).SendAsync("ReceiveNewMessage", messageDto);
    }

    public async Task StartWorkForSupport()
    {
        var supportId = Context.User.GetUserId().Value;

        await Clients.User(supportId.ToString()).SendAsync("OnStartWork");

        await FindUserOrAddToFree(supportId);
    }

    public async Task StopWorkForSupport()
    {
        var supportId = Context.User.GetUserId().Value;
        StoresForSupport.FreeSupports.Remove(supportId);
        await Clients.User(supportId.ToString()).SendAsync("OnStopWork");
    }

    public async Task SendUserInfoToSupport()
    {
        var userId = Context.User.GetUserId();
        var userInfo = userId is not null
            ? await _userService.GetUserInfoForChat(userId.Value)
            : null;

        var groupName = StoresForSupport.GroupNameByConnectionId[Context.ConnectionId];
        
        await Clients.Group(groupName).SendAsync("PrepareUserInfoInChat", userInfo);
    }

    public void PutInQueueCurrentUser()
    {
        var groupName = StoresForSupport.GroupNameByConnectionId[Context.ConnectionId];
        StoresForSupport.WaitingGroupsWithUser.Enqueue(groupName);
    }
    
    public void AddUserGroupToDisconnected()
    {
        var groupName = StoresForSupport.GroupNameByConnectionId[Context.ConnectionId];
        StoresForSupport.DisconnectedGroups.Add(groupName);
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        if (Context.User.HasRoleClaim(RoleNames.Support))
        {
            await DisconnectSupport();
        }
        else
        {
            await DisconnectUser();
        }
        
        await base.OnDisconnectedAsync(exception);
    }

    public override async Task OnConnectedAsync()
    {
        var userId = Context.User?.GetUserId();
        
        if (Context.User.HasRoleClaim(RoleNames.Support))
        {
            if (UserHasAlreadyConnection(userId.Value))
            {
                await ConnectSupportToCurrentSession();
            }
        }
        else
        {
            var groupName = await AddUserToGroup();
            if (userId is null || !UserHasAlreadyConnection(userId.Value))
            {
                await FindSupportOrAddToWaiting(groupName);
            }
        }
        
        if (userId is not null)
        {
            TryMapIdConnectionToUserId(userId.Value, Context.ConnectionId);
        }

        await base.OnConnectedAsync();
    }

    public async Task DisconnectFromChatWithUser()
    {
        var supportId = Context.User.GetUserId().Value;
        var supportConnections = StoresForSupport.ConnectionIdsByUserId[supportId];
        var groupName = StoresForSupport.GroupNameByConnectionId[supportConnections.First()];

        foreach (var connection in supportConnections)
        {
            await TryRemoveConnectionFromGroupAsync(connection);
        }
        
        await Clients.User(supportId.ToString()).SendAsync("OnDisconnectSupport");

        await Clients.Groups(groupName).SendAsync("TryAddUserToDisconnectedGroups");
        var notification = _templatedNotificationService.GetValueTemplateByName(
            ChatTemplatedNotificationName.CompletedDialogueBySupport); 
        await Clients.Groups(groupName).SendAsync("ShowNotificationForUserChat", notification);

        await FindUserOrAddToFree(supportId);
    }

    private async Task FindUserOrAddToFree(Guid supportId)
    {
        if (!await TryLinkWithWaitingUser(supportId))
        {
            StoresForSupport.FreeSupports.Enqueue(supportId);
        }
    }

    private async Task TryFindSupportIfDisconnectedGroup(string groupName)
    {
        if (StoresForSupport.DisconnectedGroups.Contains(groupName))
        {
            await FindSupportOrAddToWaiting(groupName);

            StoresForSupport.DisconnectedGroups.Remove(groupName);
        }
    }
    
    private async Task FindSupportOrAddToWaiting(string groupName)
    {
        if (!await TryLinkWithFreeSupport(groupName))
        {
            StoresForSupport.WaitingGroupsWithUser.Enqueue(groupName);
        }
    }
    
    private async Task<string> AddUserToGroup()
    {
        var groupName = GetGroupNameForUser();
        await TryAddConnectionToGroup(Context.ConnectionId, groupName);
        return groupName;
    }

    private string GetGroupNameForUser()
    {
        return Context.User?.Identity?.Name ?? Context.ConnectionId;
    }

    private static bool UserHasAlreadyConnection(Guid userId)
    {
        return StoresForSupport.ConnectionIdsByUserId.ContainsKey(userId);
    }
    
    private async Task DisconnectUser()
    {
        var userId = Context.User?.GetUserId();
        var currentConnection = Context.ConnectionId;
        if (userId is not null && 
            StoresForSupport.ConnectionIdsByUserId.TryGetValue(userId.Value, out  var userConnections))
        {
            userConnections.Remove(currentConnection);
            if (userConnections.Count != 0)
            {
                await TryRemoveConnectionFromGroupAsync(currentConnection);
                return;
            }

            StoresForSupport.ConnectionIdsByUserId.Remove(userId.Value);
        }
        
        if (StoresForSupport.GroupNameByConnectionId.TryGetValue(currentConnection, out var groupName))
        {
            await TryRemoveConnectionFromGroupAsync(currentConnection);
            
            var notification = _templatedNotificationService.GetValueTemplateByName(
                ChatTemplatedNotificationName.UserDisconnectFromChat);
            await Clients.Groups(groupName).SendAsync("ShowNotificationInSupportPage", notification);
            
            StoresForSupport.WaitingGroupsWithUser.Remove(groupName);
            StoresForSupport.DisconnectedGroups.Remove(groupName);
        }
    }
    
    private async Task DisconnectSupport()
    {
        var supportId = Context.User.GetUserId().Value;
        var connectionId = Context.ConnectionId;
        if (!StoresForSupport.ConnectionIdsByUserId.TryGetValue(supportId, out  var supportConnections))
        {
            return;
        }

        supportConnections.Remove(connectionId);
        if (supportConnections.Count != 0)
        {
            await TryRemoveConnectionFromGroupAsync(connectionId);
            return;
        }

        StoresForSupport.ConnectionIdsByUserId.Remove(supportId);
        if (StoresForSupport.GroupNameByConnectionId.TryGetValue(connectionId, out var groupName))
        {
            await TryRemoveConnectionFromGroupAsync(connectionId);

            await InformUserAboutSupportSuddenlyDisconnect(groupName);
        }

        StoresForSupport.FreeSupports.Remove(supportId);
    }

    private async Task InformUserAboutSupportSuddenlyDisconnect(string groupName)
    {
        var notification = _templatedNotificationService.GetValueTemplateByName(
            ChatTemplatedNotificationName.SupportSuddenlyDisconnected);
        await Clients.Groups(groupName).SendAsync("ShowNotificationForUserChat", notification);

        if (await TryLinkWithFreeSupport(groupName))
        {
            notification = _templatedNotificationService.GetValueTemplateByName(
                ChatTemplatedNotificationName.TransferToAnotherOperator);
        }
        else
        {
            notification = _templatedNotificationService.GetValueTemplateByName(
                ChatTemplatedNotificationName.AllOperatorsAreBusy);
            await Clients.Groups(groupName).SendAsync("GetInWaitingQueue");
        }

        await Clients.Groups(groupName).SendAsync("ShowNotificationForUserChat", notification);
    }

    private async Task ConnectSupportToCurrentSession()
    {
        var supportId = Context.User.GetUserId().Value;
        
        if (StoresForSupport.FreeSupports.Contains(supportId))
        {
            await Clients.Caller.SendAsync("OnStartWork");
            return;
        }
            
        var connection = StoresForSupport.ConnectionIdsByUserId[supportId]
            .First(s => s != Context.ConnectionId);
        if (StoresForSupport.GroupNameByConnectionId.TryGetValue(connection, out var groupName))
        {
            await ConnectSupportToExistedChat(groupName);
        }
    }

    private async Task ConnectSupportToExistedChat(string groupName)
    {
        await Clients.Caller.SendAsync("OnStartWork");
        await TryAddConnectionToGroup(Context.ConnectionId, groupName);
        await PrepareChatAsync(groupName);
    }

    private async Task<bool> TryRemoveConnectionFromGroupAsync(string connectionId)
    {
        if (StoresForSupport.GroupNameByConnectionId.TryGetValue(connectionId, out var groupName))
        {
            StoresForSupport.GroupNameByConnectionId.Remove(connectionId);
            await Groups.RemoveFromGroupAsync(connectionId, groupName);
            return true;
        }

        return false;
    }

    private async Task<bool> TryAddConnectionToGroup(string connectionId, string groupName)
    {
        if (StoresForSupport.GroupNameByConnectionId.TryAdd(connectionId, groupName))
        {
            await Groups.AddToGroupAsync(connectionId, groupName);
            return true;
        }

        return false;
    }

    private async Task PrepareChatAsync(string groupName)
    {
        await Clients.Group(groupName).SendAsync("PrepareChat", groupName);
        await Clients.Group(groupName).SendAsync("GetUserInfo");
    }
    
    private bool TryMapIdConnectionToUserId(Guid userId, string connectionId)
    {
        StoresForSupport.ConnectionIdsByUserId.TryAdd(userId, new HashSet<string>());
        return StoresForSupport.ConnectionIdsByUserId[userId].Add(connectionId);
    }

    private async Task<bool> TryLinkWithWaitingUser(Guid supportId)
    {
        if (!StoresForSupport.WaitingGroupsWithUser.TryDequeue(out var groupName))
        {
            return false;
        }

        await ConnectSupportToUserGroup(supportId, groupName);

        return true;
    }

    private async Task<bool> TryLinkWithFreeSupport(string groupName)
    {
        if (!StoresForSupport.FreeSupports.TryDequeue(out var supportId))
        {
            return false;
        }

        await ConnectSupportToUserGroup(supportId, groupName);

        return true;
    }
    
    private async Task ConnectSupportToUserGroup(Guid supportId, string groupName)
    {
        var connectionIds = StoresForSupport.ConnectionIdsByUserId[supportId];
        foreach (var supportConnectionId in connectionIds)
        {
            await TryAddConnectionToGroup(supportConnectionId, groupName);
        }

        await PrepareChatAsync(groupName);
    }
}