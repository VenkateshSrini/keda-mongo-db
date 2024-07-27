using mongodb.job.Repository;

namespace mongodb.job
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly ICandyRepo _candyRepo;
        private readonly IDistributedLock _lockRepo;
        private readonly string podName;

        public Worker(ILogger<Worker> logger, ICandyRepo candyRepo, IConfiguration configuration, IDistributedLock lockRepo)
        {
            _logger = logger;
            _candyRepo = candyRepo;
            _lockRepo = lockRepo;
            podName= configuration["podName"];

        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            
            var getToProcessCandyCount = await _candyRepo.GetCandiesCountByStatus("created");
            for (int i = 0; i < getToProcessCandyCount; i++)
            {
                var candy = await _candyRepo.GetFirstCandyByStatus("created");
                if (candy != null)
                {
                    var lockId= $"{candy.Id}";
                    bool locked = await _lockRepo.AcquireLockAsync(
                        new Model.DBLock { Id = lockId, PodName= podName,Locked=true, ExpiresAt=DateTime.UtcNow } 
                       
                    );
                    if(locked)
                    {
                        _logger.LogInformation($"Processing candy {candy.Name}");
                        await _candyRepo.UpdateStatus(candy.Id, "processing");
                        await _candyRepo.UpdateStatus(candy.Id, "processed");
                        _logger.LogInformation($"Candy {candy.Name} processed");
                        await _lockRepo.ReleaseLockAsync(lockId);
                        break;
                    }
                }
            }
                   
            
        }
    }
}
