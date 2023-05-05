using GSM.Models;
using Newtonsoft.Json;
using System;
using System.Net;

namespace GSM.SA
{
    public class EmulatorSA
    {
        public async static Task<GameServerStats> GetGameServerStats(string gameName)
        {
            var json = JsonConvert.SerializeObject("{}");
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            using (var client = new HttpClient())
            {
                Console.WriteLine(Environment.GetEnvironmentVariable("EMULATOR_HOST"));
                string host = Environment.GetEnvironmentVariable("IS_PRODUCTION") == "true" ? "saartaler.site" : "host.docker.internal";
                host = "saartaler.site";
                string url = "http://" + host + ":" + "5000" + "/GetData/" + gameName;
                Console.WriteLine("asking " + url);
                var response = await client.GetAsync(url);
                GameServerStats emulatorData;
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    dynamic responseObject = JsonConvert.DeserializeObject(responseContent);
                    emulatorData = new GameServerStats {
                        PlayersCount = responseObject.PlayersCount,
                        MAXCPU = responseObject.MaxCPU,
                        CPUUsage = responseObject.CPUUsage,
                        RAMUsage = responseObject.RAMUsage,
                        RAMSize = responseObject.RAMSize,
                        GameName = responseObject.GameName,
                        TopScore = responseObject.TopScore,
                        Temperature = responseObject.Temperature,
                        UpdateDate = DateTime.Now
                    };
                }
                else
                {
                    throw new Exception("Error! Emulator interaction unsuccessful");

                }
                return emulatorData;
            }
        }
    }
}
