using BotFramework;
using System;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types.Enums;
using SteamStatusBot.SteamStats;
using BotFramework.Attributes;
using BotFramework.Setup;
using System.Collections.Concurrent;

namespace SteamStatusBot.Handler
{
    public class HanldersCommands : BotEventHandler
    {
        public IClient client;
        public ConcurrentBag<long> bag;

        public HanldersCommands(IClient client, ConcurrentBag<long> bag)
        {
            this.bag = bag;
            this.client = client;
        }

        [Command(InChat.All, "status", CommandParseMode.Both)]
        public async Task SendStatus()
        {
            Console.WriteLine("Client used command 'status'");
            Json json = client.GetStatus();
            if (json == null) Console.WriteLine("FAILED");
            bag.Add(Chat.Id);
            await Bot.SendTextMessageAsync(Chat, $"Steam Connection Manager: `{json.Services.Where(p => (string)p[0] == "cms").Select(p => p[2]).FirstOrDefault()}`", ParseMode.Markdown);
        }
    }
}