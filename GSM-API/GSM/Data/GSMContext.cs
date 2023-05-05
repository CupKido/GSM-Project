using GSM.Models;
using Microsoft.EntityFrameworkCore;
using MySql.EntityFrameworkCore;


namespace GSM.Data
{
    public class GSMContext :DbContext
    {
        public GSMContext() : base()
        {
        }
        public GSMContext(DbContextOptions<GSMContext> options) : base(options)
        {
        }
        public DbSet<GameServerStats> GameServerStats { get; set; } = null!;
        public DbSet<GameData> GamesData { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        
            string host = "gsmsb.cx3zk7tyqo76.us-east-1.rds.amazonaws.com";


            // optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=GSM;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
            optionsBuilder.UseMySQL("server=" + host +
                ";port=" + "3306" + 
                ";database=" + "gsmdb" +
                ";user=" + "root" +
                ";password=" + "rootroot");
            // optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=master;TrustServerCertificate=True;Trusted_Connection=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GameServerStats>()
                  .HasKey(m => new { m.GameName, m.UpdateDate });
            modelBuilder.Entity<GameGenre>()
                .HasKey(gg => new { gg.Genre, gg.GameName });
        }

    }
}
