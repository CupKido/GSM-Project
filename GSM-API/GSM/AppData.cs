namespace GSM
{
    static public class AppData
    {
        static private string[] AvailableGames = { 
            "valorant", "dead by daylight", "league of legends", "among us", "fortnite","cod - modern warfare",
            "overwatch 2", "apex legends", "genshin impact", "grand theft auto 5",  
            "muck", "crab game", "counter-strike: global offensive", "rust", "vrchat", "warframe", "pubg: battlegrounds" 
        };
        
        
        static public string[] GetAvailableGames()
        {
            return AvailableGames;
        }
    }
}
