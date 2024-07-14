using MongoDB.API.Model;

namespace MongoDB.API.Repository
{
    public interface ICandyRepo
    {
        IEnumerable<Candy> GetCandies();
        Candy GetCandyById(string id);
        void CreateCandy(Candy candy);
        void BulkCreateCandy(List<Candy> candies);
    }
}
