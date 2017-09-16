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


    // !group -> x(x = amount)
    // !join -> join to group
    static int playerAmount;
    static bool IsGrouping = false;
    static List<string> players;
    [Command("group"), Summary("Create two groups")]
    public async Task Group([Remainder, Summary("how many people")] string number)
    {
        int amount;
        if (number == "stop")
        {
            await ReplyAsync("投票を中止します");
            IsGrouping = false;
            return;
        }
        if (!int.TryParse(number, out amount))
        {
            await ReplyAsync("!group [人数] の形式で書いてください");
            return;
        }
        if (amount % 2 == 1)
        {
            await ReplyAsync("人数は偶数になるようにしてください");
            return;
        }
        if (IsGrouping)
        {
            await ReplyAsync("投票受付中です。中止は !group stop です");
            return;
        }

        playerAmount = amount;
        IsGrouping = true;
        players = new List<string>();
        await ReplyAsync("受付を開始します。参加者は !join コマンドをチャットに記入してください");
    }

    [Command("join"), Summary("join group command")]
    public async Task Join()
    {
        Console.WriteLine(playerAmount);
        if (!IsGrouping)
        {
            await ReplyAsync("投票中ではありません");
            return;
        }

        players.Add(Context.User.Username);

        if (players.Count == playerAmount)
        {
            await ReplyAsync("###結果発表###");
            List<string> shufflePlayers = players.OrderBy(i => Guid.NewGuid()).ToList();
            await ReplyAsync("Group A");
            for (int i = 0; i < playerAmount / 2; i++)
                await ReplyAsync("+ " + shufflePlayers[i]);
            await ReplyAsync("Group B");
            for (int i = playerAmount / 2; i < playerAmount; i++)
                await ReplyAsync("+ " + shufflePlayers[i]);
            IsGrouping = false;
        }
    }
}