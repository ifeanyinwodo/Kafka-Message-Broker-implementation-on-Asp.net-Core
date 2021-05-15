using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Sinks.Kafka;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Workerservice_Consumer.Metrics
{
    public class MetricsRegistry 
    {

        private readonly int _batchSizeLimit;
        private readonly int _period;
        private readonly string _bootstrapServers;
        private readonly string _topic;
        public IConfiguration _configuration;
        private readonly string _application;

        public MetricsRegistry(IConfiguration configuration)
        {
            _configuration = configuration;
            _batchSizeLimit = int.Parse(_configuration["KafkaSink:batchSizeLimit"]);
            _period = int.Parse(_configuration["KafkaSink:period"]);
            _bootstrapServers = _configuration["KafkaSink:bootstrapServers"];
            _topic = _configuration["KafkaSink:topic"];
            _application = "Workerservice-Consumer";
        }
        public Logger _receivedMessage()
        {
            var _logger = new LoggerConfiguration()
                 .WriteTo.Kafka(this._bootstrapServers, _batchSizeLimit, _period, Confluent.Kafka.SecurityProtocol.Plaintext, Confluent.Kafka.SaslMechanism.Plain, _topic)
                 .MinimumLevel.Verbose()
                 .CreateLogger();
           
            return _logger;
        }

       
    }
}
