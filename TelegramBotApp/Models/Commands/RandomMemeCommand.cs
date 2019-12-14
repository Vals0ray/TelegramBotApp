using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace TelegramBotApp.Models.Commands
{
    public class RandomMemeCommand : Command
    {
        private readonly List<string> _topics = new List<string> { "meme", "memes", "funnyImage" };
        private Random rnd = new Random();

        public override string Name => "random_meme";

        public override void Execute(Message message, TelegramBotClient client)
        {
            var chatId    = message.Chat.Id;

            string html = GetHtmlCode();
            var urls    = GetUrls(html);

            int randomIndex = rnd.Next(0, urls.Count - 1);

            string luckyUrl = urls[randomIndex];

            client.SendPhotoAsync(chatId, luckyUrl, luckyUrl);
        }

        // Getting an HTML page from Google search according to the words created
        private string GetHtmlCode()
        {
            int topic = rnd.Next(0, _topics.Count - 1);

            string url = "https://www.google.com/search?q=" + _topics[topic] + "&tbm=isch";
            string data = "";

            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Accept = "text/html, application/xhtml+xml, */*";
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; Trident/7.0; rv:11.0) like Gecko";

            var response = (HttpWebResponse)request.GetResponse();

            using (Stream dataStream = response.GetResponseStream())
            {
                if (dataStream == null)
                    return "";
                using (var sr = new StreamReader(dataStream))
                {
                    data = sr.ReadToEnd();
                }
            }
            return data;
        }

        // Getting all image URLs from html page
        private List<string> GetUrls(string html)
        {
            var urls = new List<string>();

            int ndx = html.IndexOf("\"ou\"", StringComparison.Ordinal);

            while (ndx >= 0)
            {
                ndx = html.IndexOf("\"", ndx + 4, StringComparison.Ordinal);
                ndx++;
                int ndx2 = html.IndexOf("\"", ndx, StringComparison.Ordinal);
                string url = html.Substring(ndx, ndx2 - ndx);
                urls.Add(url);
                ndx = html.IndexOf("\"ou\"", ndx2, StringComparison.Ordinal);
            }
            return urls;
        }
    }
}