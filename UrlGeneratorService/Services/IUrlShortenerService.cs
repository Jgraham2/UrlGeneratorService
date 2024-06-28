
namespace UrlGeneratorService.Services
{
    public interface IUrlShortenerService
    {
        Task<string> ShortenUrl(string originalUrl);
        Task<string?> GetOriginalUrl(string shortUrl);
    }
}
