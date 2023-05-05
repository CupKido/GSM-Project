using GSM.Models;
using GSM.ViewModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GSM.Views
{
    /// <summary>
    /// Interaction logic for ServersButtons.xaml
    /// </summary>
    public partial class ServersButtons : UserControl
    {
        public readonly  ServerMonitorVM ServerMonitorVM;
        string Details { get; set; }
        string Temperature { get; set; }
        float temperature { get; set; }

        public ServersButtons ()
        {
            InitializeComponent ();


            ServerMonitorVM = new ServerMonitorVM ();

            cpuMonitor.DataContext = ServerMonitorVM.cpuMonitor;
            memoryMonitor.DataContext = ServerMonitorVM.memoryMonitor;
        }

        async private void getStatus ()
        {
            using ( var httpClient = new HttpClient () )
            {
                try
                {
                    var gamesResponse = await httpClient.GetAsync("http://saartaler.site/GameData");
                    gamesResponse.EnsureSuccessStatusCode ();
                    var gamesResponseBody = await gamesResponse.Content.ReadAsStringAsync();


                    List<string> games = new List<string> ();

                    JArray gamesJsonArray = JArray.Parse(gamesResponseBody);

                    foreach ( JObject jsonObj in gamesJsonArray )
                    {
                        games.Add ((string) jsonObj["name"]);
                    }

                    Random random = new Random();
                    int index = random.Next(games.Count);
                    string chosenGame = games[index];

                    var response = await httpClient.GetAsync("http://saartaler.site/GameStats?GameName=" + chosenGame);
                    response.EnsureSuccessStatusCode ();
                    var responseBody = await response.Content.ReadAsStringAsync();

                    JArray jsonArray = JArray.Parse(responseBody);

                    int memoryUsage = (int)(100* (float)jsonArray[0]["ramUsage"]);
                    int cpuUsage = (int)(100* (float)jsonArray[0]["cpuUsage"]);
                    int maxcpu = (int)jsonArray[0]["maxcpu"];
                    int ramsize = (int)jsonArray[0]["ramSize"];
                    int numPlayers = (int)jsonArray[0]["playersCount"];
                    string gameName = (string)jsonArray[0]["gameName"];
                    this.temperature = (float)jsonArray[0]["temperature"];
                    this.Details = "There are currently " + numPlayers + " players that play " + gameName;
                    this.Temperature = "The CPU temperature is " + temperature + "°C";


                    TemperatureText.Text = this.Temperature;
                    DetailsText.Text = this.Details;
                    TemperatureSlider.Value = this.temperature;

                    var memoryContext = memoryMonitor.DataContext as GaugeVM;
                    if ( memoryContext  != null )
                    {
                        memoryContext.Value = (double) memoryUsage / ramsize;
                    }


                    var cpuContext = cpuMonitor.DataContext as GaugeVM;
                    if ( cpuContext != null )
                    {
                        cpuContext.Value = (double) cpuUsage / maxcpu;
                    }
                }
                catch ( HttpRequestException error )
                {
                    Console.WriteLine ($"Error: {error.Message}");
                }
            }
        }

        private void UpdateServer (object sender, RoutedEventArgs e)
        {
            this.getStatus ();
        }
    }
}
