using MongoDB.API.Model;
using MongoDB.Driver;

namespace MongoDB.API.Repository
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
        public void BulkCreateCandy(List<Candy> candies)
        {
            _candiesCollection.InsertMany(candies);
        }

        public void CreateCandy(Candy candy)
        {
            _candiesCollection.InsertOne(candy);
        }

        public IEnumerable<Candy> GetCandies()
        {
            return _candiesCollection.AsQueryable().ToList();
        }

        public Candy GetCandyById(string id)
        {
           return _candiesCollection.Find(c => c.Id == id).FirstOrDefault();
        }
    }
}
