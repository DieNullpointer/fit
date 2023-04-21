using BeamerProtector.Application.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BeamerProtector.Webapp.Services
{
    /// <summary>
    /// Hosted Service to send notifications and alerts without user interaction.
    /// </summary>
    public class MonitorService : BackgroundService
    {
        private readonly ILogger<MonitorService> _logger;
        private readonly IServiceProvider _services;
        private readonly TimeSpan _pollInterval;
        private readonly byte[] _key;

        public MonitorService(IServiceProvider services, ILogger<MonitorService> logger, IConfiguration _config)
        {
            _services = services;
            _pollInterval = TimeSpan.FromSeconds(10);
            _logger = logger;
            _key = Convert.FromBase64String(_config["Secret"] ?? throw new ApplicationException("Missing secret in config."));
        }

        private async Task SendMissingDeviceMails()
        {
            using var scope = _services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<BeamerProtectorContext>();
            var mailer = await db.GetMailerAccount(_key);
            if (mailer.accountname is null || mailer.refreshToken is null)
            {
                _logger.LogWarning("No account configured to send alert emails.");
                return;
            }
            var adClinet = scope.ServiceProvider.GetRequiredService<AzureAdClient>();
            var tokens = await adClinet.GetNewToken(mailer.refreshToken);
            if (tokens.refreshToken is not null)
                await db.SetMailerAccount(mailer.accountname, tokens.refreshToken, _key);
            else
                _logger.LogWarning("GraphAPI did not provide a new refresh token.");

            var graph = adClinet.GetGraphServiceClientFromToken(tokens.authToken);
            var me = await graph.Me.Request().GetAsync();
            _logger.LogInformation($"{me.Mail} can send alerts with refresh token {tokens.refreshToken}.");
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await SendMissingDeviceMails();
                }
                catch (Exception e)
                {
                    _logger.LogError(e.InnerException?.Message ?? e.Message);
                }
                await Task.Delay(_pollInterval, stoppingToken);
            }
        }
    }
}