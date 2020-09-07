using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace KashServer
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private IServer Server;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
            IPAddress ip = IPAddress.Parse("127.0.0.1");
            Server = new Server(ip, 9000, _logger);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                Server.StartServer();
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
