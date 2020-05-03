using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SteamStatusBot.Handler
{
    public class BotData
    {
        /// <summary>
        /// The chat identifier where this user is related.
        /// </summary>
        public long ChatId { get; set; }

        /// <summary>
        /// The Telegram User identifier.
        /// </summary>
        public int UserId { get; set; }
    }
}
