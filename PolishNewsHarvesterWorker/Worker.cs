using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PolishNewsHarvesterSdk.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Data.SqlClient.Server;
using PolishNewsHarvesterSdk.Dto;

namespace PolishNewsHarvesterWorker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private IConfiguration _configuration;
        private IParser _parser;


        public Worker(ILogger<Worker> logger, IConfiguration configuration, IParser parser)
        {
            _logger = logger;
            _configuration = configuration;
            _parser = parser;
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

                await Task.Delay(TimeSpan.FromSeconds(100), stoppingToken);
            }
        }

        private async void RunWorker()
        {
            _logger.LogInformation("{workerName}: Ok", _configuration["app:workerName"]);

            var list = new List<Func<string, ParseMethodResultDto>>();

            list.Add(TestMethod);
            list.Add(TestMethod2);
            list.Add(TestMethod3);

            _parser.FetchAndParse("https://wiadomosci.wp.pl/tag/covid", list);
        }

        public ParseMethodResultDto TestMethod(string body)
        {
            body = "x";
            var ret = $"{body} test";

            return new ParseMethodResultDto("Test1", ret, ret.GetType());
        }

        public ParseMethodResultDto TestMethod2(string body)
        {
            body = " y ";
            var ret = $"{body} test2";

            return new ParseMethodResultDto("Test2", ret, ret.GetType());
        }
        
        public ParseMethodResultDto TestMethod3(string body)
        {
            
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(body);
            
            //data-reactid="240"


            foreach (HtmlNode divNode in htmlDoc.DocumentNode.SelectNodes("//a[@class='f2PrHTUx']"))
            {
                
                // foreach(HtmlNode link in divNode.SelectNodes("//a[@href]"))
                // {
                //     _logger.LogInformation(link.InnerText);
                // }
                _logger.LogInformation(divNode.InnerText.Trim());
            }
            
            /*
            foreach(HtmlNode link in x.SelectNodes("//a[@href]"))
            {
                _logger.LogInformation(link.InnerText);
            }
            */

            var ret = "ds";
            return new ParseMethodResultDto("Test3", ret, ret.GetType());
        }
    }
}