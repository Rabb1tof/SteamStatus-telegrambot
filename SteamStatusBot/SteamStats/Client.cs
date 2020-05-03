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
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Timer = System.Threading.Timer;

namespace SteamStatusBot.SteamStats
{
    public interface IClient
    {
        Json GetStatus();
    }
    public class Client : IClient, IHostedService, IDisposable
    {
        protected Timer Timer;
        public Json json;
        protected readonly ITelegramBot _bot;
        public ConcurrentBag<long> _bag;

        public Client(ITelegramBot bot, ConcurrentBag<long> bag)
        {
            _bag = bag;
            _bot = bot;
        }

        public async Task StartAsync(CancellationToken stoppingToken)
        {
            Timer = new Timer(_UpdateTimer, null, TimeSpan.Zero,
                TimeSpan.FromSeconds(45));
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
            if (json != null && json.Online < 75 && data.Online >= 75)
            {
                // SendMessage (Steam is back)
                foreach (var i in _bag)
                {
                    await _bot.BotClient.SendTextMessageAsync(i, "Test", ParseMode.Markdown);
                }
                //_bot.BotClient.SendTextMessageAsync(new BotData().ChatId);
            }
            json = data;
        }

        public Json GetStatus()
        {
            return json;
        }

        public void Dispose()
        {
            Timer?.Dispose();
        }
    }
}
