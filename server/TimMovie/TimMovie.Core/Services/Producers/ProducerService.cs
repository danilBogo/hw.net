using TimMovie.Core.Entities;
using TimMovie.Core.Specifications.InheritedSpecifications.ProducerSpec;
using TimMovie.SharedKernel.Interfaces;

namespace TimMovie.Core.Services.Producers;

public class ProducerService
{
    private readonly IRepository<Producer> _producerRepository;

    public ProducerService(IRepository<Producer> producerRepository)
    {
        _producerRepository = producerRepository;
    }

    public IEnumerable<Producer> GetProducersByNamePart(string? namePart, int count = int.MaxValue) =>
        _producerRepository.Query
            .Where(new ProducerByNamePartSpec(namePart))
            .Take(count);
}