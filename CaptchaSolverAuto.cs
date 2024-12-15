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
            string filename = $"{Directory.GetCurrentDirectory()}/captcha.png";

            WebClient webClient = new WebClient();
            webClient.DownloadFile(url, filename);

            TwoCaptcha.TwoCaptcha solver = new TwoCaptcha.TwoCaptcha("da34f8f0683dc0553438e91f0cd435e6");
            Normal captcha = new Normal(filename);

            try
            {
                solver.Solve(captcha).Wait();
                Console.WriteLine("Captcha solved: " + captcha.Code);
            }
            catch (AggregateException e)
            {
                Console.WriteLine("Error occurred: " + e.InnerExceptions.First().Message);
            }

            return captcha.Code;
        }
    }
}
