using Microsoft.AspNetCore.Mvc;
using SearchManagement.Interface;
using SearchManagement.Services;

namespace SearchManagement.API.Controllers
{
    [Route("api")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private readonly ILogger<SearchController> _logger;
        private readonly ISearchService _searchService;


        public SearchController(ILogger<SearchController> logger, ISearchService searchService)
        {
            _logger = logger;
            _searchService = searchService;
        }

        [HttpGet]
        [Route("search")]
        [ProducesResponseType(typeof(List<User>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get(string filter)
        {
            try
            {
                var response = await _searchService.Search(filter);
                if (!response.Any())
                {
                    return NotFound($"No Records Found");
                }
                return Ok(response);
            }
            catch (Exception e)
            {
                _logger.LogError($"Error Method : search, Exception:{e}, Timestamp : {DateTime.UtcNow}");

                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
