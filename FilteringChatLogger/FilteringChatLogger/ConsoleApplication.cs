using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            }
            catch
            {
                Console.WriteLine("Failed to read text from JSON file.");
                return;
            }

            var channel = args[1];

            var chatBot = new ChatBot(settings, channel);

            var commandList = new List<Command>();
            commandList.Add(new Command("help", "Prints the available commands.", chatBot =>
            {
                var maxLength = commandList.Select(command => command.Name.Length).Max();
                foreach (var command in commandList) Console.WriteLine($"  {command.Name.PadRight(maxLength + 10)} {command.Description}");
            }));
            commandList.Add(new Command("stop", "Stops the client.", chatBot => { chatBot.Stop(); }));
            Console.WriteLine("Enter help to see the available commands.");
            while (chatBot.IsRunning)
            {
                Console.Write("> ");
                var line = Console.ReadLine();
                var foundMatchingCommand = false;
                foreach (var command in commandList)
                    if (command.Matches(line))
                    {
                        foundMatchingCommand = true;
                        command.Execute(chatBot);
                        break;
                    }

                if (!foundMatchingCommand) Console.WriteLine("No command matched that.");
            }
        }

        private static void WriteUsageToConsole()
        {
            Console.WriteLine("Pass two arguments: a path to a JSON settings file and the channel name.");
        }
    }
}