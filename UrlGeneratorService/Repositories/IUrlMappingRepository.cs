using UrlGeneratorService.Models;
using System.Threading.Tasks;

namespace UrlGeneratorService.Repositories
{
    public interface IUrlMappingRepository
    {
        Task<UrlMapping?> GetUrlMappingByShortPathAsync(string domain, string shortPath);
        Task<UrlMapping?> GetUrlMappingByOriginalPathAsync(string domain, string originalPath);
        Task AddUrlMappingAsync(UrlMapping urlMapping);
    }
}
