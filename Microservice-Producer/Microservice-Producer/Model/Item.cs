using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservice_Producer.Model
{
    public class Item
    {
        public Guid Id { get; set; }
        public string Name {get; set;}

        public double Price { get; set; }

    }
}
