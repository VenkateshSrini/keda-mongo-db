using mongodb.job.Repository;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mongodb.job.ServiceExtension
{
    public static class RepoExtension
    {
        public static IServiceCollection AddMongoRepo(this IServiceCollection services,
            IConfiguration configuration, string sectionName, string connectionStrringAttribute)
        {
            var mongourl = MongoUrl.Create(configuration[$"{sectionName}:{connectionStrringAttribute}"]);
            var mongoClient = new MongoClient(mongourl);
            services.AddSingleton<MongoUrl>(mongourl);
            services.AddSingleton<IMongoClient>(mongoClient);
            services.AddSingleton<ICandyRepo, CandiesRepo>();
            return services;
        }
    }
}
