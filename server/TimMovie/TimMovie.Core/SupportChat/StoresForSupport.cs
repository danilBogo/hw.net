namespace TimMovie.Core.SupportChat;

public static class StoresForSupport
{
    public static readonly Queue<Guid> FreeSupports = new();
    public static readonly Queue<string> WaitingGroupsWithUser = new();
    public static readonly HashSet<string> DisconnectedGroups = new();
    public static readonly Dictionary<Guid, HashSet<string>> ConnectionIdsByUserId = new();
    public static readonly Dictionary<string, string> GroupNameByConnectionId = new();
}