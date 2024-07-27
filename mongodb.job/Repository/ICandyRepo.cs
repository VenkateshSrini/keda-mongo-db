using mongodb.job.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mongodb.job.Repository
{
    public interface ICandyRepo
    {
        Task<Candy> GetCandyById(string id);
        Task<Candy>GetFirstCandyByStatus(string status);
        Task<int> GetCandiesCountByStatus(string status);
        Task<bool>UpdateStatus(string id, string status);
    }
}
