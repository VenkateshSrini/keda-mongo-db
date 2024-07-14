using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mongodb.job.Model
{
    public class Candy
    {
        [BsonId]
        public string Id { get; set; } = new Guid().ToString();
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Taste { get; set; } = string.Empty;
        public float Price { get; set; }
        public string ProcessingStatus { get; set; } = "created";
    }
}
