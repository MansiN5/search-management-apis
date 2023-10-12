using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Moq;
using SearchManagement.API.Controllers;
using SearchManagement.Interface;
using SearchManagement.Services;

namespace SearchManagement.API.UnitTest.Controller
{
    public class SearchControllerTests
    {
        private readonly Mock<ISearchService> searchServiceMock;
        private readonly SearchController sut;
        private readonly Mock<ILogger<SearchController>> loggerMock;
        List<User> Testusers = new List<User> { new User() { Id = 1, FirstName = "James", LastName = "Kubu", Email = "James1Kubu@test.com", Gender = "Male" },
                                            new User() { Id = 2, FirstName = "James", LastName = "Pfieffer", Email = "James1Pfieffer@test.com", Gender = "Male" },
                                            new User() { Id = 3, FirstName = "Chalmers", LastName = "Longfut", Email = "Chalmers1Longfutjam@test.com", Gender = "Female" },
                                            new User() { Id = 3, FirstName = "Katey", LastName = "Longfut", Email = "Katey@test.com", Gender = "Female" },
                                            new User() { Id = 4, FirstName = "Katey", LastName = "Soltan", Email = "Katey1Soltan@test.com", Gender = "Female" }};

        public SearchControllerTests()
        {
            loggerMock = new Mock<ILogger<SearchController>>();

            searchServiceMock = new Mock<ISearchService>();

            sut = new SearchController(
                loggerMock.Object,
                searchServiceMock.Object
                );
        }

        [Fact]
        public async Task Get_ReturnsOkOnExpectedResults()
        {
            // Arrange
            List<User> users = new List<User> { new User() { Id = 1, FirstName = "James", LastName = "Kubu", Email = "James1Kubu@test.com", Gender = "Male" } };
            searchServiceMock.Setup(x
                => x.Search(
                    It.IsAny<string>()
                )).ReturnsAsync(users);

            // Act
            var response = await sut.Get("jam");

            // Assert
            Assert.NotNull(response);
            Assert.IsType<OkObjectResult>(response);
        }

        [Fact]
        public async Task Get_ReturnsNotFoundResults()
        {
            // Arrange
            searchServiceMock.Setup(x
                => x.Search(
                    It.IsAny<string>()
                )).ReturnsAsync(new List<User>());

            // Act
            var response = await sut.Get("jam");

            // Assert
            Assert.NotNull(response);
            Assert.IsType<NotFoundObjectResult>(response);
        }

        [Fact]
        public async Task Get_ReturnsInternalServerErrorResults()
        {
            // Arrange
            List<User> users = new List<User>();
            searchServiceMock.Setup(x
                => x.Search(
                    It.IsAny<string>()
                )).Throws(new Exception(""));

            // Act
            var response = await sut.Get("jam");

            // Assert
            var statusCodeResponse = response as StatusCodeResult;
            Assert.NotNull(statusCodeResponse);
            var statusCodeResult = (IStatusCodeActionResult)statusCodeResponse;

            Assert.Equal(500, statusCodeResult.StatusCode);
        }

    }
}
