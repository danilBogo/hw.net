using SupportChat.Domain.Models.Files;

namespace SupportChat.Domain.Interfaces;

public interface IMetadataRepository
{
    Task<Metadata> GetMetadataByFileIdAsync(string messageId);

    Task<Metadata> CreateMetadataAsync(Metadata metadata);
}