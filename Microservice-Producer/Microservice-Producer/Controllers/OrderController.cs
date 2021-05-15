using Confluent.Kafka;
using ItemModel_Nugget;
using Microservice_Producer.Metrics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Serilog;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Microservice_Producer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {

        private  ProducerConfig _producerConfig;
        private readonly string _topic;
        private readonly IConfiguration _configuration;
        private readonly ILogger<OrderController> _logger;
        private readonly IMetricsRegistry _metricsRegistry;
        private readonly Serilog.Core.Logger _receivedMessageounter;
        public OrderController(ProducerConfig producerConfig, ILogger<OrderController> logger, IConfiguration configuration, IMetricsRegistry metricsRegistry)
        {
            this._producerConfig = producerConfig;
            _topic = configuration.GetSection("producer").GetSection("Topic").Value;
            _logger = logger;
            _metricsRegistry = metricsRegistry;
            _receivedMessageounter = _metricsRegistry._receivedMessage();
        }
        

        // POST api/<OrderController>
        [HttpPost]
        public async Task<IActionResult> Post( [FromBody] Item item)
        {
            var counter = _receivedMessageounter.CountOperation("counter", "operation(s)", true, LogEventLevel.Information);
            counter.Increment();
            var random = new Random();
            var randomValue = random.Next(0, 100);
            _logger.LogInformation($"Random Value is {randomValue}");

            string serializeItem = JsonConvert.SerializeObject(item);
            using(var producer = new ProducerBuilder<Null, string>(_producerConfig).Build())
            {
                await producer.ProduceAsync(_topic, new Message<Null, string> { Value = serializeItem });
               
                producer.Flush(TimeSpan.FromSeconds(10));
                _logger.LogInformation("Item Post Successfull at: {time}", DateTimeOffset.Now);
                return Ok(true);
            }

        }

        
    }
}
