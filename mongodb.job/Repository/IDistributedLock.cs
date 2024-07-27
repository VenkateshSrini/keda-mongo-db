using mongodb.job.Model;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mongodb.job.Repository
{
    public interface IDistributedLock
    {
        Task<bool> AcquireLockAsync(DBLock dbLock);
        Task<bool> ReleaseLockAsync(string dbLockId);
    }
}
