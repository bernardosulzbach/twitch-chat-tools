using System;
using System.IO;
using System.Text;

namespace FilteringChatLogger
{
    public static class ConsoleApplication
    {
        private static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                WriteUsageToConsole();
                return;
            }

            var pathToSettings = args[0];
            Settings settings;
            try
            {
                var jsonText = File.ReadAllText(pathToSettings, Encoding.UTF8);
                settings = Settings.FromJson(jsonText);
                Console.WriteLine(settings.ChatPath);
            }
            catch
            {
                Console.WriteLine("Failed to read text from JSON file.");
                return;
            }

            var channel = args[1];

            var bot = new ChatBot(settings, channel);
            Console.WriteLine("Enter 'stop' to stop the application.");
            while (true)
            {
                Console.Write("> ");
                var line = Console.ReadLine();
                if (line == "stop")
                {
                    Console.WriteLine("Stopping the bot...");
                    bot.Stop();
                    break;
                }

                Console.WriteLine("Could not understand that.");
            }
        }

        private static void WriteUsageToConsole()
        {
            Console.WriteLine("Pass two arguments: a path to a JSON settings file and the channel name.");
        }
    }
}