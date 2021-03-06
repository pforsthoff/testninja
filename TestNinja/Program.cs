using System.Net;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Steeltoe.Extensions.Configuration.CloudFoundry;

namespace TestNinja
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureAppConfiguration(c =>
                    {
                        c.AddJsonFile("config/appsettings.json", optional: true, reloadOnChange: true);
                    });
                    webBuilder.UseStartup<Startup>();
                    var client = new WebClient();
                });
    }
}
