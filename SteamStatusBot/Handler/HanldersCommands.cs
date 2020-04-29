using BotFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SteamStatusBot.Handler;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using SteamStatusBot.SteamStats;
using BotFramework.Attributes;

namespace SteamStatusBot.Handler
{
    public class HanldersCommands : BotEventHandler
    {
        public IClient client;

        public HanldersCommands(IClient client)
        {
            this.client = client;
        }

        [Command("status")]
        public async Task SendStatus()
        {
            Console.WriteLine("Client used command 'status'");
            Json json = client.GetStatus();
            if (json == null) Console.WriteLine("FAILED");

            await Bot.SendTextMessageAsync(Chat, $"Steam Connection Manager: `{json.Services.Where(p => (string)p[0] == "cms").Select(p => p[2]).FirstOrDefault()}`", ParseMode.Markdown);
        }
    }
}