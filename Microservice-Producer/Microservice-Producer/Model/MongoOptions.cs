using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservice_Producer.Model
{
    public class MongoOptions
    {
        public string ConnectionString { get; set; }
        public string Database { get; set; }

        public string Collection { get; set; }
    }
}
