using System.Diagnostics;
using VkNet.Utils.AntiCaptcha;
using System;
using System.Linq;
using TwoCaptcha.Captcha;
using System.Net;

namespace AutoVinchik
{
    public class CaptchaSolverAuto : ICaptchaSolver
    {
        public void CaptchaIsFalse()
        {
            Console.WriteLine("Последняя капча была распознана неверно");
        }

        public string Solve(string url)
        {
            //https://rucaptcha.com

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Решается капча...");

            string filename = $"{Directory.GetCurrentDirectory()}/captcha.png";

            WebClient webClient = new WebClient();
            webClient.DownloadFile(url, filename);

            TwoCaptcha.TwoCaptcha solver = new TwoCaptcha.TwoCaptcha("b3aded8eaa2f84c6e33651642f33d943");
            Normal captcha = new Normal(filename);

            try
            {
                solver.Solve(captcha).Wait();
                Console.WriteLine("Капча решена: " + captcha.Code);
            }
            catch (AggregateException e)
            {
                Console.WriteLine("Error occurred: " + e.InnerExceptions.First().Message);
            }

            return captcha.Code;
        }
    }
}
