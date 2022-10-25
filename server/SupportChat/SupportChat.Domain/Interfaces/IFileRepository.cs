using MongoDB.Driver;
using SharedKernel.Files;

namespace SupportChat.Domain.Interfaces;

public interface IFileRepository
{
    Task<IEnumerable<T>> GetCollectionWithFilter<T>(Guid messageId) where T : FileMetadata;
}