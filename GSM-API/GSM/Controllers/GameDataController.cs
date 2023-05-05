using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using GSM.SA;
using GSM.Models;


namespace GSM.Controllers
{
    [Route("/GameData")]
    [ApiController]
    public class GameDataController : ControllerBase
    {

        private readonly ILogger<GameDataController> _logger;

        public GameDataController(ILogger<GameDataController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync(string? GameName = null)
        {
            SA.IGDB igdb = SA.IGDB.GetInstance(Environment.GetEnvironmentVariable("TWITCH_CLIENT_ID"), Environment.GetEnvironmentVariable("TWITCH_CLIENT_SECRET"));
            if (GameName == null)
            {
                List<LightGameData> LightGamesList = new List<LightGameData>();
                foreach (string game in AppData.GetAvailableGames())
                {
                    LightGamesList.Add(await igdb.GetGameLightData(game));
                }
                return Ok(LightGamesList);
            }
            if (!AppData.GetAvailableGames().Contains(GameName.ToLower()))
            {
                return NotFound();
            }

            string GptKey = Environment.GetEnvironmentVariable("OPENAI_KEY");
            GameData? gameData = new GameData();
            string ChatGPTDescription = "ERROR";
            try
            {
                gameData = await SQLInterface.GetGameData(GameName);
                if (gameData != null)
                {
                    ChatGPTDescription = gameData.ChatGPTDescription;
                }
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                return NotFound("SQL connection error: " + ex.Message);
            }
            
            if (ChatGPTDescription == null || ChatGPTDescription.StartsWith("ERROR"))
            {
                try
                {
                    ChatGPTDescription = await SA.ChatGPT.GetInstance(GptKey).SendToGPT3_5WithHistory("What is the game " + GameName + "?");
                }
                catch
                {
                    ChatGPTDescription = "ERROR: ChatGPT is unavailable";
                }
            }
            if (gameData == null)
            {
                gameData = await igdb.GetGameFullData(GameName);
            }
            gameData.ChatGPTDescription = ChatGPTDescription;
            SQLInterface.AddGameData(gameData);
            return Ok(gameData);
        }
    }
}