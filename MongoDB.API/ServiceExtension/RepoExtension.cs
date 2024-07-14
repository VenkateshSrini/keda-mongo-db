using MongoDB.API.Repository;
using MongoDB.Driver;

namespace MongoDB.API.ServiceExtension
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
