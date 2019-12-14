using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace TelegramBotApp.Models.Commands
{
    public class RandomImageCommand : Command
    {
        private readonly string topic = "";
        private Random rnd = new Random();

        public RandomImageCommand() => topic = GetRandomTopic();

        public override string Name => "random_image";

        public override void Execute(Message message, TelegramBotClient client)
        {
            var chatId    = message.Chat.Id;

            string html       = GetHtmlCode();
            List<string> urls = GetUrls(html);

            int randomIndex = rnd.Next(0, urls.Count - 1);

            string luckyUrl = urls[randomIndex];

            client.SendPhotoAsync(chatId, luckyUrl, luckyUrl);
        }

        //Creating a random word of 6 letters
        private string GetRandomTopic()
        {
            string result = "";

            //A B C D E F G H I J K L M N O P Q R S T U V W X Y Z
            string[] alphabetV = { "B", "C", "D", "F", "G", "H", "J", "K", "L", "M", "N", "P", "Q", "R", "S", "T", "V", "W", "X", "Z" };
            string[] alphabetC = { "A", "E", "I", "O", "U", "Y" };

            for (int i = 0; i < 6; i++)
            {
                result += i % 2 == 0 ? 
                    alphabetV[rnd.Next(0, alphabetV.Length - 1)] :
                    alphabetC[rnd.Next(0, alphabetC.Length - 1)] ;
            }

            return result;
        }

        // Getting an HTML page from Google search according to the word generated
        private string GetHtmlCode()
        {
            string url = "https://www.google.com/search?q=" + topic + "&tbm=isch";
            string data = "";

            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Accept    = "text/html, application/xhtml+xml, */*";
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