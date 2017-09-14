using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Discord.Commands;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace DiscordBot
{
    class Program
    {
        // DiscordClient and options...
        DiscordSocketClient client;
        CommandService commands;
        IServiceProvider services;

        // Main => MainAsync
        static void Main(string[] args) => new Program().MainAsync().GetAwaiter().GetResult();

        // Main
        public async Task MainAsync()
        {
            var _config = new DiscordSocketConfig { MessageCacheSize = 100 };
            client = new DiscordSocketClient(_config);

            services = new ServiceCollection().BuildServiceProvider();

            await client.LoginAsync(TokenType.Bot, "bot token");
            await client.StartAsync();

            client.Ready += () =>
            {
                Console.WriteLine("Bot is connected!");
                return Task.FromResult(0);
            };

            await Task.Delay(-1);
        }
    }
}
