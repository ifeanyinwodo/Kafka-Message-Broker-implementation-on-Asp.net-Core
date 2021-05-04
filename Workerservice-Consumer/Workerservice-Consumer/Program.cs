using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Workerservice_Consumer.Models;

namespace Workerservice_Consumer
{
    public class Program
    {
        public static void Main(string[] args)
        {
           
            //Read Configuration from appSettings
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
            //Initialize Logger
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(config)
                .CreateLogger();
            try
            {
                Log.Information("Application Starting.");
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "The Application failed to start.");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
        public static MongoOptions MongoDBURL(string[] args)
        {

            var config = new ConfigurationBuilder()
           .SetBasePath(Directory.GetCurrentDirectory())
           .AddJsonFile($"appsettings.json", optional: true, reloadOnChange: true)
           .AddCommandLine(args)
           .Build();

            var options = new MongoOptions();
            var section = config.GetSection("AppLogRepository");
            section.Bind(options);
            return options;
        }
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args).UseSerilog()/*UseSerilog((_, config) =>
            {
                config
                    .MinimumLevel.Information()
                    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                    .Enrich.FromLogContext()
                    .WriteTo.MongoDB(MongoDBURL(args).ConnectionString + "/" + MongoDBURL(args).Database, collectionName: MongoDBURL(args).Collection);
            })*/
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<Worker>();
                });
    }
}
