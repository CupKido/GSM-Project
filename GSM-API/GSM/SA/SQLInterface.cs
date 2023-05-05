using GSM.Models;
using GSM.Data;
using Microsoft.EntityFrameworkCore;


namespace GSM.SA
{
    public static class SQLInterface
    {
        static GSMContext context = new GSMContext();

        async public static void AddServerStat(GameServerStats serverStat)
        {
            GSMContext thisContext = new GSMContext();
            while (thisContext.Database.CurrentTransaction != null)
            {
                await Task.Delay(100); // Wait for 100 milliseconds before checking again
            }
            thisContext.GameServerStats.Add(serverStat);
            try
            {
                await thisContext.SaveChangesAsync();
                Console.WriteLine("Saved " + serverStat.GameName + " to database");
            }catch(Exception ex)
            {

            }
        }

        async public static void AddGameData(GameData gameData)
        {
            GSMContext thisContext = new GSMContext();
            GameData last = await GetGameData(gameData.Name);
            if (last != null)
            {
                thisContext.GamesData.Remove(last);
            }
            thisContext.GamesData.Add(gameData);
            while (thisContext.Database.CurrentTransaction != null)
            {
                await Task.Delay(100); // Wait for 100 milliseconds before checking again
            }
            
            try
            {
                await thisContext.SaveChangesAsync();
                Console.WriteLine("Saved " + gameData.Name + "\'s Data to database");
            }
            catch (Exception ex)
            {

            }
        }

        async public static Task<GameData?> GetGameData(string GameName)
        {
            GSMContext thisContext = new GSMContext();
            thisContext.Database.SetCommandTimeout(20);
            while (thisContext.Database.CurrentTransaction != null)
            {
                await Task.Delay(100); // Wait for 100 milliseconds before checking again
            }
            return await thisContext.GamesData.FirstOrDefaultAsync(x => x.Name == GameName);
        }
        
        async public static Task<List<GameServerStats>> GetGameServerStats(string GameName, DateTime? start = null, DateTime? finish = null)
        {
            GSMContext thisContext = new GSMContext();
            List<GameServerStats> gameServerStatsList = new List<GameServerStats>();
            while (thisContext.Database.CurrentTransaction != null)
            {
                await Task.Delay(100); // Wait for 100 milliseconds before checking again
            }
            if (finish == null && start == null)
            {
                gameServerStatsList.Add(thisContext.GameServerStats.Where(x => x.GameName == GameName).OrderByDescending(x => x.UpdateDate).FirstOrDefault());
                return gameServerStatsList;
            }
            if (finish == null)
            {
                finish = DateTime.Now;
            }
            if(start == null)
            {
                start = DateTime.MinValue;
            }
            gameServerStatsList = thisContext.GameServerStats.Where(x => x.GameName == GameName && x.UpdateDate >= start && x.UpdateDate <= finish).ToList();
            return gameServerStatsList;
        }
    }
}
