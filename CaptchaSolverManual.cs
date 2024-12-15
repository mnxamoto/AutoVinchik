using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VkNet.Utils.AntiCaptcha;

namespace AutoVinchik
{
    public class CaptchaSolverManual : ICaptchaSolver
    {
        public void CaptchaIsFalse()
        {
            Console.WriteLine("Последняя капча была распознана неверно");
        }

        public string Solve(string url)
        {
            Console.WriteLine("Реши капчу");
            ProcessStartInfo processStartInfo = new ProcessStartInfo(@"C:\Program Files\Mozilla Firefox\firefox.exe");
            processStartInfo.Arguments = url;
            Process.Start(processStartInfo);
            return Console.ReadLine();
        }
    }
}
