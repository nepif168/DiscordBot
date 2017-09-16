using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System.Threading.Tasks;
using System.Threading;
using System;
using System.Collections.Generic;
using System.Linq;

public class Test : ModuleBase
{
    // !say hello -> hello
    [Command("say"), Summary("Echos a message")]
    public async Task Say([Remainder, Summary("The text to echo")] string echo)
    {
        await ReplyAsync(echo);
    }

}