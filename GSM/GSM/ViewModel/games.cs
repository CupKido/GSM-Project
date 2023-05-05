using GSM.Models;
using GSM.ViewModels;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace GSM.ViewModel
{
    public class Games : ViewModelBase
    {
        private static Games instance = null;
        private static readonly object lockObject = new object();

        private Games ()
        {
            fetchGames ().Wait();
        }

        async private Task fetchGames ()
        {
            using ( var httpClient = new HttpClient () )
            {
                try
                {
                    var response = httpClient.GetAsync("http://saartaler.site/GameData").Result;
                    response.EnsureSuccessStatusCode ();
                    var responseBody = await response.Content.ReadAsStringAsync();

                    JArray jsonArray = JArray.Parse(responseBody);

                    foreach ( JObject jsonObj in jsonArray )
                    {
                        string name = (string)jsonObj["name"];
                        string imageUrl = (string)jsonObj["imageUrl"];

                        games.Add (new GameModel (name, imageUrl));
                    }
                    
                }
                catch ( HttpRequestException e )
                {
                    Console.WriteLine ($"Error: {e.Message}");
                }
            }
        }

        public static Games Instance
        {
            get
            {
                // Lazy initialization
                if ( instance == null )
                {
                    lock ( lockObject )
                    {
                        if ( instance == null )
                        {
                            instance = new Games ();
                        }
                    }
                }
                return instance;
            }
        }

        private List<GameModel> games = new List<GameModel> ();

       

        public void Add(GameModel game)
        {
            games.Add (game);
        }

        public List<string> GetNames ()
        {
            return games.Select (x => x.GameName).ToList ();
        }

        public List<GameModel> GetGames ()
        {
            return games;
        }


    }
}
