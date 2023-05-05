using GSM;
using GSM.Models;
using GSM.SA;

namespace GSM.Services
{
    public class DatabaseUpdater : IHostedService, IDisposable
    {
        private Timer _timer;
        private int waitTime = 25;
        private int GameIndex = 0;
        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(UpdateDatabase, null, TimeSpan.Zero, TimeSpan.FromSeconds(waitTime));
            return Task.CompletedTask;
        }

        async private void UpdateDatabase(object state)
        {
            try
            {
                string ThisGame = GSM.AppData.GetAvailableGames()[GameIndex];
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine(waitTime.ToString() + " seconds passed, Updating " + ThisGame);
                Console.ResetColor();
                GameIndex++;
                if (GameIndex >= GSM.AppData.GetAvailableGames().Count())
                {
                    GameIndex = 0;
                }
            
                List<GameServerStats> res = await Emulator.GetGameServerStats(ThisGame);
                foreach (GameServerStats gss in res)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("recieved answer for: " + gss.GameName + " | Player Count: " + gss.PlayersCount);
                    SQLInterface.AddServerStat(gss);
                    Console.ResetColor();
                }
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(e.Message);
                Console.ResetColor();
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Dispose();
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
