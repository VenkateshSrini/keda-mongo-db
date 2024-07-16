using mongodb.job.Repository;

namespace mongodb.job
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly ICandyRepo _candyRepo;

        public Worker(ILogger<Worker> logger, ICandyRepo candyRepo)
        {
            _logger = logger;
            _candyRepo = candyRepo;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            //while (!stoppingToken.IsCancellationRequested)
            //{
            //    if (_logger.IsEnabled(LogLevel.Information))
            //    {
            //        _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            //    }
            //    await Task.Delay(1000, stoppingToken);
            //}
            var candy = await _candyRepo.GetFirstCandyByStatus("created");
            if (candy != null)
            {
                _logger.LogInformation($"Processing candy {candy.Name}");
                await _candyRepo.UpdateStatus(candy.Id, "processing");
                await Task.Delay(60 * 1000);
                _logger.LogInformation($"Candy {candy.Name} processed");
                await _candyRepo.UpdateStatus(candy.Id, "processed");
            }
            else
            {
                _logger.LogInformation("No candy to process");
            }
        }
    }
}
