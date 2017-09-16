using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System.Threading.Tasks;
using System.Threading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Net;

[Group("twitter")]
public class TwitterCommand : ModuleBase
{
    [Command("ranking"), Summary("twitter ranking")]
    public async Task Ranking([Summary("Tweet ranking")] int amount = 3)
    {
        WebClient wc = new WebClient();
        wc.Encoding = System.Text.Encoding.UTF8;
        wc.Headers.Add("Accept-Language", "ja"); //日本語要求
        wc.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
        string html = wc.DownloadString("https://tr.twipple.jp/tweet/");

        Regex reg = new Regex("<a href=\"(?<url>.*?)\".*?>(?<text>.*?)</a>",
                        RegexOptions.IgnoreCase | RegexOptions.Singleline);

        int i = 0;
        for ( Match m = reg.Match(html); m.Success; m = m.NextMatch())
        {
            string url = m.Groups["url"].Value;
            if (url.IndexOf("/status/") > 0)
            {
                i++;
                await ReplyAsync(url);
                if (i == amount)
                    break;
            }
        }
    }
}