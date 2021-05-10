using Confluent.Kafka;
using ItemModel_Nugget;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

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
            _configuration = configuration;
            _groupId = configuration.GetSection("consumer").GetSection("GroupId").Value;
            _bootstrapServers = configuration.GetSection("consumer").GetSection("Bootstrapservers").Value;
            _topic = configuration.GetSection("consumer").GetSection("Topic").Value;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                File.WriteAllText(@"d:\logstash\product.log", "1");
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            File.WriteAllText(@"d:\logstash\product.log", "2");
            var config = new ConsumerConfig
                {
                    GroupId = _groupId,
                    BootstrapServers = _bootstrapServers,
                    
                    
                    
                };
            File.WriteAllText(@"d:\logstash\product.log", "3");
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
