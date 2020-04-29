using BotFramework;
using BotFramework.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types.Enums;
using SteamStatusBot.Handler;

namespace SteamStatusBot.Handler
{
    public class Commands : BotEventHandler
    {
        /// <summary>
        /// пока просто уберу.
        /// </summary>
        public void OnStatusUsed()
        {
            Console.WriteLine("Client used command 'status'");
            // await new HanldersCommands().SendStatus();
            //await Bot.SendTextMessageAsync(Chat, "Test", ParseMode.Markdown);
        }
    }
}
