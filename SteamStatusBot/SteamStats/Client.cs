﻿using BotFramework.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Timer = System.Threading.Timer;

namespace SteamStatusBot.SteamStats
{
    public interface IClient
    {
        Json GetStatus();
    }
    public class Client : IClient
    {
        protected Timer Timer;
        public Json json;

        public Client()
        {
            Timer = new Timer(_UpdateTimer, null, TimeSpan.Zero, TimeSpan.FromSeconds(45));
            Update().GetAwaiter().GetResult();
        }

        ~Client()
        {
            Timer.Dispose();
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
            }
            json = data;
        }

        public Json GetStatus()
        {
            return json;
        }
    }
}