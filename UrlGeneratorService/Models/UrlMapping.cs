namespace UrlGeneratorService.Models
{
    public class UrlMapping
    {
        public int Id { get; set; }
        public required string Domain { get; set; }
        public required string OriginalPath { get; set; }
        public required string ShortPath { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
