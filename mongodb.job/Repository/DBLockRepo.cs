using Microsoft.Extensions.Configuration;
using mongodb.job.Model;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mongodb.job.Repository
{
    public class DBLockRepo: IDistributedLock
    {
        private IMongoCollection<DBLock> _collection;
        private readonly ILogger<DBLockRepo> _logger;
        private readonly IConfiguration _configuration;
        public DBLockRepo(IMongoClient mongoClient, MongoUrl mongoUrl,
            ILogger<DBLockRepo> logger, IConfiguration configuration) { 
            _logger = logger;
            _configuration = configuration;
            var db = mongoClient.GetDatabase(mongoUrl?.DatabaseName);
            var collectionName = "DistributedLock";
            _collection = db.GetCollection<DBLock>(collectionName);

            // Ensure TTL index is created
            var indexKeysDefinition = Builders<DBLock>.IndexKeys.Ascending(d => d.ExpiresAt);
            var indexOptions = new CreateIndexOptions { ExpireAfter = TimeSpan.FromMinutes(10) };
            var indexModel = new CreateIndexModel<DBLock>(indexKeysDefinition, indexOptions);
            _collection.Indexes.CreateOne(indexModel);
        }
        public async Task<bool> AcquireLockAsync(Model.DBLock dbLock)
        {
           
            //var filter = Builders<DBLock>.Filter.And(
            //Builders<DBLock>.Filter.Eq(d => d.Id, dbLock.Id),
            //Builders<DBLock>.Filter.Or(
            //        Builders<DBLock>.Filter.Eq(d => d.Locked, false),
            //        Builders<DBLock>.Filter.Lte(d => d.ExpiresAt, now)
            //    )
            //);

            //var update = Builders<DBLock>.Update
            //.Set(d => d.Locked, true)
            //.Set(d => d.PodName, dbLock.PodName)
            //.Set(d => d.ExpiresAt, expiresAt);

            //var options = new UpdateOptions { IsUpsert = true };
            try
            {

                await _collection.InsertOneAsync(dbLock);
               _logger.LogWarning($"Lock Accquired for {dbLock.Id} by PodName {_configuration["podName"]}");
                
                return true;
            }
            catch (MongoWriteException ex)
            {
                if (ex.WriteError.Category == ServerErrorCategory.DuplicateKey)
                {
                    _logger.LogError("Duplicate key error while acquiring lock: {0}", ex.Message);
                    return false;
                }
                throw;
            }

            
        }

        public async Task<bool> ReleaseLockAsync(string dbLockId)
        {
            var filter = Builders<DBLock>.Filter.Eq(d => d.Id, dbLockId);
            var update = Builders<DBLock>.Update.Set(d => d.Locked, false);

            var result = await _collection.UpdateOneAsync(filter, update);
            return result.ModifiedCount > 0;
        }
    }
}
