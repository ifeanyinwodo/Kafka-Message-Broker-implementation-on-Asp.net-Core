using Confluent.Kafka;
using Microservice_Producer.Model;
using Microsoft.AspNetCore.Mvc;
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

        public OrderController(ProducerConfig producerConfig)
        {
            this._producerConfig = producerConfig;
        }
        

        // POST api/<OrderController>
        [HttpPost]
        public async Task<IActionResult> Post(string topic, [FromBody] Item item)
        {
            string serializeItem = JsonConvert.SerializeObject(item);
            using(var producer = new ProducerBuilder<Null, string>(_producerConfig).Build())
            {
                await producer.ProduceAsync(topic, new Message<Null, string> { Value = serializeItem });
               
                producer.Flush(TimeSpan.FromSeconds(10));
                return Ok(true);
            }

        }

        
    }
}
