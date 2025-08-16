using System;
using System.IO;
using System.Linq;
using LLama;


namespace OfflineCodingBot.Helpers
{

    public static class LLamaHelper
    {
        public static void LogLLamaContextMethods()
        {
            var methods = typeof(LLamaContext).GetMethods()
                .Select(m => m.ToString())
                .OrderBy(m => m)
                .ToList();

            var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            var logFile = Path.Combine(desktopPath, "LLamaContextMethods.txt");

            File.WriteAllLines(logFile, methods);

            Console.WriteLine($"LLamaContext methods logged to {logFile}");
        }
    }

}

