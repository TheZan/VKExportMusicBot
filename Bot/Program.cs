using System;
using Telegram.Bot;
using System.Collections.Generic;
using System.IO;
using Telegram.Bot.Types.ReplyMarkups;
using System.Threading;

namespace Bot
{
    class Program
    {
        static TelegramBotClient bot;
        static General general;
        static List<AudioFile> audioFile = new List<AudioFile>();
        static void Main(string[] args)
        {
            Console.WriteLine("Вас приветствует VKExportMusicBot!");
            string[] auth = File.ReadAllText(@"auth.txt").Split(';');
            string token = auth[2];
            bot = new TelegramBotClient(token) { Timeout = TimeSpan.FromSeconds(10) };
            general = new General(auth[0], auth[1]);
            bot.OnMessage += Bot_OnMessage;
            bot.StartReceiving();
            Console.ReadKey();
            bot.StopReceiving();
        }

        private static void Bot_OnMessage(object sender, Telegram.Bot.Args.MessageEventArgs e)
        {
            if (e.Message.Type == Telegram.Bot.Types.Enums.MessageType.Text)
                BotCommand(e);
        }
        public static void BotCommand(Telegram.Bot.Args.MessageEventArgs e)
        {
            var keyboardStart = new ReplyKeyboardMarkup
            {
                Keyboard = new[]
                {   new KeyboardButton[]
                    {
                        "Из профиля"
                    },
                    new KeyboardButton[]
                    {
                        "Из поиска"
                    },
                }
            };
            if (e.Message.Text.StartsWith("https://"))
            {
                try
                {
                    audioFile = general.StartExportFromProfile(int.Parse(e.Message.Text.Substring(17)));
                    foreach (var item in audioFile)
                    {
                        bot.SendAudioAsync(
                              chatId: e.Message.Chat,
                              audio: $"{item.url}",
                              caption: $"{item.performer} - {item.title}"
                              );
                        Console.WriteLine($"{item.performer} - {item.title}");
                        Thread.Sleep(3000);
                    }
                }
                catch
                {
                    bot.SendTextMessageAsync(e.Message.Chat, "Неверная ссылка на профиль или аудиозаписи закрыты!").ConfigureAwait(false);
                }
            }
            else if (e.Message.Text.StartsWith("/search"))
            {
                try
                {
                    bot.SendTextMessageAsync(e.Message.Chat, "Сейчас будут выведены результаты поиска.").ConfigureAwait(false);
                    audioFile = general.StartSearch(e.Message.Text.Substring(8));
                    foreach (var item in audioFile)
                    {
                        bot.SendAudioAsync(
                              chatId: e.Message.Chat,
                              audio: $"{item.url}",
                              caption: $"{item.performer} - {item.title}"
                              );
                        Console.WriteLine($"Пользователь {e.Message.From.FirstName} загрузил: {item.performer} - {item.title}");
                        Thread.Sleep(3000);
                    }
                }
                catch
                {
                    bot.SendTextMessageAsync(e.Message.Chat, "Введите поисковой запрос!").ConfigureAwait(false);
                }
            }
            else if(e.Message.Text == "/start")
            {
                bot.SendTextMessageAsync(e.Message.Chat, "Добро пожаловать!\nВыберите откуда хотите загрузить музыку.", replyMarkup: keyboardStart).ConfigureAwait(false);
            }
            else if (e.Message.Text == "Из профиля")
            {
                bot.SendTextMessageAsync(e.Message.Chat.Id, "Введите ссылку на прифиль.\nВнимание! Аудиозаписи должны быть доступны для всех пользователей!\n\nОбразец:\nhttps://vk.com/id12345").ConfigureAwait(false);
            }
            else if (e.Message.Text == "Из поиска")
            {
                bot.SendTextMessageAsync(e.Message.Chat.Id, "Введите поисковой запрос.\nОбразец:\n\n/search ЗАПРОС").ConfigureAwait(false);
            }
            else
            {
                bot.SendTextMessageAsync(e.Message.Chat.Id, "Я не знаю такой команды...").ConfigureAwait(false);
            }
        }
    }
}
