using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AureliaTemplate
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var env = Debugger.IsAttached ? "Development" : "Production";

            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env}.json", optional: true, reloadOnChange: true)
                .Build();

            Console.WriteLine($"Using {env} Setting");

            var host = CreateWebHostBuilder(args, config).Build();

            host.Run();

        }

        public static IHostBuilder CreateWebHostBuilder(string[] args, IConfigurationRoot config) =>
             Host.CreateDefaultBuilder(args)
             .ConfigureWebHostDefaults(webBuilder =>
             {
                 webBuilder.UseStartup<Startup>();

                 webBuilder.UseKestrel(options =>
                 {
                     //PORT was randomly assigned by heroku in order to bind to expose port (80)
                     var port = int.Parse(Environment.GetEnvironmentVariable("PORT") ?? $"{config["Port"]}");
                     Console.WriteLine($"Port: {port}");
                     options.ListenAnyIP(port);
                 });
             });

    }
}
