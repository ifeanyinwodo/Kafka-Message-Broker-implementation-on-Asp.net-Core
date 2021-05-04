using Confluent.Kafka;
using ItemModel_Nugget;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
            _groupId = configuration.GetSection("consumer").GetSection("").Value;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            var config = new ConsumerConfig
            {
                GroupId = "",
                BootstrapServers = ""

            };
            using (var consumer = new ConsumerBuilder<Null, string>(config).Build())
            {
                consumer.Subscribe("");
                while (!stoppingToken.IsCancellationRequested)
                {
                    
                    var cr = consumer.Consume();
                    Item item = JsonConvert.DeserializeObject<Item>(cr.Message.Value);
                    Console.WriteLine("Product Name " + item.Name);
                    await Task.Delay(1000, stoppingToken);
                }
            }
            
        }
    }
}
