using System;
using System.IO;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramBot;

public class Program
{
    public static void Main(string[] args)
    {
        var botClient = new TelegramBotClient("5806252293:AAG9fAzGmC0ZkjWRoImQlmN_Lz46k3pXM6Q");
        var me = botClient.GetMeAsync();
        botClient.StartReceiving(Update, Error);
        Console.ReadLine();
    }

    
    static Task Error(ITelegramBotClient botClient, Exception exception, CancellationToken token)
    {
        throw new NotImplementedException();
    }

    async static Task Update(ITelegramBotClient botClient, Update update, CancellationToken token)
    {
        var message = update.Message;
     
        if (message.Text != null)
        {
            Console.WriteLine($"{message.Chat.FirstName} send message: {message.Text}");
            switch(message.Text)
            {
                case "/start":
                    {
                        await botClient.SendTextMessageAsync(message.Chat.Id, "Приветствую, выберите нужную команду в меню", replyMarkup: GetButtons());
                        break;
                    }

                case "Привет":
                    {
                        await botClient.SendTextMessageAsync(message.Chat.Id, "Добрый вечер, я интернешнл выиграл");
                        break;
                    }
                case "Скачать картинку":
                    {
                        break;
                    }
                case "Скачать видео":
                    {

                        break;
                    }
                default:
                    break;
            }

            return;
        }

        if (message.Photo != null)
        {
            await botClient.SendTextMessageAsync(message.Chat.Id, "Отправь лучше документом");
            return;

        }

        if (message.Document != null)
        {
            var fileId = update.Message.Document.FileId;
            var fileInfo = await botClient.GetFileAsync(fileId);
            var filePath = fileInfo.FilePath;

            string destinationFilePath = $@"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}\{message.Document.FileName}";
            FileStream fileStream = System.IO.File.OpenWrite(destinationFilePath);
            await botClient.DownloadFileAsync(filePath,fileStream);
            fileStream.Close();

            return;
        }
    }

    private static IReplyMarkup? GetButtons()
    {
        return new ReplyKeyboardMarkup( new List<KeyboardButton> { new KeyboardButton ("Скачать картинку") ,new KeyboardButton ("Скачать видео")});
    }
}