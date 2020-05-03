using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore;
using SteamStatusBot.Handler;

namespace SteamStatusBot.Database
{
    public class DatabaseContext : DbContext
    {
        public DbSet<BotData> Users { get; set; }

        public override void Dispose()
        {
            SaveChanges();
            base.Dispose();
        }

        public override ValueTask DisposeAsync()
        {
            SaveChanges();
            return base.DisposeAsync();
        }

        public DatabaseContext(DbContextOptions<DatabaseContext> ctx) : base(ctx)
        {
            Database.Migrate();
        }
    }
}
