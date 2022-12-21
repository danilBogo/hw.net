using TimMovie.Core.Entities;
using TimMovie.Core.Query;
using TimMovie.SharedKernel.Interfaces;

namespace TimMovie.Core.Services.Banners;

public class BannerService
{
    private readonly IRepository<Banner> _bannerRepository;
    private readonly IRepository<Film> _filmsRepository;
    

    public BannerService(IRepository<Banner> bannerRepository, IRepository<Film> filmsRepository)
    {
        _bannerRepository = bannerRepository;
        _filmsRepository = filmsRepository;
    }

    public IEnumerable<Banner> GetBanners()
    {
        var executor = new QueryExecutor<Banner>(_bannerRepository.Query, _bannerRepository);
        executor.IncludeInResult(b => b.Film);
        return executor.GetEntities();
    }

    public string[] GetSmallBannerImages(Guid[] filmIds)
    {
        string[] smallImages = new string[filmIds.Length];
        for (var i = 0; i < smallImages.Length; i++)
            smallImages[i] = _filmsRepository.Query.FirstOrDefault(f => f.Id == filmIds[i]).Image;
        return smallImages;
    }
    
    public string[] GetBigBannerImages(Guid[] filmIds)
    {
        string[] bigImages = new string[filmIds.Length];
        for (var i = 0; i < bigImages.Length; i++)
            bigImages[i] = _bannerRepository.Query.FirstOrDefault(b => b.Film.Id == filmIds[i]).Image;
        return bigImages;
    }
}