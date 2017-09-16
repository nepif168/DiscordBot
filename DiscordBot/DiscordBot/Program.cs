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
using System.IO;

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
            commands = new CommandService();
            
            services = new ServiceCollection().BuildServiceProvider();

            string path = Directory.GetCurrentDirectory();
            string botToken = File.ReadAllText(Path.Combine(path, "BotToken.txt"));
            Console.WriteLine(Path.Combine(path, "BotToken.txt"));
            Console.WriteLine(botToken);

            await InstallCommands();

            await client.LoginAsync(TokenType.Bot, botToken);
            await client.StartAsync();

            client.Ready += () =>
            {
                Console.WriteLine("Bot is connected!");
                return Task.FromResult(0);
            };

            // wait infinity
            await Task.Delay(-1);
        }

        // HandleCommand呼び出し
        public async Task InstallCommands()
        {
            client.MessageReceived += HandleCommand;
            await commands.AddModulesAsync(Assembly.GetEntryAssembly());
        }

        //よくわかんないけど...
        public async Task HandleCommand(SocketMessage messageParam)
        {
            var message = messageParam as SocketUserMessage;
            if (message == null) return;
            int argPos = 0;

            if (!(message.HasCharPrefix('!', ref argPos) || message.HasMentionPrefix(client.CurrentUser, ref argPos))) return;
            var context = new CommandContext(client, message);
            var result = await commands.ExecuteAsync(context, argPos, services);
            if (!result.IsSuccess)
                await context.Channel.SendMessageAsync(result.ErrorReason);
        }
    }
}
