using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using SearchManagement.Datasource;
using SearchManagement.Interface;

namespace SearchManagement.Services.Tests
{
    public class SearchServiceTest
    {
        private readonly Mock<IUserRepository> userRepositoryMock;
        private readonly SearchService sut;
        private readonly Mock<ILogger<SearchService>> loggerMock;
        List<User> testusers = new List<User> { new User() { Id = 1, FirstName = "James", LastName = "Kubu", Email = "James1Kubu@test.com", Gender = "Male" },
                                            new User() { Id = 2, FirstName = "James", LastName = "Pfieffer", Email = "James1Pfieffer@test.com", Gender = "Male" },
                                            new User() { Id = 3, FirstName = "Chalmers", LastName = "Longfut", Email = "Chalmers1Longfutjam@test.com", Gender = "Female" },
                                            new User() { Id = 3, FirstName = "Katey", LastName = "Longfut", Email = "Katey@test.com", Gender = "Female" },
                                            new User() { Id = 4, FirstName = "Katey", LastName = "Soltan", Email = "Katey1Soltan@test.com", Gender = "Female" }};

        public SearchServiceTest()
        {
            loggerMock = new Mock<ILogger<SearchService>>();

            userRepositoryMock = new Mock<IUserRepository>();

            sut = new SearchService(
                loggerMock.Object,
                userRepositoryMock.Object
                );
        }

        [Fact]
        public async Task Search_Test1()
        {
            //Arrange
            userRepositoryMock.Setup(x
                    => x.GetUsersAsync())
                .ReturnsAsync(testusers);

            // Act
            var response = await sut.Search("James");

            // Asserts
            response.ToList().Should().HaveCount(2);

        }

        [Fact]
        public async Task Search_Test2()
        {
            //Arrange
            userRepositoryMock.Setup(x
                    => x.GetUsersAsync())
                .ReturnsAsync(testusers);

            // Act
            var response =  await sut.Search("jam");

            // Asserts
            response.ToList().Should().HaveCount(3);

        }

        [Fact]
        public async Task Search_Test3()
        {
            //Arrange
            userRepositoryMock.Setup(x
                    => x.GetUsersAsync())
                .ReturnsAsync(testusers);

            // Act
            var response = await sut.Search("Katey Soltan");

            // Asserts
            response.ToList().Should().HaveCount(1);

        }

        [Fact]
        public async Task Search_Test4()
        {
            //Arrange
            userRepositoryMock.Setup(x
                    => x.GetUsersAsync())
                .ReturnsAsync(testusers);

            // Act
            var response = await sut.Search("Jasmine Duncan");

            // Asserts
            response.ToList().Should().HaveCount(0);

        }

        [Fact]
        public async Task Search_ShoulFail()
        {
            //Arrange
            string exceptionMessage = "JSON file 'Users.json' not found in the 'Data' folder.";
            userRepositoryMock.Setup(x
                    => x.GetUsersAsync())
                .Throws(new Exception(exceptionMessage));


            // Asserts
            await Assert.ThrowsAsync<Exception>(async () => await sut.Search("Jasmine Duncan"));

        }


    }
}