using GSM.Models;
using GSM.ViewModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text.Json;
using System.Windows.Controls;
using System.Xml.Linq;

namespace GSM.Views
{
    /// <summary>
    /// Interaction logic for GameListView.xaml
    /// </summary>
    public partial class GameListView : UserControl
    {
        public readonly GameDescriptionVM gameDescriptionVM = new GameDescriptionVM();


        public GameListView ()
        {
            InitializeComponent ();

            DataContext = gameDescriptionVM;

            Games games = Games.Instance;
            gameList.ItemsSource = games.GetGames ();
        }

        async private void getDescription (object sender, System.Windows.RoutedEventArgs e)
        {
            string name = (sender as Button).Tag as string;

            using ( var httpClient = new HttpClient () )
            {
                try
                {
                    var response = await httpClient.GetAsync("http://saartaler.site/GameData?GameName=" + name);
                    response.EnsureSuccessStatusCode ();
                    var responseBody = await response.Content.ReadAsStringAsync();

                    Console.Write (responseBody);

                    dynamic jsonObject = JsonConvert.DeserializeObject(responseBody);


                    string description = (string)jsonObject.description;
                    string GPTdescription = (string)jsonObject.chatGPTDescription;

                    gameDescriptionVM.Description = description;
                    gameDescriptionVM.GPTDescription = GPTdescription;
                }
                catch ( HttpRequestException error )
                {
                    Console.WriteLine ($"Error: {error.Message}");
                }
            }
        }
    }
}
