using Microsoft.Extensions.Logging;
using SearchManagement.Datasource;
using SearchManagement.Interface;

namespace SearchManagement.Services
{
    public interface ISearchService
    {
        Task<List<User>> Search(string filter);
    }
    public class SearchService : ISearchService
    {
        private readonly ILogger<SearchService> _logger;
        private readonly IUserRepository _userRepository;
        public SearchService(ILogger<SearchService> logger, IUserRepository userRepository)
        {
            _logger = logger;
            _userRepository = userRepository;
        }
        
        /// <summary>
        /// Search
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public async Task<List<User>> Search(string filter)
        {
            try
            {
                var users = await _userRepository.GetUsersAsync();

                return users.FindAll(person =>
                person.FirstName.Contains(filter, StringComparison.OrdinalIgnoreCase) ||
                person.LastName.Contains(filter, StringComparison.OrdinalIgnoreCase) ||
                person.FullName.Contains(filter, StringComparison.OrdinalIgnoreCase) ||
                person.Email.Contains(filter, StringComparison.OrdinalIgnoreCase));
            }
            catch (Exception e)
            {
                _logger.LogError($"Error Method : Search, Exception:{e}, Timestamp : {DateTime.UtcNow}");
                throw;
            }

        }
    }
}
