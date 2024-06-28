using System;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using UrlGeneratorService.Models;
using UrlGeneratorService.Repositories;
using UrlGeneratorService.Services;

namespace UrlGeneratorService.Tests.Services
{
    [TestFixture]
    public class UrlShortenerServiceTests
    {
        private Mock<IUrlMappingRepository> _urlMappingRepositoryMock;
        private UrlShortenerService _urlShortenerService;

        [SetUp]
        public void Setup()
        {
            _urlMappingRepositoryMock = new Mock<IUrlMappingRepository>();
            _urlShortenerService = new UrlShortenerService(_urlMappingRepositoryMock.Object);
        }

        [Test]
        public void ShortenUrl_ShouldThrowArgumentException_WhenOriginalUrlIsEmpty()
        {
            // Arrange
            var originalUrl = string.Empty;

            // Act & Assert
            var exception = Assert.ThrowsAsync<ArgumentException>(async () =>
                await _urlShortenerService.ShortenUrl(originalUrl));

            Assert.AreEqual("URL cannot be empty.", exception.Message);
        }

        [Test]
        public void ShortenUrl_ShouldThrowArgumentException_WhenOriginalUrlIsInvalid()
        {
            // Arrange
            var originalUrl = "invalid-url";

            // Act & Assert
            var exception = Assert.ThrowsAsync<ArgumentException>(async () =>
                await _urlShortenerService.ShortenUrl(originalUrl));

            Assert.AreEqual("Needs to be a real URL.", exception.Message);
        }

        [Test]
        public async Task ShortenUrl_ShouldReturnExistingShortUrl_WhenOriginalUrlAlreadyShortened()
        {
            // Arrange
            var originalUrl = "http://example.com/images";
            var domain = "http://example.com";
            var shortPath = "abc123";
            var originalPath = "images";

            var existingMapping = new UrlMapping
            {
                Domain = domain,
                OriginalPath = originalPath,
                ShortPath = shortPath
            };

            _urlMappingRepositoryMock
                .Setup(repo => repo.GetUrlMappingByOriginalPathAsync(domain, originalPath))
                .ReturnsAsync(existingMapping);

            // Act
            var result = await _urlShortenerService.ShortenUrl(originalUrl);

            // Assert
            Assert.AreEqual($"{domain}/{shortPath}", result);
        }

        [Test]
        public async Task GetOriginalUrl_ShouldReturnOriginalUrl_WhenShortUrlExists()
        {
            // Arrange
            var domain = "http://example.com";
            var shortPath = "abc123";
            var originalPath = "images";
            var shortUrl = $"{domain}/{shortPath}";
            var originalUrl = $"{domain}/{originalPath}";

            var urlMapping = new UrlMapping
            {
                Domain = domain,
                ShortPath = shortPath,
                OriginalPath = originalPath
            };

            _urlMappingRepositoryMock
                .Setup(repo => repo.GetUrlMappingByShortPathAsync(domain, shortPath))
                .ReturnsAsync(urlMapping);

            // Act
            var result = await _urlShortenerService.GetOriginalUrl(shortUrl);

            // Assert
            Assert.AreEqual(originalUrl, result);
        }

        [Test]
        public async Task GetOriginalUrl_ShouldReturnNull_WhenShortUrlDoesNotExist()
        {
            // Arrange
            var domain = "http://example.com";
            var shortPath = "unknown";
            var shortUrl = $"{domain}/{shortPath}";

            _urlMappingRepositoryMock
                .Setup(repo => repo.GetUrlMappingByShortPathAsync(domain, shortPath))
                .ReturnsAsync((UrlMapping?)null);

            // Act
            var result = await _urlShortenerService.GetOriginalUrl(shortUrl);

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public void GenerateShortPath_ShouldReturnSixCharacterString()
        {
            // Arrange
            var input = "test-input";

            // Act
            var shortPath = _urlShortenerService.GenerateShortPath(input);

            // Assert
            Assert.AreEqual(6, shortPath.Length);
        }
    }
}
