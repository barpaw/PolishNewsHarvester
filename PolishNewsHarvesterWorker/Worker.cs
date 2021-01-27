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
using PolishNewsHarvesterCommon.NewsSites;
using PolishNewsHarvesterSdk.NewsSites;

namespace PolishNewsHarvesterWorker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private IConfiguration _configuration;
        private IWirtualnaPolska _wirtualnaPolska;
        private IPolskaAgencjaPrasowa _polskaAgencjaPrasowa;
        private ITvpInfo _tvpInfo;

        public Worker(ILogger<Worker> logger, IConfiguration configuration, IWirtualnaPolska wirtualnaPolska, IPolskaAgencjaPrasowa polskaAgencjaPrasowa, ITvpInfo tvpInfo)
        {
            _logger = logger;
            _configuration = configuration;
            _wirtualnaPolska = wirtualnaPolska;
            _polskaAgencjaPrasowa = polskaAgencjaPrasowa;
            _tvpInfo = tvpInfo;
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

                await Task.Delay(TimeSpan.FromSeconds(99999), stoppingToken);
            }
        }

        private async void RunWorker()
        {
            _logger.LogInformation("{workerName}: Ok", _configuration["app:workerName"]);


            var tag = "szczepionka";
            var tag2 = "covid-19";

            await _wirtualnaPolska.GetNewsByTag(tag);
            await _wirtualnaPolska.GetNewsByTag(tag2);

            await _polskaAgencjaPrasowa.GetNewsByTag(tag);
            await _polskaAgencjaPrasowa.GetNewsByTag(tag2);
            

            await _tvpInfo.GetNewsByTag(tag);
            await _tvpInfo.GetNewsByTag(tag2);


        }

    }
}