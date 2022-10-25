using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

using Domain.Services;
using Domain.Services.Interfaces;
using System.Diagnostics;

namespace Api.Services;

public class TelegramService
{
    private readonly ITelegramBotClient _telegramBotClient;
    private readonly IProductInfoService _productInfoService;
    private readonly DatabaseService _databaseService;
    private readonly MessageService _messageService;
    private readonly ILogger<TelegramService> _logger;

    public TelegramService(ITelegramBotClient telegramBotClient, IProductInfoService productInfoService,
         DatabaseService databaseService, MessageService messageService, ILogger<TelegramService> logger)
    {
        _telegramBotClient = telegramBotClient;
        _productInfoService = productInfoService;
        _databaseService = databaseService;
        _messageService = messageService;
        _logger = logger;
    }

    public async Task HandleMessage(Update update, CancellationToken cancellationToken = default)
    {
        _logger.LogDebug("Message Handling Started");

        if (update.Type != UpdateType.Message)
        {
            await _telegramBotClient.SendTextMessageAsync(update.Message.Chat.Id, $"I cannot handle {update.Type}", cancellationToken: cancellationToken);
            return;
        }

        var text = update.Message.Text;

        if (text.Equals("/start"))
        {
            await _telegramBotClient.SendTextMessageAsync(update.Message.Chat.Id, "Welcome to our bot!", cancellationToken: cancellationToken);
            return;
        }

        if (text.Equals("/update"))
        {
            try
            {
                //await _databaseService.FillDatabase();
                await _telegramBotClient.SendTextMessageAsync(update.Message.Chat.Id, "Sorry, this options isn't enabled now", cancellationToken: cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                await _telegramBotClient.SendTextMessageAsync(update.Message.Chat.Id, "Something went wrong (((" + ex.Message, cancellationToken: cancellationToken);
            }

            return;
        }

        var products = _productInfoService.GetProductInfoByQuery(text);

        foreach (var message in _messageService.CreateMessageForProducts(products, (int)update.Message.From.Id))
        {
            await _telegramBotClient.SendTextMessageAsync(update.Message.Chat.Id, message, cancellationToken: cancellationToken);
        }
    }
}