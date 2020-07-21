using System;
using FilteringChatLogger;
using NUnit.Framework;

namespace FilteringChatLoggerTests
{
    public class SettingsTests
    {
        [Test]
        public void TestFromJsonValidation()
        {
            Assert.DoesNotThrow(ValidFromJson);
            Assert.Throws<ArgumentException>(FromJsonWithMissingUsername);
            Assert.Throws<ArgumentException>(FromJsonWithMissingOAuthPrefix);
            Assert.Throws<ArgumentException>(FromJsonWithOAuthTooShort);
            Assert.Throws<ArgumentException>(FromJsonWithOAuthTooLong);
        }

        private static void ValidFromJson()
        {
            Settings.FromJson(@"{""username"": ""user"", ""password"": ""oauth:000000000000000000000000000000"", ""path-to-chat-files"": ""/var/chat"", ""path-to-log-files"": ""/var/log""}");
        }

        private static void FromJsonWithMissingUsername()
        {
            Settings.FromJson(@"{""password"": ""oauth:000000000000000000000000000000"", ""path-to-chat-files"": ""/var/chat"", ""path-to-log-files"": ""/var/log""}");
        }

        private static void FromJsonWithMissingOAuthPrefix()
        {
            Settings.FromJson(@"{""username"": ""user"", ""password"": ""000000000000000000000000000000"", ""path-to-chat-files"": ""/var/chat"", ""path-to-log-files"": ""/var/log""}");
        }

        private static void FromJsonWithOAuthTooShort()
        {
            Settings.FromJson(@"{""username"": ""user"", ""password"": ""oauth:00000000000000000000000000000"", ""path-to-chat-files"": ""/var/chat"", ""path-to-log-files"": ""/var/log""}");
        }

        private static void FromJsonWithOAuthTooLong()
        {
            Settings.FromJson(@"{""username"": ""user"", ""password"": ""oauth:0000000000000000000000000000000"", ""path-to-chat-files"": ""/var/chat"", ""path-to-log-files"": ""/var/log""}");
        }
    }
}