using Microsoft.AspNetCore.Mvc;
using Telegram.Bot;

using Api.Services;
using Telegram.Bot.Types;

namespace Api.Controllers;

public class BotController : ControllerBase
{
    private readonly TelegramService _telegramService;

    public BotController(TelegramService telegramService)
    {
        _telegramService = telegramService;

    }

    [HttpPost("api/bot")]
    public async Task<IActionResult> Post([FromBody] Update update, CancellationToken cancellationToken = default)
    {
        await _telegramService.HandleMessage(update, cancellationToken);
        return Ok();
    }
}
