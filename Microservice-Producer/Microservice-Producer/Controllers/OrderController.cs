using Confluent.Kafka;
using ItemModel_Nugget;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
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
        public OrderController(ProducerConfig producerConfig, ILogger<OrderController> logger, IConfiguration configuration)
        {
            this._producerConfig = producerConfig;
            _topic = configuration.GetSection("producer").GetSection("Topic").Value;
            _logger = logger;
        }
        

        // POST api/<OrderController>
        [HttpPost]
        public async Task<IActionResult> Post( [FromBody] Item item)
        {
            
            string serializeItem = JsonConvert.SerializeObject(item);
            using(var producer = new ProducerBuilder<Null, string>(_producerConfig).Build())
            {
                await producer.ProduceAsync(_topic, new Message<Null, string> { Value = serializeItem });
               
                producer.Flush(TimeSpan.FromSeconds(10));
                return Ok(true);
            }

        }

        
    }
}
