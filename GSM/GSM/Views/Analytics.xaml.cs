using GSM.Models;
using GSM.ViewModel;
using LiveCharts.Definitions.Charts;
using LiveCharts.Wpf;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Security.Cryptography;
using System.Windows;
using System.Windows.Controls;

namespace GSM.Views
{
    /// <summary>
    /// Interaction logic for Analytics.xaml
    /// </summary>
    public partial class Analytics : UserControl
    {
        pieChartVM pieChartVM = new pieChartVM();
        public Analytics ()
        {
            InitializeComponent ();

            Games games = Games.Instance;
            chooseGame.ItemsSource = games.GetNames ();
            pieChart.Visibility = Visibility.Hidden;
            pieChart.DataContext = pieChartVM;
        }

        async private void GetAnalytics (object sender, RoutedEventArgs e)
        {
            string gameName =chooseGame.SelectedValue.ToString();
            string[] start = StartDate.Text.Split('/');
            DateTime startDate = new DateTime(Int32.Parse(start[2]), Int32.Parse(start[1]), Int32.Parse(start[0]));

            string[] end = StartDate.Text.Split('/');
            DateTime endDate = new DateTime(Int32.Parse(end[2]), Int32.Parse(end[1]), Int32.Parse(end[0]));
           
            using ( var httpClient = new HttpClient () )
            {
                try
                {
                    string url = "http://saartaler.site/GameStats?GameName=" + gameName + "&start=" + startDate + "&finish=" + endDate;
                    var response = await httpClient.GetAsync(url);
                    response.EnsureSuccessStatusCode ();
                    var responseBody = await response.Content.ReadAsStringAsync();

                    pieChartVM.lowLoad = 0;
                    pieChartVM.mediumLoad = 0;
                    pieChartVM.highLoad = 0;
                    JArray jsonArray = JArray.Parse(responseBody);

                    foreach ( JObject jsonObj in jsonArray )
                    {
                        int memoryUsage = (int)(100* (float)jsonObj["ramUsage"]);
                        int cpuUsage = (int)(100* (float)jsonObj["cpuUsage"]);
                        int maxcpu = (int)jsonObj["maxcpu"];
                        int ramsize = (int)jsonObj["ramSize"];

                        float cpu =  (float)100*cpuUsage/maxcpu;
                        float memory = (float) 100*memoryUsage/ramsize;
                        if (cpu<20 && memory<20)
                        {
                            pieChartVM.load["low"]+= 1;
                        }
                        else if ( cpu > 80 && memory > 80 )
                        {
                            pieChartVM.load["high"] += 1;
                        }
                        else
                        {
                            pieChartVM.load["medium"] += 1;
                        } 
                    }


                    //lowLoadChart.Values = new LiveCharts.IChartValues(lowLoad);
                    //mediumLoadChart.Values = mediumLoad;
                    //highLoadChart.Values = highLoad;
                    pieChart.Visibility = Visibility.Visible;
                    

                }
                catch ( HttpRequestException error )
                {
                    Console.WriteLine ($"Error: {error.Message}");
                }
            }
        }
    }
}
