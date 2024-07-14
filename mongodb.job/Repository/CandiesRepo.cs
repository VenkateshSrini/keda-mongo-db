using mongodb.job.Model;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mongodb.job.Repository
{
    public class CandiesRepo : ICandyRepo
    {
        private IMongoCollection<Candy> _candiesCollection;
        private readonly ILogger<CandiesRepo> _logger;
        public CandiesRepo(IMongoClient mongoClient, MongoUrl mongoUrl,
            ILogger<CandiesRepo> logger)
        {
            _logger = logger;
            var db = mongoClient.GetDatabase(mongoUrl?.DatabaseName);
            var collectionName = "Candies-ToProcess";
            _candiesCollection = db.GetCollection<Candy>(collectionName);
            _logger = logger;
        }
        public async Task<Candy> GetCandyById(string id)
        {
            return (await _candiesCollection.FindAsync(c => c.Id == id)).FirstOrDefault();
        }
        public async Task<Candy> GetFirstCandyByStatus(string status)
        {
            return (await _candiesCollection.FindAsync(c => c.ProcessingStatus == status)).FirstOrDefault();
        }

        public async Task<bool> UpdateStatus(string id, string status)
        {
            var filter = Builders<Candy>.Filter.Eq(nameof(Candy.Id), id);
            var update = Builders<Candy>.Update.Set(nameof(Candy.ProcessingStatus), status);
            var modifiedResult = await _candiesCollection.UpdateOneAsync(filter, update);
            return modifiedResult.ModifiedCount > 0;
        }
    }
}
