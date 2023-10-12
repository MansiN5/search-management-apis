using System.IO.Abstractions;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using SearchManagement.Interface;

namespace SearchManagement.Datasource.Tests
{
    public class UserRepositoryTest
    {

        private readonly UserRepository sut;
        private readonly Mock<ILogger<UserRepository>> loggerMock;

        public UserRepositoryTest()
        {
            loggerMock = new Mock<ILogger<UserRepository>>();

            sut = new UserRepository(
                loggerMock.Object
                );
        }

        [Fact]
        public async void GetUsersAsync_InvalidFilePath_ShouldFail()
        {
            // Arrange
            var filePath = "a/b.txt";

            // Create a mock of the IFileSystem interface using Moq
            var fileSystemMock = new Mock<IFileSystem>();
            fileSystemMock.Setup(fs => fs.File.Exists(filePath)).Returns(false);

            // Act and Assert
            await Assert.ThrowsAsync<Exception>(async () => await sut.GetUsersAsync());
        }

        [Fact]
        public async void GetUsersAsync_ValidFile_ShouldPass()
        {
            // Arrange
            string folderPath = "TestFiles"; // Specify the folder where your JSON file is located
            string jsonFileName = "TestFile1.json"; // Specify the name of your JSON file

            // Build the absolute path to your data file
            var jsonFilePath = Path.Combine(folderPath, jsonFileName);


            string json = await File.ReadAllTextAsync(jsonFilePath);
            List<User> people = JsonConvert.DeserializeObject<List<User>>(json) ?? new List<User>();

            // Act and Assert
            people.Should().HaveCount(2);
        }


        [Fact]
        public async void GetUsersAsync_EmptyFile_ShouldPass()
        {
            // Arrange
            string folderPath = "TestFiles"; // Specify the folder where your JSON file is located
            string jsonFileName = "TestFile2.json"; // Specify the name of your JSON file

            // Build the absolute path to your data file
            var jsonFilePath = Path.Combine(folderPath, jsonFileName);
            string json = await File.ReadAllTextAsync(jsonFilePath);
            List<User> people = JsonConvert.DeserializeObject<List<User>>(json) ?? new List<User>();

            // Act and Assert
            people.Should().HaveCount(0);
        }

    }
}