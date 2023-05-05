using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GSM.Models
{
    public class GameData
    {
        [Key, Column(Order = 0)]
        public string Name { get; set; }
        
        
        public string? Description { get; set; }

        public string? ChatGPTDescription { get; set; }

        public string? ImageUrl { get; set; }
        
        
        [Column(TypeName = "jsonb")]
        public List<GameGenre> Genres { get; set; }
    }
}
