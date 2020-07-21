using System;
using System.Globalization;
using System.IO;
using TwitchLib.Client;
using TwitchLib.Client.Events;
using TwitchLib.Client.Models;
using TwitchLib.Communication.Clients;
using TwitchLib.Communication.Models;

namespace FilteringChatLogger
{
    internal class ChatBot
    {
        private const string TextFileExtension = ".txt";

        private readonly StreamWriter _chatStreamWriter;
        private readonly TwitchClient _client;

        private readonly StreamWriter _logStreamWriter;

        public ChatBot(in Settings settings, string channel)
        {
            Directory.CreateDirectory(settings.LogPath);
            _logStreamWriter = new StreamWriter(Path.Combine(settings.LogPath, channel + TextFileExtension), true);

            var clientOptions = new ClientOptions
            {
                MessagesAllowedInPeriod = 750,
                ThrottlingPeriod = TimeSpan.FromSeconds(30)
            };
            var customClient = new WebSocketClient(clientOptions);
            _client = new TwitchClient(customClient);
            var credentials = new ConnectionCredentials(settings.Username, settings.OAuth);
            _client.Initialize(credentials, channel);

            _client.OnLog += OnLogHandler;
            _client.OnConnected += OnConnectedHandler;
            _client.OnJoinedChannel += OnJoinedChannelHandler;
            _client.OnMessageReceived += OnMessageReceivedHandler;
            _client.OnWhisperReceived += OnWhisperReceivedHandler;

            Directory.CreateDirectory(settings.ChatPath);
            _chatStreamWriter = new StreamWriter(Path.Combine(settings.ChatPath, channel + TextFileExtension), true);

            _client.Connect();
        }

        private void OnLogHandler(object sender, OnLogArgs e)
        {
            _logStreamWriter.WriteLine($"{e.DateTime.ToString(CultureInfo.InvariantCulture)}: {e.BotUsername} - {e.Data}");
            _logStreamWriter.Flush();
        }

        private void OnConnectedHandler(object sender, OnConnectedArgs e)
        {
            _chatStreamWriter.WriteLine($"-- Connected to {e.AutoJoinChannel}.");
            _chatStreamWriter.Flush();
            // TODO: Time these and write to the logs
        }

        private void OnJoinedChannelHandler(object sender, OnJoinedChannelArgs e)
        {
            _chatStreamWriter.WriteLine($"-- Joined the channel {e.Channel}.");
            _chatStreamWriter.Flush();
        }

        private void OnMessageReceivedHandler(object sender, OnMessageReceivedArgs e)
        {
            _chatStreamWriter.WriteLine($"{e.ChatMessage.Username}: {e.ChatMessage.Message}");
            _chatStreamWriter.Flush();
        }

        private void OnWhisperReceivedHandler(object sender, OnWhisperReceivedArgs e)
        {
            _chatStreamWriter.WriteLine($"{e.WhisperMessage.Username}: {e.WhisperMessage.Message}");
            _chatStreamWriter.Flush();
        }

        public void Stop()
        {
            _client.Disconnect();
            _chatStreamWriter.Dispose();
            _logStreamWriter.Dispose();
        }
    }
}