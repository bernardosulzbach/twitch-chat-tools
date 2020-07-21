using System;
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

        private readonly SectionTimer _logHandlerTimer = new SectionTimer("OnLogHandler");

        private readonly StreamWriter _logStreamWriter;
        private readonly SectionTimer _messageReceivedHandlerTimer = new SectionTimer("OnMessageReceivedHandler");
        private readonly SectionTimer _whisperReceivedHandlerTimer = new SectionTimer("OnWhisperReceivedHandler");

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
            _logHandlerTimer.Start();
            _logStreamWriter.WriteLine($"[{e.DateTime:O}] {e.BotUsername} - {e.Data}");
            _logStreamWriter.Flush();
            _logHandlerTimer.Stop();
        }

        private void OnConnectedHandler(object sender, OnConnectedArgs e)
        {
            _chatStreamWriter.WriteLine($"[{DateTime.Now:O}] Connected to {e.AutoJoinChannel}.");
            _chatStreamWriter.Flush();
        }

        private void OnJoinedChannelHandler(object sender, OnJoinedChannelArgs e)
        {
            _chatStreamWriter.WriteLine($"[{DateTime.Now:O}] Joined the channel {e.Channel}.");
            _chatStreamWriter.Flush();
        }

        private void OnMessageReceivedHandler(object sender, OnMessageReceivedArgs e)
        {
            _messageReceivedHandlerTimer.Start();
            _chatStreamWriter.WriteLine($"[{DateTime.Now:O}] {e.ChatMessage.Username}: {e.ChatMessage.Message}");
            _chatStreamWriter.Flush();
            _messageReceivedHandlerTimer.Stop();
        }

        private void OnWhisperReceivedHandler(object sender, OnWhisperReceivedArgs e)
        {
            _whisperReceivedHandlerTimer.Start();
            _chatStreamWriter.WriteLine($"[{DateTime.Now:O}] {e.WhisperMessage.Username}: {e.WhisperMessage.Message}");
            _chatStreamWriter.Flush();
            _whisperReceivedHandlerTimer.Stop();
        }

        public void Stop()
        {
            _client.Disconnect();
            _chatStreamWriter.Dispose();
            _logStreamWriter.WriteLine(_logHandlerTimer.GetSummary());
            _logStreamWriter.WriteLine(_messageReceivedHandlerTimer.GetSummary());
            _logStreamWriter.WriteLine(_whisperReceivedHandlerTimer.GetSummary());
            _logStreamWriter.Dispose();
        }
    }
}