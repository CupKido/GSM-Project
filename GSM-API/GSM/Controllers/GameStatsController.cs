using GSM.Models;
using GSM.SA;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GSM.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class GameStatsController : ControllerBase
    {
        private readonly ILogger<GameStatsController> _logger;

        public GameStatsController(ILogger<GameStatsController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync(string GameName, DateTime? start = null, DateTime? finish = null)
        {
            if (!AppData.GetAvailableGames().Contains(GameName.ToLower()))
            {
                return NotFound();
            }
            
            return Ok(await SQLInterface.GetGameServerStats(GameName, start, finish));
        }
    }
}
