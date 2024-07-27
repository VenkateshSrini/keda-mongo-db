using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mongodb.job.Model
{
    public class DBLock
    {
        [BsonId]
        public string Id { get; set; }
        public bool Locked { get; set; }
        public string PodName { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}
