using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SteamStatusBot.Extension
{
    public static class StartupExt
    {
        public static IServiceCollection AddBag<T>(this IServiceCollection collection)
           => collection.AddSingleton<ConcurrentBag<T>>();
    }
}
