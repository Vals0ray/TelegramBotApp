﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot;
using TelegramBotApp.Models.Commands;

namespace TelegramBotApp.Models
{
    public static class Bot
    {
        private static TelegramBotClient client;
        private static List<Command> commandsList;

        public static IReadOnlyList<Command> Commands => commandsList.AsReadOnly();

        public static async Task<TelegramBotClient> Get()
        {
            if(client != null)
            {
                return client;
            }

            commandsList = new List<Command>();
            commandsList.Add(new HelloCommand());
            commandsList.Add(new ExchangeRateCommand());
            commandsList.Add(new RandomImageCommand());
            commandsList.Add(new RandomMemeCommand());

            client = new TelegramBotClient(AppSettings.Key);
            var hock = string.Format(AppSettings.URL, "api/message/update");
            await client.SetWebhookAsync(hock);

            return client;
        }
    }
}