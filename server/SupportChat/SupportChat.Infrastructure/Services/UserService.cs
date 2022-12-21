using SupportChat.Domain.Models;

namespace SupportChat.Infrastructure.Services;

public static class UserService
{
    public static readonly HashSet<AdminUser> HashSet = new();
}