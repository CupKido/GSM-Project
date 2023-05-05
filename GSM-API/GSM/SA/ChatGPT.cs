using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;

namespace GSM.SA
{
    public class ChatGPT
    {
        // make singletone
        private static ChatGPT instance;
        private ChatGPT(string OpenAIKey)
        {
            this.openaiKey = OpenAIKey;
            if (staticOpenaiKey == "")
            {
                staticOpenaiKey = OpenAIKey;
            }
            chat_messages = new List<object>();
            Console.WriteLine("constructor");
        }



        private List<object> chat_messages;
        private int maxMessages = 10;
        private int maxMessages_len = 3500;
        private string openaiKey;
        private static string staticOpenaiKey = "";

        public static ChatGPT GetInstance(string OpenAIKey)
        {
            if (instance == null)
            {
                instance = new ChatGPT(OpenAIKey);
            }
            return instance;
        }
        
        public static void SetStaticOpenAIKey(string OpenAIKey)
        {
            staticOpenaiKey = OpenAIKey;
        }
        public async Task<string> SendToGPT3_5WithHistory(string message)
        {
            var url = "https://api.openai.com/v1/chat/completions";


            var headers = new Dictionary<string, string>()
            {
                { "Authorization", "Bearer " + this.openaiKey}
            };
            this.chat_messages.Add(new
                                {
                                    role = "user",
                                    content = message
                                }
                            );
            if (this.chat_messages.Count > this.maxMessages)
            {
                this.chat_messages.RemoveAt(0);
            }
            while(JsonConvert.SerializeObject(chat_messages).ToString().Length > this.maxMessages_len)
            {
                this.chat_messages.RemoveAt(0);
            }
            var data = new
            {
                model = "gpt-3.5-turbo",
                messages = this.chat_messages
            };

            using (var client = new HttpClient())
            {
                client.Timeout = TimeSpan.FromSeconds(20);
                foreach (var header in headers)
                {
                    client.DefaultRequestHeaders.Add(header.Key, header.Value);
                }

                var json = JsonConvert.SerializeObject(data);
                var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

                var response = await client.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    dynamic responseObject = JsonConvert.DeserializeObject(responseContent);
                    string response_text = responseObject.choices[0].message.content;
                    this.chat_messages.Add(new
                    {
                        role = "assistant",
                        content = response_text
                    }
                            );
                    return response_text;
                }
                else
                {
                    return "Error:\n" + response.Content.ReadAsStringAsync().Result;
                }
            }
        }
        public async static Task<string> SendToGPT3_5(string message)
        {
            var url = "https://api.openai.com/v1/chat/completions";
         

            var headers = new Dictionary<string, string>()
            {
                { "Authorization", "Bearer " + staticOpenaiKey}
            };

            var data = new
            {
                model = "gpt-3.5-turbo",
                messages = new List<object>
                {
                    new
                    {
                        role = "user",
                        content = message
                    }
                }
            };

            using (var client = new HttpClient())
            {
                foreach (var header in headers)
                {
                    client.DefaultRequestHeaders.Add(header.Key, header.Value);
                }

                var json = JsonConvert.SerializeObject(data);
                var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

                var response = await client.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    dynamic responseObject = JsonConvert.DeserializeObject(responseContent);
                    return responseObject.choices[0].message.content;
                }
                else
                {
                    return "Error:\n" + response.Content.ReadAsStringAsync().Result;
                }
            }
        }
    }
}
