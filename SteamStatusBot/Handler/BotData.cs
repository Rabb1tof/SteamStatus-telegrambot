using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SteamStatusBot.Handler
{
    public class BotData
    {
        /// <summary>
        /// The chat identifier where this user is related.
        /// </summary>
        [Key]
        public long ChatId { get; set; }
    }
}
