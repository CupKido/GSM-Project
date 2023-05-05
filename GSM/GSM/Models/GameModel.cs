using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSM.Models
{
    public class GameModel
    {
        public string GameName { get; set; }
        public string Description { get; set; }
        public string DescriptionByChatGPT { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string ThumbnailUrl { get; set; }
        public List<string> Genres { get; set; }

        public GameModel (string gameName, string thumbnailUrl)
        {
            ThumbnailUrl = thumbnailUrl;
            GameName = gameName;
        }
    }
}
