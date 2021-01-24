using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PolishNewsHarvesterSdk.Http;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Data.SqlClient.Server;
using Newtonsoft.Json;
using PolishNewsHarvesterSdk;
using PolishNewsHarvesterSdk.Dto;
using PolishNewsHarvesterSdk.Targets;
using PolishNewsHarvesterCommon.NewsSites;

namespace PolishNewsHarvesterWorker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private IConfiguration _configuration;
        private IWirtualnaPolska _wirtualnaPolska;


        public Worker(ILogger<Worker> logger, IConfiguration configuration, IWirtualnaPolska wirtualnaPolska)
        {
            _logger = logger;
            _configuration = configuration;
            _wirtualnaPolska = wirtualnaPolska;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _ = Task.Run(() =>
                {
                    try
                    {
                        _logger.LogInformation("{workerName}: Worker running at: {WorkerRunningTime}",
                            _configuration["app:workerName"], DateTimeOffset.Now);

                        RunWorker();
                    }
                    catch (Exception e)
                    {
                        _logger.LogError(e, "{workerName}: Exception caught in ExecuteAsync method.",
                            _configuration["app:workerName"]);
                    }
                });

                await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
            }
        }

        private async void RunWorker()
        {
            _logger.LogInformation("{workerName}: Ok", _configuration["app:workerName"]);

            _wirtualnaPolska.GetNewsByTag("covid19");


        }

    }
}