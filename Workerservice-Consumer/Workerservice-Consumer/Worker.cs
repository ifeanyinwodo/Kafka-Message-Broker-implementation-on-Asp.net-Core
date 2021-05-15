using Confluent.Kafka;
using ItemModel_Nugget;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using Serilog;
using Serilog.Events;
using System.Threading;
using System.Threading.Tasks;
using Workerservice_Consumer.Metrics;
using Serilog.Core;
using System.IO;

namespace Workerservice_Consumer
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly string _groupId;
        private readonly string _bootstrapServers;
        private readonly string _topic;
        private readonly IConfiguration _configuration;
        

        public Worker(ILogger<Worker> logger, IConfiguration configuration)
        {
            File.WriteAllText(@"D:\logstash\product.log", "1");
            _configuration = configuration;
            _groupId = configuration.GetSection("consumer").GetSection("GroupId").Value;
            _bootstrapServers = configuration.GetSection("consumer").GetSection("Bootstrapservers").Value;
            _topic = configuration.GetSection("consumer").GetSection("Topic").Value;
            _logger = logger;
            
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            MetricsRegistry _metricsRegistry = new MetricsRegistry(_configuration);
            Logger _receivedMessageounter = _metricsRegistry._receivedMessage();
            var counter = _receivedMessageounter.CountOperation("counter", "operation(s)", true, LogEventLevel.Information);
            counter.Increment();

            try
            {
                var config = new ConsumerConfig
                {
                    GroupId = _groupId,
                    BootstrapServers = _bootstrapServers,
       
                };
          
            using (var consumer = new ConsumerBuilder<Null, string>(config).Build())
                {
                    consumer.Subscribe(_topic);
                    while (!stoppingToken.IsCancellationRequested)
                    {

                        var cr = consumer.Consume();
                        Item item = JsonConvert.DeserializeObject<Item>(cr.Message.Value);

                        Console.WriteLine("Product Name " + item.Name);
                        
                        await Task.Delay(1000, stoppingToken);
                        
                    }
                }
            }
            catch (Exception ex)
            {
               
                _logger.LogError("Exception Occured: {ex}", ex.ToString());
            }

        }
    }
}
