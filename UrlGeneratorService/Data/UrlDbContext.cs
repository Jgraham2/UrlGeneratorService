using Microsoft.EntityFrameworkCore;
using UrlGeneratorService.Models;

namespace UrlGeneratorService.Data
{
    public class UrlDbContext : DbContext
    {
        public UrlDbContext(DbContextOptions<UrlDbContext> options) : base(options)
        {
        }

        public DbSet<UrlMapping> UrlMappings { get; set; }
    }
}
