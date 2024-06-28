using Microsoft.EntityFrameworkCore;
using UrlGeneratorService.Models;
using UrlGeneratorService.Data;

namespace UrlGeneratorService.Repositories
{
    public class UrlMappingRepository : IUrlMappingRepository
    {
        private readonly UrlDbContext _context;

        public UrlMappingRepository(UrlDbContext context)
        {
            _context = context;
        }

        public async Task<UrlMapping?> GetUrlMappingByShortPathAsync(string domain, string shortPath)
        {
            return await _context.UrlMappings.FirstOrDefaultAsync(u => u.Domain == domain && u.ShortPath == shortPath);
        }

        public async Task<UrlMapping?> GetUrlMappingByOriginalPathAsync(string domain, string originalPath)
        {
            return await _context.UrlMappings.FirstOrDefaultAsync(u => u.Domain == domain && u.OriginalPath == originalPath);
        }

        public async Task AddUrlMappingAsync(UrlMapping urlMapping)
        {
            _context.UrlMappings.Add(urlMapping);
            await _context.SaveChangesAsync();
        }
    }
}
