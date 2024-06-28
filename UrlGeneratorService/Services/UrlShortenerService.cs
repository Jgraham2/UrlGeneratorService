using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using UrlGeneratorService.Models;
using UrlGeneratorService.Repositories;

namespace UrlGeneratorService.Services
{
    public class UrlShortenerService : IUrlShortenerService
    {
        private readonly IUrlMappingRepository _urlMappingRepository;
        private const int MaxRetries = 5;

        public UrlShortenerService(IUrlMappingRepository urlMappingRepository)
        {
            _urlMappingRepository = urlMappingRepository;
        }

        public async Task<string> ShortenUrl(string originalUrl)
        {
            if (string.IsNullOrWhiteSpace(originalUrl))
            {
                throw new ArgumentException("URL cannot be empty.");
            }

            if (!Uri.IsWellFormedUriString(originalUrl, UriKind.Absolute) || originalUrl.Contains("localhost"))
            {
                throw new ArgumentException("Needs to be a real URL.");
            }

            var uri = new Uri(originalUrl);
            var domain = $"{uri.Scheme}://{uri.Host}";
            var originalPath = uri.PathAndQuery.TrimStart('/');

            var existingMapping = await _urlMappingRepository.GetUrlMappingByOriginalPathAsync(domain, originalPath);
            if (existingMapping != null)
            {
                return $"{domain}/{existingMapping.ShortPath}";
            }

            string shortPath;
            for (int attempt = 0; attempt < MaxRetries; attempt++)
            {
                shortPath = GenerateShortPath(originalUrl + attempt);
                var existingShortPathMapping = await _urlMappingRepository.GetUrlMappingByShortPathAsync(domain, shortPath);
                if (existingShortPathMapping == null)
                {
                    var urlMapping = new UrlMapping
                    {
                        Domain = domain,
                        OriginalPath = originalPath,
                        ShortPath = shortPath,
                        CreatedAt = DateTime.UtcNow
                    };

                    await _urlMappingRepository.AddUrlMappingAsync(urlMapping);
                    return $"{domain}/{shortPath}";
                }
            }

            throw new Exception("Failed to generate a unique short path. Please try again.");
        }

        public async Task<string?> GetOriginalUrl(string shortUrl)
        {
            var uri = new Uri(shortUrl);
            var domain = $"{uri.Scheme}://{uri.Host}";
            var shortPath = uri.PathAndQuery.TrimStart('/');

            var urlMapping = await _urlMappingRepository.GetUrlMappingByShortPathAsync(domain, shortPath);
            return urlMapping != null ? $"{domain}/{urlMapping.OriginalPath}" : null;
        }

        public string GenerateShortPath(string input)
        {
            using (var sha256 = SHA256.Create())
            {
                var hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
                return Convert.ToBase64String(hash).Replace("/", "-").Replace("+", "_").Substring(0, 6);
            }
        }
    }
}
