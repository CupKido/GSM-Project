using GSM.Models;
using GSM.SA;

namespace GSM
{
    public class Emulator
    {
        static string EmulatorUrl = "http://localhost:5000";
        async public static Task<List<GameServerStats>> GetGameServerStats(string GameName, DateTime? start = null, DateTime? finish = null)
        {
            List<GameServerStats> gameServerStatsList = new List<GameServerStats>();

            if (finish == null && start == null)
            {
                // api call to emulator
                gameServerStatsList.Add(await SA.EmulatorSA.GetGameServerStats(GameName));
                return gameServerStatsList;
            }
            if (finish != null)
            {
                // Make sure finish is not after now
                if (finish > DateTime.Now)
                {
                    finish = DateTime.Now;
                }
                start = DateTime.MinValue;
            }
            if (start != null)
            {
                finish = DateTime.Now;
            }
            // add all data between dates from sql

            return gameServerStatsList;
        }
    }
}
