using BotFramework;
using System;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types.Enums;
using SteamStatusBot.SteamStats;
using BotFramework.Attributes;
using BotFramework.Setup;
using System.Collections.Concurrent;
using Microsoft.Extensions.DependencyInjection;
using SteamStatusBot.Database;

namespace SteamStatusBot.Handler
{
    public class HanldersCommands : BotEventHandler
    {
        public IClient client;
        protected readonly DatabaseContext _dbContext;

        public HanldersCommands(IClient client, DatabaseContext dbContext)
        {
            _dbContext = dbContext;
            this.client = client;
        }

        [Command(InChat.All, "status", CommandParseMode.Both)]
        public async Task SendStatus()
        {
            Console.WriteLine("Client used command 'status'");
            var json = client.GetJson();
            if (json == null) Console.WriteLine("FAILED");

            if (!_dbContext.Users.Any(x => x.ChatId == Chat.Id))
                _dbContext.Users.Add(new BotData() {ChatId = Chat.Id});

            await _dbContext.SaveChangesAsync();
            await Bot.SendTextMessageAsync(Chat, 
                $"Steam Connection Manager: `{json.Services.Where(p => (string)p[0] == "cms").Select(p => p[2]).FirstOrDefault()}`", 
                        ParseMode.Markdown);
        }
    }
}