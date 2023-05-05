using IGDB;
using IGDB.Models;
using GSM.Models;
using MySqlX.XDevAPI;

namespace GSM.SA
{
    public class IGDB
    {
        // Singleton
        private static IGDB instance;

        private IGDBClient igdb;
        
        private IGDB(string TwitchClientID, string TwitchAPISecret)
        {
            this.igdb = new IGDBClient(
              // Found in Twitch Developer portal for your app
              TwitchClientID,
              TwitchAPISecret
            );
        }

        public static IGDB GetInstance(string TwitchClientID = "", string TwitchAPISecret = "")
        {
            if (instance == null)
            {
                if (TwitchClientID == "" || TwitchAPISecret == "")
                {
                    throw new Exception("TwitchClientID and TwitchAPISecret must be set");
                }
                instance = new IGDB(TwitchClientID, TwitchAPISecret);
            }
            return instance;
        }
        
        async public Task<GameData> GetGameFullData(string GameName)
        {
            var games = await igdb.QueryAsync<Game>(IGDBClient.Endpoints.Games, query: "fields id, name, cover.*, genres.*, screenshots.*, summary; search \"" + GameName + "\"; limit 10;");
            if (games == null || games.Length == 0)
            {
                throw new Exception("Game not found");
            }
            Game? g = null;
            foreach(Game game in games)
            {
                if(game.Name.ToLower() == GameName.ToLower())
                {
                    g = game;
                    break;
                }
            }
            if (g == null)
            {
                g = games[0];
            }
            
            string gameName = g.Name;
            List<GameGenre> genres = new List<GameGenre>();
            if (g.Genres != null && g.Genres.Values != null)
            {
                foreach (Genre genre in g.Genres.Values)
                {
                    genres.Add(new GameGenre() { GameName = gameName, Genre = genre.Name });
                }
            }
            string coverUrl = "";
            if (g.Cover != null && g.Cover.Value != null)
            {
                coverUrl = "https:" + g.Cover.Value.Url;
            }
            
            //ChatGPT my_chat = ChatGPT.GetInstance(Environment.GetEnvironmentVariable("OPENAI_KEY"));
            //string ChatGPTdesc = await my_chat.SendToGPT3_5WithHistory("What is the game " + gameName + "?");
            
            
            // replace "thumb" with "1080p" to get full size image
            coverUrl = coverUrl.Replace("thumb", "1080p");
            
            GameData gameData = new GameData
            {
                Name = gameName,
                Description = g.Summary != null ? g.Summary : "",
                Genres = genres,
                ImageUrl = coverUrl
            };
            


            //var game = games.Select(game => /*"Name: " "\nCategory: " + game.Category*/game.Name + " " + game.Genres.Values[0].Name + " " + game.Websites.Values[0].Url + " " + game.Cover.Value.Url).ToArray()[0];
            return gameData;
        }
    
        async public Task<LightGameData> GetGameLightData(string GameName)
        {
            var games = await igdb.QueryAsync<Game>(IGDBClient.Endpoints.Games, query: "fields name, cover.*; search \"" + GameName + "\"; limit 10;");
            if (games == null || games.Length == 0)
            {
                throw new Exception("Game not found");
            }
            Game? g = null;
            foreach (Game game in games)
            {
                if (game.Name.ToLower() == GameName.ToLower())
                {
                    g = game;
                    break;
                }
            }
            if (g == null)
            {
                g = games[0];
            }

            string coverUrl = "";
            if (g.Cover != null && g.Cover.Value != null)
            {
                coverUrl = "https:" + g.Cover.Value.Url;
            }
            coverUrl = coverUrl.Replace("thumb", "1080p");

            LightGameData gameData = new LightGameData
            {
                Name = g.Name,
                ImageUrl = coverUrl
            };

            return gameData;
        }
    
    }
}
