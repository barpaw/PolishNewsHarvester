using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PolishNewsHarvesterWorker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private IConfiguration _configuration;

        public Worker(ILogger<Worker> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {

                _ = Task.Run(() =>
                {
                    try
                    {
                        _logger.LogInformation("{workerName}: Worker running at: {WorkerRunningTime}", _configuration["app:workerName"], DateTimeOffset.Now);

                        RunWorker();

                    }
                    catch (Exception e)
                    {
                        _logger.LogError(e, "{workerName}: Exception caught in ExecuteAsync method.", _configuration["app:workerName"]);
                    }

                });

                await Task.Delay(TimeSpan.FromSeconds(1), stoppingToken);
            }
        }

        private void RunWorker()
        {
            _logger.LogInformation("{workerName}: Ok", _configuration["app:workerName"]);
        }
    }
}
