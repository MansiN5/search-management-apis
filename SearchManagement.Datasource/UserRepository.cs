using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SearchManagement.Interface;

namespace SearchManagement.Datasource
{
    public interface IUserRepository
    {
        Task<List<User>> GetUsersAsync();
    }
    public class UserRepository : IUserRepository
    {
        private readonly ILogger<UserRepository> _logger;
        public UserRepository(ILogger<UserRepository> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Get Users Async
        /// </summary>
        /// <returns></returns>
        public async Task<List<User>> GetUsersAsync()
        {
            try
            {
                string folderPath = "Data"; // Specify the folder where your JSON file is located
                string jsonFileName = "Users.json"; // Specify the name of your JSON file

                string jsonFilePath = Path.Combine(folderPath, jsonFileName);

                // Check if the JSON file exists
                if (File.Exists(jsonFilePath))
                {
                    string json = await File.ReadAllTextAsync(jsonFilePath);
                    List<User> people = JsonConvert.DeserializeObject<List<User>>(json) ?? new List<User>();
                    return people;
                }
                else
                {
                    _logger.LogError($"Error Method : GetUsersAsync, Exception:JSON file '{jsonFileName}' not found in the '{folderPath}' folder., Timestamp : {DateTime.UtcNow}");
                    throw new Exception($"JSON file '{jsonFileName}' not found in the '{folderPath}' folder.");
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Error Method : GetUsersAsync, Exception:{e}, Timestamp : {DateTime.UtcNow}");
                throw;
            }
        }
    }
}