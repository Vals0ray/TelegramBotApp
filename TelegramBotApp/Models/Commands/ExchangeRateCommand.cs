using HtmlAgilityPack;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace TelegramBotApp.Models.Commands
{
    public class ExchangeRateCommand : Command
    {
        public override string Name => "exchange_rate";

        public override void Execute(Message message, TelegramBotClient client)
        {
            var chatId    = message.Chat.Id;
            var messageId = message.MessageId;
            var web       = new HtmlWeb();
            var html      = @"https://minfin.com.ua/ua/currency/lvov/";

            var htmlDoc = web.Load(html);

            var dolr = htmlDoc.DocumentNode.SelectSingleNode("//div/table[1]/tbody/tr[1]/td[3]/span/text()");
            var euro = htmlDoc.DocumentNode.SelectSingleNode("//div/table[1]/tbody/tr[2]/td[3]/span/text()");
            var rubl = htmlDoc.DocumentNode.SelectSingleNode("//div/table[1]/tbody/tr[3]/td[3]/span/text()");
            var zlot = htmlDoc.DocumentNode.SelectSingleNode("//div/table[1]/tbody/tr[4]/td[3]/span/text()");

            string buildString = "USD: " + dolr.InnerText.Substring(1) + '\n' +
                                 "EUR: " + euro.InnerText.Substring(1) + '\n' +
                                 "RUB: " + rubl.InnerText.Substring(1) + '\n' +
                                 "PLN: " + zlot.InnerText.Substring(1);

            client.SendTextMessageAsync(chatId, buildString, replyToMessageId: messageId);
        }
    }
}