using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Headers;

using Telegram.Bot;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;

using Api.Configurations;
using Api.Services;
using Domain.Services;
using Domain.Services.Interfaces;
using Domain.Database;
using Domain.XmlData;
using Domain.ApiClient.Interfaces;
using Domain.ApiClient;
using Serilog;
using Api.Controllers.Helpers;
using Api.Services.Helpers;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog();
// Add services to the container.

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Error()
    .Enrich.FromLogContext()
    .WriteTo.File("log.txt", rollingInterval: RollingInterval.Day, fileSizeLimitBytes: 5000000)
    .WriteTo.Console()
    .CreateLogger();

builder.Services.AddControllers().AddNewtonsoftJson();

builder.Services.Configure<BotConfiguration>(builder.Configuration.GetSection("TelegramSettings"));
builder.Services.Configure<ApiConfiguration>(builder.Configuration.GetSection("YyApiData"));
builder.Services.Configure<Client1cConfiguration>(builder.Configuration.GetSection("Client1cCredentials"));

builder.Services.AddHttpClient("tgclient")
    .AddTypedClient<ITelegramBotClient>((client, sp) =>
    {
        var configuration = sp.GetRequiredService<IOptionsMonitor<BotConfiguration>>();
        return new TelegramBotClient(configuration.CurrentValue.BotAccessToken, client);
    });    

builder.Services.AddTransient<TelegramService>();

builder.Services.AddHostedService<InitService>();

builder.Services.AddHttpClient("yyapi", client =>
    {    
        client.BaseAddress = new Uri(builder.Configuration["YyApiData:Url"]);
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
        "Basic", Convert.ToBase64String(
            System.Text.ASCIIEncoding.ASCII.GetBytes(
               $"{builder.Configuration["YyApiData:ApiKey"]}:{builder.Configuration["YyApiData:ApiSecret"]}")));
        client.Timeout = TimeSpan.FromSeconds(10);
    })
    .AddTypedClient<IProductApiClient>(client =>
        new ProductApiClient(client));
  
builder.Services.AddDbContext<BotDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("myDb"),
     new MySqlServerVersion(new Version(8, 0, 26)))
     .EnableSensitiveDataLogging(),
    contextLifetime: ServiceLifetime.Transient, 
    optionsLifetime: ServiceLifetime.Singleton);

builder.Services.AddTransient<DataFromXml>();
builder.Services.AddTransient<Authorization>();
builder.Services.AddSingleton<IMessagesToUsers, MessageForUsers>();
builder.Services.AddSingleton<IProductInfoService, ProductInfoService>();
builder.Services.AddTransient<GetDataFromDb>();
builder.Services.AddSingleton<DataToDatabase>();
builder.Services.AddSingleton<DatabaseService>();
builder.Services.AddSingleton<MessageService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // app.UseSwagger();
    // app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

//app.UseAuthorization();

app.MapControllers();

app.Run();


