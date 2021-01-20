using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PolishNewsHarvesterSdk.Http;
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
        private IHttpManager _httpManager;


        public Worker(ILogger<Worker> logger, IConfiguration configuration, IHttpManager httpManager)
        {
            _logger = logger;
            _configuration = configuration;
            _httpManager = httpManager;
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

                await Task.Delay(TimeSpan.FromSeconds(100), stoppingToken);
            }
        }

        private async void RunWorker()
        {
            _logger.LogInformation("{workerName}: Ok", _configuration["app:workerName"]);
            var x = await _httpManager.SendGetRequestAsync("https://wiadomosci.wp.pl/tag/test");
            var y = await x.Content.ReadAsStringAsync();
            _logger.LogInformation(y);

        }
    }
}
