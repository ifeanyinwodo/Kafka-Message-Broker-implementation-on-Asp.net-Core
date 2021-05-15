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
using Workerservice_Consumer.Metrics;


namespace Workerservice_Consumer
{
    public class Program
    {
        public static void Main(string[] args)
        {
           
         
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
                File.WriteAllText(@"D:\logstash\product1.log", ex.ToString());
                Log.Fatal(ex, "The Application failed to start.");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
        
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args).UseSerilog()
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<Worker>();
                });
    }
}
