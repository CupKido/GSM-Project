using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GSM.Models
{
    public class GameServerStats
    {
        [Key, Column(Order = 0)]
        public string GameName { get; set; }

        

        [Column(Order = 1, TypeName = "datetime")]
        public DateTime UpdateDate { get; set; }
        
        public long PlayersCount { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public float CPUUsage { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public float RAMUsage { get; set; }

        public float MAXCPU { get; set; }

        public float RAMSize { get; set; }
        
        public int TopScore { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public float Temperature { get; set; }
    }
}
