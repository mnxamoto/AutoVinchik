using AutoVinchik;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Net.Mail;
using System.Text.RegularExpressions;
using VkNet;
using VkNet.Enums.Filters;
using VkNet.Model;

VkApi vk = new VkApi(new Logger(), captchaSolver: new CaptchaSolverManual());

vk.Authorize(new ApiAuthParams
{
    ApplicationId = 2685278,
    AccessToken = "",
    Settings = Settings.All,
});

Console.WriteLine(vk.Token);

MessagesGetHistoryParams messagesGetHistoryParams = new MessagesGetHistoryParams
{
    PeerId = -91050183,  //Дайвинчик
    Count = 1
};

string[] keywords = File.ReadAllLines(@".\keywords.txt");

while (true)
{
    string text = vk.Messages.GetHistory(messagesGetHistoryParams).Messages.FirstOrDefault().Text;

    Console.ForegroundColor = ConsoleColor.White;
    Console.WriteLine(text);

    string[] lines = text.Split("\n");

    if ((text.Contains('\n')) && (keywords.Any(text.Contains)))  //(text.Contains('\n'))" - Пропускаем анкету, если описания нет вообще
    {
        string keyword = "";

        foreach (string keyword_i in keywords)
        {
            if (text.Contains(keyword_i))
            {
                keyword = keyword_i;
            }
        }

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"Ура, есть ключевое слово: {keyword}");
        Console.ReadKey();
    }
    else
    {
        MessagesSendParams messagesSendParams = new MessagesSendParams
        {
            Payload = "3",
            Message = "👎",
            PeerId = -91050183,
            RandomId = Environment.TickCount64
        };

        vk.Messages.Send(messagesSendParams);
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Нет совпадения по ключевым словам");
    }

    Thread.Sleep(300);
}

Console.WriteLine("\r\nВсё");
Console.ReadKey();