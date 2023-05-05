using GSM.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
namespace GSM
{
    public class Setup
    {
        static public void ConfigureServices(IServiceCollection Services)
        {

            string host = Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true"
            ? "host.docker.internal" : "172.17.0.1";
            host = "172.17.0.1";
            string connection_string = "server=" + host +
                ";port=" + "3306" +
                ";database=" + "gsmdb" +
                ";user=" + "root" +
                ";password=" + "root";
            Services.AddDbContext<GSMContext>(options => options.UseMySQL(connection_string));
        }
        static public void ConfigureServices2(IServiceProvider ServiceProvider)
        {
            var dbContext = ServiceProvider.GetRequiredService<GSMContext>();
            dbContext.Database.Migrate();
        }
    }
}
