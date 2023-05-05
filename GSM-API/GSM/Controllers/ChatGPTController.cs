using GSM.Models;
using GSM.SA;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GSM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatGPTController : ControllerBase
    {

        private readonly ILogger<ChatGPTController> _logger;

        public ChatGPTController(ILogger<ChatGPTController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> GetAsync([FromBody] string Query)
        {
            string ChatGPTAnswer;
            string GptKey = Environment.GetEnvironmentVariable("OPENAI_KEY");
            try
            {
                ChatGPTAnswer = await SA.ChatGPT.GetInstance(GptKey).SendToGPT3_5WithHistory(Query);
            }
            catch
            {
                return NotFound("ERROR: ChatGPT is unavailable");
            }
            return Ok("{\"Answer\":\"" + ChatGPTAnswer + "\"}");
        }
    }
}
