using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UrlGeneratorService.Controllers;
using UrlGeneratorService.Services;
using System.Dynamic;

namespace UrlGeneratorService.Tests.Controllers
{
    [TestFixture]
    public class UrlShortenerControllerTests
    {
        private Mock<IUrlShortenerService> _urlShortenerServiceMock;
        private UrlShortenerController _controller;

        [SetUp]
        public void Setup()
        {
            _urlShortenerServiceMock = new Mock<IUrlShortenerService>();
            _controller = new UrlShortenerController(_urlShortenerServiceMock.Object);
        }

        [Test]
        public async Task ShortenUrl_ShouldReturnBadRequest_WhenOriginalUrlIsEmpty()
        {
            // Arrange
            string originalUrl = string.Empty;

            // Act
            var result = await _controller.ShortenUrl(originalUrl) as BadRequestObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(400, result.StatusCode);
        }

        [Test]
        public async Task ShortenUrl_ShouldReturnBadRequest_WhenOriginalUrlIsInvalid()
        {
            // Arrange
            string originalUrl = "invalid-url";
            _urlShortenerServiceMock.Setup(s => s.ShortenUrl(originalUrl)).Throws(new ArgumentException("Needs to be a real URL."));

            // Act
            var result = await _controller.ShortenUrl(originalUrl) as BadRequestObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(400, result.StatusCode);
        }

        [Test]
        public async Task ShortenUrl_ShouldReturnShortUrl_WhenOriginalUrlIsValid()
        {
            // Arrange
            string originalUrl = "http://example.com";
            string shortUrl = "http://short.url/abc123";
            _urlShortenerServiceMock.Setup(s => s.ShortenUrl(originalUrl)).ReturnsAsync(shortUrl);

            // Act
            var result = await _controller.ShortenUrl(originalUrl) as CreatedAtActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(201, result.StatusCode);
        }

        [Test]
        public async Task GetOriginalUrl_ShouldReturnBadRequest_WhenShortUrlIsEmpty()
        {
            // Arrange
            string shortUrl = string.Empty;

            // Act
            var result = await _controller.GetOriginalUrl(shortUrl) as BadRequestObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(400, result.StatusCode);
        }

        [Test]
        public async Task GetOriginalUrl_ShouldReturnNotFound_WhenShortUrlDoesNotExist()
        {
            // Arrange
            string shortUrl = "http://short.url/abc123";
            _urlShortenerServiceMock.Setup(s => s.GetOriginalUrl(shortUrl)).ReturnsAsync((string)null);

            // Act
            var result = await _controller.GetOriginalUrl(shortUrl) as NotFoundObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(404, result.StatusCode);
        }

        [Test]
        public async Task GetOriginalUrl_ShouldReturnOriginalUrl_WhenShortUrlExists()
        {
            // Arrange
            string shortUrl = "http://short.url/abc123";
            string originalUrl = "http://example.com";
            _urlShortenerServiceMock.Setup(s => s.GetOriginalUrl(shortUrl)).ReturnsAsync(originalUrl);

            // Act
            var result = await _controller.GetOriginalUrl(shortUrl) as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
        }

        [Test]
        public async Task RedirectShortUrl_ShouldReturnBadRequest_WhenShortUrlIsEmpty()
        {
            // Arrange
            string shortUrl = string.Empty;

            // Act
            var result = await _controller.RedirectShortUrl(shortUrl) as BadRequestObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(400, result.StatusCode);
        }

        [Test]
        public async Task RedirectShortUrl_ShouldReturnNotFound_WhenShortUrlDoesNotExist()
        {
            // Arrange
            string shortUrl = "http://short.url/abc123";
            _urlShortenerServiceMock.Setup(s => s.GetOriginalUrl(shortUrl)).ReturnsAsync((string)null);

            // Act
            var result = await _controller.RedirectShortUrl(shortUrl) as NotFoundObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(404, result.StatusCode);
        }

        [Test]
        public async Task RedirectShortUrl_ShouldReturnOriginalUrl_WhenShortUrlExists()
        {
            // Arrange
            string shortUrl = "http://short.url/abc123";
            string originalUrl = "http://example.com";
            _urlShortenerServiceMock.Setup(s => s.GetOriginalUrl(shortUrl)).ReturnsAsync(originalUrl);

            // Act
            var result = await _controller.RedirectShortUrl(shortUrl) as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
        }
    }
}
