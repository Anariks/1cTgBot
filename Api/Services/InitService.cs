using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Api.Configurations;

namespace Api.Services;

class InitService : IHostedService
{   
    private ITelegramBotClient _telegramBotClient;
    private readonly IOptionsMonitor<BotConfiguration> _botConfiguration;
    private readonly ILogger<InitService> _logger;

    public InitService(ITelegramBotClient telegramBotClient, IOptionsMonitor<BotConfiguration> botConfiguration, ILogger<InitService> logger)
    {
        _telegramBotClient = telegramBotClient;
        _botConfiguration = botConfiguration;
        _logger = logger;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {   
        var domain = _botConfiguration.CurrentValue.Domain;
        var token = _botConfiguration.CurrentValue.BotAccessToken;
        return _telegramBotClient.SetWebhookAsync($"{domain}/api/bot",
            allowedUpdates: Enumerable.Empty<UpdateType>(),
            cancellationToken: cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.FromResult(0);
        //return _telegramBotClient.DeleteWebhookAsync(cancellationToken: cancellationToken);
    }
}