using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using UrlGeneratorService.Services;

namespace UrlGeneratorService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UrlShortenerController : ControllerBase
    {
        private readonly IUrlShortenerService _urlShortenerService;

        public UrlShortenerController(IUrlShortenerService urlShortenerService)
        {
            _urlShortenerService = urlShortenerService;
        }

        [HttpPost]
        public async Task<IActionResult> ShortenUrl([FromBody] string originalUrl)
        {
            if (string.IsNullOrWhiteSpace(originalUrl))
            {
                return BadRequest(new { message = "The URL cannot be empty." });
            }

            try
            {
                var shortUrl = await _urlShortenerService.ShortenUrl(originalUrl);
                return CreatedAtAction(nameof(GetOriginalUrl), new { shortUrl }, new { shortUrl });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetOriginalUrl([FromQuery] string shortUrl)
        {
            if (string.IsNullOrWhiteSpace(shortUrl))
            {
                return BadRequest(new { message = "The short URL cannot be empty." });
            }

            var originalUrl = await _urlShortenerService.GetOriginalUrl(shortUrl);
            if (originalUrl == null)
            {
                return NotFound(new { message = "URL does not exist." });
            }
            return Ok(new { originalUrl });
        }

        [HttpGet("redirect")]
        public async Task<IActionResult> RedirectShortUrl([FromQuery] string shortUrl)
        {
            if (string.IsNullOrWhiteSpace(shortUrl))
            {
                return BadRequest(new { message = "The short URL cannot be empty." });
            }

            var originalUrl = await _urlShortenerService.GetOriginalUrl(shortUrl);
            if (originalUrl == null)
            {
                return NotFound(new { message = "URL does not exist." });
            }

            return Ok(new { originalUrl });
        }
    }
}
