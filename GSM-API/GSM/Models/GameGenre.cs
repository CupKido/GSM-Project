using Org.BouncyCastle.Asn1.X509;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GSM.Models
{
    public class GameGenre
    {
        [Key]
        public string Genre { get; set; }

        [Key, ForeignKey(nameof(GameData))]
        public string GameName { get; set; }
        

    }
}
