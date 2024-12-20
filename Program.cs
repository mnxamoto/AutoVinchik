﻿using AutoVinchik;
using VkNet;
using VkNet.Enums.Filters;
using VkNet.Model;

VkApi vk = new VkApi(new Logger(), captchaSolver: new CaptchaSolverAuto());

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

int[] counts = File.ReadAllLines(@".\statistics.txt").Select(a => Convert.ToInt32(a)).ToArray();
int count_NoDescription = counts[0];
int count_NoMatch       = counts[1];
int count_YesMatch      = counts[2];
int count_Total         = counts[3];
int count_Repeat        = counts[4];

List<string> ids = File.ReadAllLines(@".\ids.txt").ToList();

while (true)
{
    Message message = vk.Messages.GetHistory(messagesGetHistoryParams).Messages.FirstOrDefault();
    string text = message.Text;

    Console.ForegroundColor = ConsoleColor.White;
    Console.WriteLine(text);

    if ((message.Attachments.Count != 0) && (ids.Contains(((Photo)message.Attachments[0].Instance).Sizes[0].Url.OriginalString)))
    {
        Pass();
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("Повтор. Пропускаем.");
        count_Repeat++;

        continue;
    }
    else if (!text.Contains('\n'))  //Пропускаем анкету, если описания нет вообще
    {
        Pass();
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Нет описания");
        count_NoDescription++;
    } 
    else if (keywords.Any(text.ToLower().Contains))
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

        while (text == vk.Messages.GetHistory(messagesGetHistoryParams).Messages.FirstOrDefault().Text)
        {
            Thread.Sleep(1000);
        }

        count_YesMatch++;

        File.WriteAllLines(@".\ids.txt", ids);

        File.WriteAllLines(
            @".\statistics.txt", 
            [$"{count_NoDescription}", $"{count_NoMatch}", $"{count_YesMatch}", $"{count_Total}", $"{count_Repeat}"]);
    }
    else
    {
        Pass();
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Нет совпадения по ключевым словам");
        count_NoMatch++;
    }

    if (message.Attachments.Count != 0)
    {
        ids.Add(((Photo)message.Attachments[0].Instance).Sizes[0].Url.OriginalString);
    }

    count_Total++;
    Console.Title = 
        $"Всего: {count_Total}. " +
        $"Без описания: {count_NoDescription}. " +
        $"Нет совпадения по ключевым словам: {count_NoMatch}. " +
        $"Есть совпадение: {count_YesMatch}. " +
        $"Повтор: {count_Repeat}.";

    Thread.Sleep(1000);
}

void Pass()
{
    MessagesSendParams messagesSendParams = new MessagesSendParams
    {
        Payload = "3",
        Message = "👎",
        PeerId = -91050183,
        RandomId = Environment.TickCount64
    };

    vk.Messages.Send(messagesSendParams);
}

Console.WriteLine("\r\nВсё");
Console.ReadKey();