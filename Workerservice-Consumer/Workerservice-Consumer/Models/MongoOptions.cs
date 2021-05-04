using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Workerservice_Consumer.Models
{
    public class MongoOptions
    {
        public string ConnectionString { get; set; }
        public string Database { get; set; }

        public string Collection { get; set; }
    }
}
