using SupportChat.Domain.Interfaces;
using SupportChat.Domain.Models.Files;

namespace SupportChat.Infrastructure.Services;

public class MetadataService
{
    private readonly IMetadataRepository _metadataRepository;

    public MetadataService(IMetadataRepository metadataRepository)
    {
        _metadataRepository = metadataRepository;
    }

    public async Task<Metadata> GetMetadataByFileIdAsync(string fileId) =>
        await _metadataRepository.GetMetadataByFileIdAsync(fileId);

    public async Task<Metadata> CreateMetadataAsync(Metadata metadata) =>
        await _metadataRepository.CreateMetadataAsync(metadata);
}