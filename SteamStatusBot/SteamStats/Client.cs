using BotFramework;
using BotFramework.Attributes;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using SteamStatusBot.Handler;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using SteamStatusBot.Database;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Timer = System.Threading.Timer;

namespace SteamStatusBot.SteamStats
{
    public interface IClient
    {
        Json GetJson();
    }
    public class Client : IClient, IHostedService, IDisposable
    {
        protected Timer Timer;
        public Json json;
        protected readonly IServiceScopeFactory _scopeFactory;
        protected readonly ITelegramBot _bot;
        public string cms;

        public Client(ITelegramBot bot, IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
            _bot = bot;
        }

        public async Task StartAsync(CancellationToken stoppingToken)
        {
            Timer = new Timer(_UpdateTimer, null, TimeSpan.Zero,
                TimeSpan.FromSeconds(60));
            await Update();
        }

        public Task StopAsync(CancellationToken stoppingToken)
        {
            Timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public async Task<Json> GetAsync()
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("Microsoft.CSharp", "1.0"));
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls |
                                                       SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                return JsonConvert.DeserializeObject<Json>(
                    await client.GetStringAsync("https://crowbar.steamstat.us/gravity.json"));
            }
        }

        public async void _UpdateTimer(object obj)
        {
            await Update();
        }

        public async Task Update()
        {
            var data = await GetAsync();
            using var scope = _scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
            if (json != null)
            {
                
                cms = $"Steam Connection Manager: `{json.Services.Where(p => (string) p[0] == "cms").Select(p => p[2]).FirstOrDefault()}`";
                if (json.Online < 75 && data.Online >= 75)
                {
                    // SendMessage (Steam is back)
                    

                    foreach (var i in dbContext.Users)
                    {
                        await _bot.BotClient.SendTextMessageAsync(i.ChatId, cms, ParseMode.Markdown);
                    }

                    //_bot.BotClient.SendTextMessageAsync(new BotData().ChatId);
                }
            } else if (data != null && json == null) {
                foreach (var i in dbContext.Users)
                {
                    await _bot.BotClient.SendTextMessageAsync(i.ChatId, "JSON Service is *back* or bot has *reloaded*!", ParseMode.Markdown);
                }
            }
            json = data;
        }

        public Json GetJson()
        {
            return json;
        }

        public void Dispose()
        {
            Timer?.Dispose();
        }
    }
}
